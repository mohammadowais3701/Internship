using AmazonInstanceHandler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerAutomation.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LotGenerator_Core
{
    public class MainLotService
    {
        private static TcpListener _listener;
        private static Thread _listenerThread;
        private static TcpListener _listenerBrowser;
        private static Thread _listenerThreadBrowser;
        private static int MainPort = 44500;

        private String _serverAutomationAddress = "mainserver.ticketpeers.com";
        private Uri _serverAddress = null;

        public static ChannelFactory<IMainService> _factory = null;
        public static IMainService _client = null;
        public static string RijendealPassword = String.Empty;
        public static string RijendealInitVector = String.Empty;

        private static ConcurrentDictionary<String, DateTime> _sendBrowsers = null;

        static ConcurrentQueue<ClientRequest> _clientRequests = null;
        private static ConcurrentDictionary<String, TcpClient> _handlerList = null;

        private System.Threading.Timer _connectionTimer = null;
        private System.Threading.Timer _pushTimer = null;
        private System.Threading.Timer _releaseTimer = null;

        static ConcurrentQueue<String> _proxies = null;
        static ConcurrentDictionary<String, DateTime> _proxiesUSED = null;

        public static ConcurrentQueue<ClientRequest> ClientRequests
        {
            get { return _clientRequests; }
            set { _clientRequests = value; }
        }

        public static ConcurrentDictionary<String, TcpClient> HandlerList
        {
            get { return MainLotService._handlerList; }
            set { MainLotService._handlerList = value; }
        }

        public static ConcurrentDictionary<String, DateTime> ProxiesUSED
        {
            get { return MainLotService._proxiesUSED; }
            set { MainLotService._proxiesUSED = value; }
        }

        public static ConcurrentQueue<String> Proxies
        {
            get { return MainLotService._proxies; }
            set { MainLotService._proxies = value; }
        }

        private int Redundant = 0;
        private static int _BrowserCount = 0;

        public static int BrowserCount
        {
            get { return _BrowserCount; }
            set { _BrowserCount = value; }
        }

        static MainLotService _Service = null;

        public static MainLotService Service
        {
            get { return MainLotService._Service; }
            set { MainLotService._Service = value; }
        }

        public static void createMainLotService()
        {
            _Service = new MainLotService();
        }

        private MainLotService()
        {
            try
            {
                _clientRequests = new ConcurrentQueue<ClientRequest>();
                _handlerList = new ConcurrentDictionary<String, TcpClient>();
                _sendBrowsers = new ConcurrentDictionary<string, DateTime>();
                _proxies = new ConcurrentQueue<string>();
                _proxiesUSED = new ConcurrentDictionary<string, DateTime>();

                _serverAddress = new Uri("net.tcp://" + _serverAutomationAddress + ":44100/ServerAutomationDatabaseService");

                readConfigFile(Application.StartupPath + "\\config.txt");
                loadProxies(Application.StartupPath + "\\proxy.txt");

                String address = LocalIPAddress();

                if (InitializeHost(address))
                {
                    //WebServiceHost host = new WebServiceHost(typeof(LotMainService), new Uri("http://" + address + ":9000/"));

                    //ServiceEndpoint ep = host.AddServiceEndpoint(typeof(ILotMainService), new WebHttpBinding(), "");

                    //host.Open();

                    IPAddress ipAddress = IPAddress.Loopback;
                    IPAddress.TryParse(address, out ipAddress);
                    _listenerBrowser = new TcpListener(ipAddress, 9000);
                    _listenerBrowser.Start();

                    _listenerThreadBrowser = new Thread(new ParameterizedThreadStart(LotMainServiceWork));
                    _listenerThreadBrowser.Start(_listenerBrowser);

                    _pushTimer = new System.Threading.Timer(PushEventsToLotGenerator, null, 5 * 1000, 5 * 1000);
                    _connectionTimer = new System.Threading.Timer(VerifyClient, null, 60 * 1000, 60 * 1000);
                    _releaseTimer = new System.Threading.Timer(ReleaseThingsTimer, null, 30 * 60 * 1000, 30 * 60 * 1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void VerifyClient(object state)
        {
            try
            {
                for (int i = _handlerList.Count - 1; i >= 0; i--)
                {
                    String key = String.Empty;
                    try
                    {
                        BrowserRequest req = new BrowserRequest() { Command = "isAlive" };
                        byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(req)) + "<EOF>");
                        key = _handlerList.ElementAt(i).Key;
                        TcpClient client = _handlerList.ElementAt(i).Value;
                        NetworkStream stream = client.GetStream();
                        stream.Write(buffer, 0, buffer.Length);
                        //Console.WriteLine(key + " isAlive");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(key + " isDead");
                        TcpClient client = null;
                        _handlerList.TryRemove(key, out client);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ReleaseThingsTimer(Object obj)
        {
            try
            {
                for (int i = _sendBrowsers.Count - 1; i >= 0; i--)
                {
                    String key = String.Empty;

                    key = _sendBrowsers.ElementAt(i).Key;
                    DateTime dt = _sendBrowsers.ElementAt(i).Value;

                    if ((DateTime.Now - dt).TotalMinutes > 50)
                    {
                        _sendBrowsers.TryRemove(key, out dt);
                    }
                }

                for (int i = _proxiesUSED.Count - 1; i >= 0; i--)
                {
                    String key = String.Empty;

                    key = _proxiesUSED.ElementAt(i).Key;
                    DateTime dt = _proxiesUSED.ElementAt(i).Value;

                    if ((DateTime.Now - dt).TotalMinutes > 50)
                    {
                        _proxiesUSED.TryRemove(key, out dt);
                        _proxies.Enqueue(key);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LotMainServiceWork(Object obj)
        {
            try
            {
                TcpListener serverListener = (TcpListener)obj;

                try
                {
                    while (true)
                    {
                        TcpClient client = serverListener.AcceptTcpClient();

                        Thread th = new Thread(new ParameterizedThreadStart(this.handleLotMainServiceClient));
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

        private void handleLotMainServiceClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            try
            {
                //stream.ReadTimeout = 30000;
                //stream.WriteTimeout = 30000;

                string message = String.Empty;

                while (true)
                {
                    try
                    {
                        message = Encryptor.Decrypt(Encryptor.ReadMessage(stream));

                        Debug.WriteLine("Received: " + message);

                        BrowserRequest request = null;
                        request = JsonConvert.DeserializeObject<BrowserRequest>(message);

                        if (request != null)
                        {
                            Boolean verify = true;

                            if (_handlerList.ContainsKey(request.ID))
                            {
                                _handlerList[request.ID] = client;
                            }
                            else
                            {
                                _handlerList.TryAdd(request.ID, client);
                            }

                            if (request.Command.Equals("getConfig"))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Out.WriteLine(DateTime.Now + " -> Received: " + message);

                                byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(BrowserCount)) + "<EOF>");
                                client.GetStream().Write(buffer, 0, buffer.Length);
                            }
                            else if (request.Command.Equals("UpdateLot"))
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Out.WriteLine(DateTime.Now + " -> Received: " + message);

                                UpdateLotID(request.LotID, request.AppPrefix);
                            }
                            else if (request.Command.Equals("releaseProxy"))
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Out.WriteLine(DateTime.Now + " -> Received: " + message);

                                if (!String.IsNullOrEmpty(request.Proxy))
                                {
                                    DateTime dt = DateTime.Now;
                                    MainLotService.ProxiesUSED.TryRemove(request.Proxy, out dt);
                                    MainLotService.Proxies.Enqueue(request.Proxy);
                                }
                            }
                            else if (request.Command.Equals("Ping"))
                            {
                                //Console.ForegroundColor = ConsoleColor.White;
                                //Console.Out.WriteLine("Ping from Workers." + request.ID);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Out.WriteLine(DateTime.Now + " -> Received: " + message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("EXCEPTION: " + ex.Message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                //Log e
                //Console.ForegroundColor = ConsoleColor.Gray;
                Debug.WriteLine(e.Message);
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }

        void readConfigFile(String fileName)
        {
            try
            {
                String fileText = String.Empty;

                using (TextReader _reader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    fileText = _reader.ReadToEnd();
                }

                if (!String.IsNullOrEmpty(fileText))
                {
                    String[] Config = fileText.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    if (Config.Length > 0)
                    {
                        foreach (var item in Config)
                        {
                            String[] config = item.Split('=');

                            if (config.Length == 2)
                            {
                                if (config[0].ToLower().Equals("redundant"))
                                {
                                    Redundant = int.Parse(config[1]);
                                }
                                else if (config[0].ToLower().Equals("browsers"))
                                {
                                    BrowserCount = int.Parse(config[1]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new Exception("No Config file found");
            }
        }

        void loadProxies(String fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string text = File.ReadAllText(fileName);

                    String[] proxies = text.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    _proxies = new ConcurrentQueue<string>(proxies);
                }
                else
                {
                    _proxies = new ConcurrentQueue<string>();
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception("No Proxy file found");
            }
        }

        private Boolean InitializeHost(String address)
        {
            Boolean Result = false;
            try
            {
                IPAddress ipAddress = IPAddress.Loopback;
                IPAddress.TryParse(address, out ipAddress);
                _listener = new TcpListener(ipAddress, MainPort);
                _listener.Start();

                //loadProxies();

                _listenerThread = new Thread(new ParameterizedThreadStart(work));
                _listenerThread.Start(_listener);

                if (_client == null || _factory == null)
                {
                    _factory = new ChannelFactory<IMainService>(new NetTcpBinding(SecurityMode.None)
                    {
                        OpenTimeout = new TimeSpan(01, 10, 01),
                        CloseTimeout = new TimeSpan(01, 10, 01),
                        SendTimeout = new TimeSpan(01, 10, 01),
                        ReceiveTimeout = TimeSpan.MaxValue
                    }, new EndpointAddress(_serverAddress));

                    _client = _factory.CreateChannel();
                    ((IClientChannel)_client).Open();
                    getSharedkey();
                }

                Result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Result;
        }

        private void work(Object obj)
        {
            TcpListener serverListener = (TcpListener)obj;

            try
            {
                while (true)
                {
                    TcpClient client = serverListener.AcceptTcpClient();

                    Thread th = new Thread(new ParameterizedThreadStart(this.handleClient));
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

        private void handleClient(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            try
            {
                stream.ReadTimeout = 30000;
                stream.WriteTimeout = 30000;

                string message = Encryptor.Decrypt(Encryptor.ReadMessage(stream));

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Out.WriteLine(DateTime.Now + " -> Received: " + message);
                Debug.WriteLine("Received: " + message);

                ClientRequest request = null;
                request = JsonConvert.DeserializeObject<ClientRequest>(message);


                if (request != null)
                {
                    Boolean verify = true;

                    Uri url = new Uri(request.URL);
                    if (url.Host.ToLower().Equals("tickets.axs.com") || url.Host.ToLower().Equals("axsmobile.eventshopper.com") || url.Host.ToLower().Equals("tickets.evenko.ca") || url.Host.ToLower().Equals("wroom.centrebell.ca"))
                    {
                        int count = 0;

                        if (_clientRequests.Count > 0)
                        {
                            count = _clientRequests.Count(pred => pred.URL.Equals(request.URL));
                        }

                        if (count == 0)
                        {
                            if (!_sendBrowsers.ContainsKey(request.URL))
                            {
                                for (int i = 0; i < this.Redundant; i++)
                                {
                                    _clientRequests.Enqueue(request);
                                }
                            }
                        }
                    }
                }

                stream.Close();
                client.Close();

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
                client.Close();
            }
        }

        private void PushEventsToLotGenerator(object state)
        {
            try
            {
                for (int x = ClientRequests.Count - 1; x >= 0; x--)
                {
                    ClientRequest request = null;

                    ClientRequests.TryDequeue(out request);

                    if (request != null)
                    {
                        Boolean done = false;
                        String proxy = String.Empty;

                        for (int i = _handlerList.Count - 1; i >= 0; i--)
                        {
                            String key = String.Empty;
                            try
                            {
                                MainLotService.Proxies.TryDequeue(out proxy);

                                //if (!String.IsNullOrEmpty(proxy))
                                {
                                    try
                                    {
                                        key = _handlerList.ElementAt(i).Key;
                                        BrowserRequest req = new BrowserRequest() { Command = "startEvent", Proxy = proxy, AppPrefix = request.AppPrefix, URL = request.URL };
                                        Console.Out.WriteLine(key + " " + JsonConvert.SerializeObject(req));
                                        byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(req)) + "<EOF>");
                                        TcpClient client = _handlerList.ElementAt(i).Value;
                                        NetworkStream handlerstream = client.GetStream();
                                        handlerstream.Write(buffer, 0, buffer.Length);

                                        done = true;
                                        if (!String.IsNullOrEmpty(proxy))
                                        {
                                            MainLotService.ProxiesUSED.TryAdd(proxy, DateTime.Now);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.Message);
                                        TcpClient handler = null;
                                        _handlerList.TryRemove(key, out handler);
                                        if (!String.IsNullOrEmpty(proxy))
                                        {
                                            MainLotService.Proxies.Enqueue(proxy);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                TcpClient handler = null;
                                _handlerList.TryRemove(key, out handler);

                                if (!String.IsNullOrEmpty(proxy))
                                {
                                    MainLotService.Proxies.Enqueue(proxy);
                                }
                            }
                        }

                        if (done)
                        {
                            _sendBrowsers.TryAdd(request.URL, DateTime.Now);
                        }
                        else
                        {
                            ClientRequests.Enqueue(request);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static String LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            if (!File.Exists(System.Windows.Forms.Application.StartupPath + "\\serverip.txt"))
            {
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            else
            {
                localIP = File.ReadAllText(System.Windows.Forms.Application.StartupPath + "\\serverip.txt");
            }
            return localIP;
        }

        public void UpdateLotID(String lotID, String appPrefix)
        {
            try
            {
                LicenseRights _licRights = new LicenseRights();
                if (!String.IsNullOrEmpty(appPrefix))
                {
                    _licRights.AppPrefix = appPrefix;
                    _licRights.Key = "getAAXMagic";
                    _licRights.lastUpdateTime = DateTime.Now;
                    _licRights.Value = lotID;

                    byte[] encrptedMessage = encryptMessage(_licRights);

                    try
                    {
                        if (_client == null || ((IClientChannel)_client).State == CommunicationState.Faulted)
                        {
                            _factory = new ChannelFactory<IMainService>(new NetTcpBinding(SecurityMode.None)
                            {
                                OpenTimeout = new TimeSpan(01, 10, 01),
                                CloseTimeout = new TimeSpan(01, 10, 01),
                                SendTimeout = new TimeSpan(01, 10, 01),
                                ReceiveTimeout = TimeSpan.MaxValue
                            }, new EndpointAddress(_serverAddress));

                            _client = _factory.CreateChannel();
                            ((IClientChannel)_client).Open();
                            getSharedkey();
                        }

                        _client.SetLicnenseRightsFromBrowser(encrptedMessage);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("[Handle Client()] Could not connect to main service.");
                    }

                    //Commented because of doing this work on Main Service directly
                    ////Doing this for Appending Lot id for other App

                    if (appPrefix.ToLower().Equals("amx"))
                        _licRights.AppPrefix = "DMX";
                    else if (appPrefix.ToLower().Equals("dmx"))
                        _licRights.AppPrefix = "AMX";
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #region ServerAutomation events

        public static void getSharedkey()
        {
            Boolean flag = false;
            JObject realMsg = null;
            try
            {
                String encryptedBase64String = String.Empty;
                byte[] encryptedBase64StringBytes = null;

                var settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Objects;

                RSACryptoServiceProvider _clientPublic = new RSACryptoServiceProvider(1024);

                ServerAutomation.Core.ReqResponseData message = new ServerAutomation.Core.ReqResponseData() { Command = "getKey" };

                _clientPublic.FromXmlString(EncryptionKeys.ServerPublicKey);

                byte[] bytes = _clientPublic.Encrypt(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)), false);

                encryptedBase64String = Convert.ToBase64String(bytes);

                encryptedBase64StringBytes = Encoding.UTF8.GetBytes(encryptedBase64String);
                byte[] result = null;

                if (((IClientChannel)_client).State != CommunicationState.Opened && ((IClientChannel)_client).State == CommunicationState.Faulted)
                {
                    ((IClientChannel)_client).Open();
                }

                result = _client.GetSharedKey(encryptedBase64StringBytes);

                string _message = Encoding.UTF8.GetString(result);
                byte[] base64EncryptedString = Convert.FromBase64String(_message);

                _clientPublic.FromXmlString(EncryptionKeys.ClientPrivateKey);

                bytes = _clientPublic.Decrypt(base64EncryptedString, false);

                realMsg = JObject.Parse(ASCIIEncoding.UTF8.GetString(bytes));
                RijendealPassword = realMsg["PassPhrase"].ToString();
                RijendealInitVector = realMsg["InitVector"].ToString();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message + Environment.NewLine + e.InnerException);
            }
        }

        public static byte[] encryptMessage(LicenseRights _licRights)
        {
            String encryptedBase64String = String.Empty;

            byte[] encryptedBase64StringBytes = null;
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;

            try
            {
                RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(RijendealPassword, RijendealInitVector);

                encryptedBase64String = Convert.ToBase64String(rijndaelKey.EncryptToBytes(JsonConvert.SerializeObject(_licRights, Newtonsoft.Json.Formatting.Indented, settings)));
                encryptedBase64StringBytes = Encoding.UTF8.GetBytes(encryptedBase64String);

                return encryptedBase64StringBytes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return encryptedBase64StringBytes;
        }

        #endregion
    }
}
