using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using TCPClient;
using Newtonsoft.Json;

namespace Automatick.Core
{
    public class TixToxProxyListener
    {
        #region members
        private TcpListener _listener;
        private String _listeningIP = String.Empty;
        private Thread _listenerThread;
        public IPAddress IPAdress
        {
            get
            {
                IPAddress addr = IPAddress.Loopback;
                if (!String.IsNullOrEmpty(this._listeningIP))
                    IPAddress.TryParse(this._listeningIP, out addr);

                return addr;
            }
        }
        public int PortNumber
        {
            get;
            set;
        }
        public Proxy Proxy
        {

            get;
            set;
        }

        #endregion

        #region methods
        public Boolean start()
        {
            try
            {
                _listener = new TcpListener(this.IPAdress, this.PortNumber);

                Console.ForegroundColor = ConsoleColor.Yellow;

                _listener.Start();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                return false;
            }

            _listenerThread = new Thread(new ParameterizedThreadStart(this.Listen));
            _listenerThread.Start(_listener);

            return true;
        }
        public void Listen(object obj)
        {
            try
            {
                while (true)
                {
                    TcpListener listener = (TcpListener)obj;

                    TcpClient client = listener.AcceptTcpClient();

                    Thread th = new Thread(new ParameterizedThreadStart(ProcessClient));
                    th.IsBackground = true;
                    th.Start(client);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public void ProcessClient(object obj)
        {
            try
            {
                Debug.WriteLine("Client Connected");

                TcpClient client = (TcpClient)obj;
                NetworkStream stream = client.GetStream();

                try
                {
                    // Set timeouts for the read and write to 5 seconds.
                    stream.ReadTimeout = 30000 * 1000;
                    stream.WriteTimeout = 30000 * 1000;

                    TCPEncryptor encryptor = new TCPEncryptor();
                    try
                    {
                        String message = ReadMessage(stream);
                        message = TCPEncryptor.Decrypt(message);

                        ClientProxy clientProxy = JsonConvert.DeserializeObject<ClientProxy>(message);

                        if (clientProxy != null)
                        {
                            if (clientProxy.Command.Equals("getNextProxies"))
                            {
                                Proxy = ProxyPicker.ProxyPickerInstance.getNextProxyForTixTox();

                                if (Proxy != null)
                                {
                                    if ((!String.IsNullOrEmpty(Proxy.Address)) && (!String.IsNullOrEmpty(Proxy.Port)))
                                    {
                                        clientProxy.Host = Proxy.Address;
                                        clientProxy.Port = Proxy.Port;

                                        if ((!String.IsNullOrEmpty(Proxy.UserName)) && (!String.IsNullOrEmpty(Proxy.Password)))
                                        {

                                            clientProxy.Username = Proxy.UserName;
                                            clientProxy.Password = Proxy.Password;

                                        }
                                        if (Proxy.IfLuminatiProxy)
                                        {
                                            clientProxy.LuminatiSessionId = Proxy.LuminatiSessionId;
                                            clientProxy.TypeProxy = ProxyType.MyIP;
                                        }

                                        byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(clientProxy)) + "<EOF>");

                                        stream.Write(buffer, 0, buffer.Length);


                                    }
                                    else
                                    {
                                        Debug.WriteLine("Proxies Address and port are empty");
                                        byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(new ClientProxy("Proxies Address and port are empty"))) + "<EOF>");

                                        stream.Write(buffer, 0, buffer.Length);
                                        stream.Close();
                                        client.Close();
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("getNextProxyForTixTox returned null");
                                    byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(new ClientProxy("getNextProxyForTixTox returned null"))) + "<EOF>");

                                    stream.Write(buffer, 0, buffer.Length);
                                    stream.Close();
                                    client.Close();
                                }
                            }
                            else
                            {
                                Debug.WriteLine("Invalid Command");
                                byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(new ClientProxy("Invalid Command"))) + "<EOF>");

                                stream.Write(buffer, 0, buffer.Length);
                                stream.Close();
                                client.Close();
                            }
                        }
                        else
                        {
                            Debug.WriteLine("object recieved is null");
                            byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(new ClientProxy("object recieved is null"))) + "<EOF>");

                            stream.Write(buffer, 0, buffer.Length);
                            stream.Close();
                            client.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);

                        Console.Out.WriteLine(e.Message);

                        if (e.Message.Contains("403"))
                        {
                            //WriteLog(e);
                        }

                        byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(new ClientProxy(e.Message))) + "<EOF>");

                        stream.Write(buffer, 0, buffer.Length);
                        stream.Close();
                        client.Close();

                        return;
                    }

                    finally
                    {
                        stream.Close();
                        client.Close();

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }
        static string ReadMessage(NetworkStream stream)
        {
            // Read the  message sent by the client.
            // The client signals the end of the message using the
            // "<EOF>" marker.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                // Read the client's test message.
                bytes = stream.Read(buffer, 0, buffer.Length);

                // Use Decoder class to convert from bytes to UTF8
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                // Check for EOF or an empty message.
                if (messageData.ToString().IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            return messageData.ToString().Substring(0, messageData.ToString().IndexOf("<EOF>"));
        }
        public void Stop()
        {
            try
            {
                _listener.Stop();

                if (_listenerThread != null)
                {
                    if (_listenerThread.IsAlive)
                    {
                        _listenerThread.Abort();
                    }
                }
            }
            catch { }
        }
        #endregion

        #region constructor
        public TixToxProxyListener(String _ipAdress, int _portNumber)
        {
            this._listeningIP = _ipAdress;
            this.PortNumber = _portNumber;

        }
        #endregion
    }
}
