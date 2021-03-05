using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LotGenerator_Core
{
    public class Util
    {
        private static String mainServer = "mainserver.ticketpeers.com";
        private static TcpListener _listener;
        private static Thread _listenerThread;
        private static Boolean isRunning = false;
        private static System.Threading.Timer _connectionTimer = null;

        public static TcpClient client = null;
        public static String Key = String.Empty;

        public static ConcurrentQueue<BrowserRequest> ClientRequests = null;

        public static String InitializeClient()
        {
            String result = String.Empty;
            ClientRequests = new ConcurrentQueue<BrowserRequest>();
            _connectionTimer = new System.Threading.Timer(Pinger, null, 10 * 1000, 10 * 1000);
            isRunning = true;

            try
            {
                String address = "127.0.0.1";
                Key = UniqueKey.getUniqueKey();

                IPAddress ipAddress = IPAddress.Loopback;
                IPAddress.TryParse(address, out ipAddress);
                _listener = new TcpListener(ipAddress, 12345);
                _listener.Start();

                _listenerThread = new Thread(new ParameterizedThreadStart(LotMainServiceWork));
                _listenerThread.Start(_listener);

                result = createConnectionToServer();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return result;
        }

        private static void Pinger(object state)
        {
            try
            {
                NetworkStream stream = null;

                if (client == null || !client.Connected)
                {
                    //client = new TcpClient(mainServer, 9000);
                }

                stream = client.GetStream();

                BrowserRequest browserReq = new BrowserRequest();
                browserReq.Command = "Ping";
                browserReq.ID = Key;

                byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(browserReq)) + "<EOF>");

                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //client = new TcpClient(mainServer, 9000);
            }
        }

        public static void CloseClient()
        {
            try
            {
                isRunning = false;

                if (_listener != null)
                {
                    _listener.Stop();
                }

                if (client != null)
                {
                    client.Close();
                }

                if (_listenerThread != null)
                {
                    _listenerThread.Abort();
                }

                if (_connectionTimer != null)
                {
                    _connectionTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _connectionTimer.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static String createConnectionToServer()
        {
            String message = String.Empty;
            try
            {
                if (isRunning)
                {
                    client = new TcpClient(mainServer, 9000);
                    NetworkStream stream = client.GetStream();

                    BrowserRequest browserReq = new BrowserRequest();
                    browserReq.Command = "getConfig";
                    browserReq.ID = Key;

                    byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(browserReq)) + "<EOF>");

                    stream.Write(buffer, 0, buffer.Length);

                    message = Encryptor.ReadMessage(stream);

                    message = Encryptor.Decrypt(message);

                    isConnected = true;

                    Thread th = new Thread(takeServerRequests);

                    th.Start();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return message;
        }

        private static Boolean isConnected = false;

        private static void takeServerRequests(object obj)
        {
            try
            {
                try
                {
                    while (isConnected)
                    {
                        string message = Encryptor.ReadMessage(client.GetStream());

                        try
                        {
                            message = Encryptor.Decrypt(message);

                            if (!String.IsNullOrEmpty(message))
                            {
                                BrowserRequest req = JsonConvert.DeserializeObject<BrowserRequest>(message);

                                if (req.Command.Equals("startEvent"))
                                {
                                    ClientRequests.Enqueue(req);
                                }
                                else if (req.Command.Equals("isAlive"))
                                {
                                    //Dono nothing its for server
                                    Debug.WriteLine(message);
                                }
                                else
                                {

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        if (client != null || !client.Connected)
                            client.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    isConnected = false;
                    createConnectionToServer();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void LotMainServiceWork(Object obj)
        {
            try
            {
                TcpListener serverListener = (TcpListener)obj;

                try
                {
                    while (true)
                    {
                        TcpClient client = serverListener.AcceptTcpClient();

                        Thread th = new Thread(new ParameterizedThreadStart(handleLotMainServiceClient));
                        th.Priority = ThreadPriority.Highest;
                        th.IsBackground = true;
                        th.Start(client);
                    }
                }
                catch (Exception e)
                {
                    //Log e
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void handleLotMainServiceClient(object obj)
        {
            TcpClient _client = (TcpClient)obj;
            NetworkStream stream = _client.GetStream();

            try
            {
                //stream.ReadTimeout = 30000;
                //stream.WriteTimeout = 30000;

                string message = String.Empty;

                //while (true)
                {
                    message = Encryptor.Decrypt(Encryptor.ReadMessage(stream));

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Out.WriteLine("Received: " + message);
                    Debug.WriteLine("Received: " + message);

                    BrowserRequest request = null;
                    request = JsonConvert.DeserializeObject<BrowserRequest>(message);

                    if (request != null)
                    {
                        Boolean verify = true;

                        if (request.Command.Equals("updateLot"))
                        {
                            BrowserRequest req = new BrowserRequest();
                            req.Command = "UpdateLot";
                            req.AppPrefix = request.AppPrefix;
                            req.LotID = request.LotID;
                            req.ID = Key;

                            try
                            {
                                if (client == null || !client.Connected)
                                {
                                    client = new TcpClient(mainServer, 9000);
                                    stream = client.GetStream();
                                }

                                byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(req)) + "<EOF>");
                                client.GetStream().Write(buffer, 0, buffer.Length);
                            }
                            catch (Exception ex)
                            {
                                if (client == null || !client.Connected)
                                {
                                    client = new TcpClient(mainServer, 9000);
                                    stream = client.GetStream();
                                }

                                byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(req)) + "<EOF>");
                                client.GetStream().Write(buffer, 0, buffer.Length);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Log e
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Out.WriteLine(e.Message);
            }
            finally
            {
                stream.Close();
                _client.Close();
            }
        }

        public static void ReleaseProxy(String proxy)
        {
            try
            {
                BrowserRequest req = new BrowserRequest() { Proxy = proxy, Command = "releaseProxy", ID = Key };
                byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(req)) + "<EOF>");
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }

        #region OldOne
        public static string getEvent()
        {
            String result = String.Empty;

            try
            {
                //TODO: update server address here
                HttpWebRequest request = HttpWebRequest.CreateHttp("http://" + mainServer + ":9000/getEvent");
                request.Method = "GET";
                String response = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

                result = response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public static string getConfig()
        {
            String result = String.Empty;

            try
            {
                //TODO: update server address here
                HttpWebRequest request = HttpWebRequest.CreateHttp("http://" + mainServer + ":9000/getConfig");
                request.Method = "GET";
                String response = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

                result = response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public static string updateID(String id, String appPrefix)
        {
            String result = String.Empty;

            try
            {
                //TODO: update server address here
                HttpWebRequest request = HttpWebRequest.CreateHttp("http://" + mainServer + ":9000/UpdateLotID?lotID=" + id + "&appPrefix=" + appPrefix);
                request.Method = "GET";
                String response = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

                result = response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }
        #endregion
    }
}
