using LotID_Core;
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

namespace LotGenerator
{
    class Program
    {
        private static Timer _refreshCountdownTimer = null;
        private static ConcurrentDictionary<String, ClientRequest> _clientUrlList = null;
        private static List<Process> _processes = null;
        private static List<Process> _notStartedProcess = null;
        private static List<String> _urlList = null;
        private static String port = "44500";
        private static Boolean isProcessRunning = false;

        private static TcpListener _listener = null;
        private static Thread _listenerThread = null;

        private static int usingResourceForTokensGenerationsOnRequest = 0;
        private static int usingResourceOnRequest = 0;

        private static int getTokenOnRequestCount
        {
            get
            {
                int tokenCount = 0;// Convert.ToInt32(InitialTokenPool) + 1;

                if (!_clientUrlList.IsEmpty)
                {
                    if (0 == Interlocked.Exchange(ref usingResourceOnRequest, 1))
                    {
                        tokenCount = _clientUrlList.Count + usingResourceForTokensGenerationsOnRequest;

                        //Release the lock
                        Interlocked.Exchange(ref usingResourceOnRequest, 0);
                    }
                }

                return tokenCount;
            }
        }

        public static Boolean generateMoreOnRequestToken
        {
            get
            {
                if (getTokenOnRequestCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        static void Main(string[] args)
        {
            _clientUrlList = new ConcurrentDictionary<String, ClientRequest>();
            _processes = new List<Process>();
            _notStartedProcess = new List<Process>();
            _urlList = new List<String>();
            intializeListener();
        }

        private static void intializeListener()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Loopback;
                ipAddress = getLocalIpAddress();
                _listener = new TcpListener(ipAddress, Convert.ToInt32(port));
                _listener.Start();

                _listenerThread = new Thread(new ParameterizedThreadStart(work));
                _listenerThread.Start(_listener);

                load();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static IPAddress getLocalIpAddress()
        {
            IPHostEntry host;
            IPAddress localIP = IPAddress.Loopback;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip;
                    break;
                }
            }
            return localIP;
        }

        private static void work(object obj)
        {
            TcpListener serverListener = (TcpListener)obj;

            try
            {
                while (true)
                {
                    TcpClient client = serverListener.AcceptTcpClient();

                    Thread th = new Thread(new ParameterizedThreadStart(handleClient));
                    th.Priority = ThreadPriority.Highest;
                    th.IsBackground = true;
                    th.Start(client);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private static void handleClient(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            try
            {
                string messageData = TCPEncryptor.Decrypt(ReadMessage(stream));
                try
                {
                    ClientRequest msg = JsonConvert.DeserializeObject<ClientRequest>(messageData);

                    if (String.IsNullOrEmpty(msg.URL))
                    {
                        BrowserRequest tempMsg = JsonConvert.DeserializeObject<BrowserRequest>(messageData);
                        int tempProcessID = int.Parse(tempMsg.ProcessID);

                        if (!string.IsNullOrEmpty(tempMsg.WaitingURL))
                            _urlList.Add(tempMsg.WaitingURL);

                        Boolean ifExist = _processes.Exists(pre => pre.Id.Equals(tempProcessID));

                        //Process temProcess = _processes.FirstOrDefault(pre => pre.Id.Equals(tempProcessID));

                        if (ifExist)
                        {
                            try
                            {
                                _processes.Remove(_processes.FirstOrDefault(pre => pre.Id.Equals(tempProcessID)));
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Out.WriteLine("Problem In removing Process - HandleClient\n" + e.Message + "\n" + e.InnerException + "\n" + e.StackTrace);
                            }
                        }

                        if (_notStartedProcess != null && _notStartedProcess.Count > 0)
                        {
                            start(_notStartedProcess.FirstOrDefault());
                        }
                        else
                            isProcessRunning = false;

                    }
                    else
                    {
                        ClientRequest tempMsg = JsonConvert.DeserializeObject<ClientRequest>(messageData);
                        if (!String.IsNullOrEmpty(tempMsg.URL) && !_urlList.Contains(tempMsg.URL))
                        {
                            if (tempMsg.URL.ToLower().Contains("priority") && tempMsg.URL.ToLower().Contains("tickets.axs.com"))
                            {

                            }
                            else
                            {
                                if (!_clientUrlList.ContainsKey(tempMsg.URL))
                                {
                                    //_urlList.Add(tempMsg.URL);
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " URL --->\n" + tempMsg.URL + "\n\n");
                                    _clientUrlList.TryAdd(tempMsg.URL, tempMsg);
                                }
                            }
                        }
                    }

                    stream.Close();
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Out.WriteLine(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Out.WriteLine(e.Message + "\n" + "Client Count " + _clientUrlList.Count + "\n" + e.InnerException + "\n" + e.StackTrace);
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }

        private static string ReadMessage(NetworkStream stream)
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

        public static void load()
        {
            try
            {
                if (_refreshCountdownTimer == null)
                {
                    TimerCallback tcb = refreshCountdownTimerCallBackHandler;
                    _refreshCountdownTimer = new Timer(tcb, null, 1000 * 2, 1000 * 2);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private static void refreshCountdownTimerCallBackHandler(object sender)
        {
            try
            {
                if (generateMoreOnRequestToken)
                {
                    String fileName = getClientRequest();

                    if (!string.IsNullOrEmpty(fileName))
                        start(fileName);

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.Out.WriteLine(e.Message + "\n" + "Process Count " + e.InnerException + "\n" + e.StackTrace);
            }
        }

        private static string getClientRequest()
        {
            String FileName = String.Empty;
            try
            {
                String key = _clientUrlList.First().Key;
                ClientRequest req = null;
                Boolean result = _clientUrlList.TryRemove(key, out req);

                if (result)
                {
                    FileName = System.Windows.Forms.Application.StartupPath + @"\" + DateTime.Now.Ticks + ".setting";

                    var json = JsonConvert.SerializeObject(req);

                    using (TextWriter _writer = new StreamWriter(FileName))
                    {
                        _writer.Write(json);
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace);
            }

            return FileName;
        }

        private static void start(String fileName)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.Arguments = "\"" + fileName + "\"";
                process.StartInfo.FileName = System.Windows.Forms.Application.StartupPath + @"\LotBrowser.exe";
                process.StartInfo.WorkingDirectory = System.Windows.Forms.Application.StartupPath;
                process.EnableRaisingEvents = true;

                if (!isProcessRunning)
                {
                    process.Start();
                    isProcessRunning = true;
                    lock (_processes)
                    {
                        _processes.Add(process);
                    }
                }
                else
                {
                    _notStartedProcess.Add(process);
                }

            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Problem in start(fileName)\n" + e.Message + "\n" + e.InnerException + "\n" + e.StackTrace);
            }

        }

        private static void start(Process process)
        {

            try
            {
                _notStartedProcess.Remove(process);
                process.Start();
                isProcessRunning = true;
                lock (_processes)
                {
                    _processes.Add(process);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("Problem In removing Process - Start(PROCESS)\n" + e.Message + "\n" + e.InnerException + "\n" + e.StackTrace);
            }

        }

        private static void process_Exited(object sender, System.EventArgs ex)
        {
            try
            {
                Process exitedProcess = (Process)sender;
                if (exitedProcess != null)
                {
                    Process processToStart = null;

                    processToStart = _processes.FirstOrDefault(p => p == exitedProcess);
                    if (processToStart != null)
                    {
                        if (!processToStart.HasExited)
                        {
                            processToStart.Kill();
                            processToStart.WaitForExit();
                        }

                        processToStart.Dispose();
                        GC.SuppressFinalize(processToStart);
                        //GC.Collect();
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message + "\n" + "Process Count " + _processes.Count + "\n" + e.InnerException + "\n" + e.StackTrace);
            }
        }

    }
}
