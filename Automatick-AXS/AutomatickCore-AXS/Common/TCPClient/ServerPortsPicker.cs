using CapsiumSharedMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using TCPClient;
using TraceUtils;

namespace Automatick.Core
{
    public class ServerPortsPicker
    {
        static ServerPortsPicker _serverPortsPicker = null;
        String appPreFix = "";
        String hardDiskSerial;
        String licenseCode;
        String processorID;
        String jwtToken = String.Empty;

        TCPClient.PortResponse _clientports;
        List<TokenServer> _servers;
        int currentPortIndex = 0;

        public long LoadBalancerWorkers = 10;
        public long Requested = 0;
        public long usingRefreshJWTToken = 0;

        public TCPClient.PortResponse ClientPorts
        {
            get { return _clientports; }
            set { _clientports = value; }
        }

        public List<TokenServer> Servers
        {
            get { return _servers; }
            set { _servers = value; }
        }

        public String AppPreFix
        {
            get { return appPreFix; }
            set { appPreFix = value; }
        }

        public String HardDiskSerial
        {
            get { return hardDiskSerial; }
            set { hardDiskSerial = value; }
        }

        public String LicenseCode
        {
            get { return licenseCode; }
            set { licenseCode = value; }
        }

        public String ProcessorID
        {
            get { return processorID; }
            set { processorID = value; }
        }

        public String JwtToken
        {
            get { return jwtToken; }
            set { jwtToken = value; }
        }

        public static ServerPortsPicker ServerPortsPickerInstance
        {
            get
            {
                if (_serverPortsPicker == null)
                {
                    throw new Exception("ServerPorts picker Object not created");
                }
                return _serverPortsPicker;

            }
        }

        public static void createServerPortsPickerInstance(LicenseCore msg)
        {
            _serverPortsPicker = new ServerPortsPicker(msg);
        }

        private ServerPortsPicker(LicenseCore lic)
        {
            this.licenseCode = lic.LicenseCode;
            this.HardDiskSerial = lic.HardDiskSerial;
            this.ProcessorID = lic.ProcessorID;
            this.AppPreFix = lic.AppPreFix;
            this.JwtToken = "nothing";
            //this.licenseCode = "system";//;
            //this.HardDiskSerial = "system";//
            //this.ProcessorID = "system";//
            //this.AppPreFix = "RTM";
            this.Servers = new List<TokenServer>();
           // this.initializeLicenseToken();
           // this.InitializeTimerForJWTToken(60); 
        }

        #region PortsWork

        public void GetPortsfromServer(string servers)
        {
            try
            {
                String[] serverlist = null;// servers = "192.168.8.104:8018";

                if (!String.IsNullOrEmpty(servers))
                {
                    serverlist = servers.Split(',');

                    if (this.Servers.Count > 0)
                    {
                        for (int i = this.Servers.Count - 1; i >= 0; i--)
                        {
                            if (!serverlist.Contains(this.Servers[i].ServerIP + ":" + this.Servers[i].ServerPort))
                            {
                                this.Servers[i].DisposeTimer();
                                this.Servers.RemoveAt(i);
                            }
                        }
                    }

                    foreach (String item in serverlist)
                    {
                        if (this.Servers != null)
                        {
                            try
                            {
                                if (this.Servers.FirstOrDefault(pred => pred.ServerIP == item.Split(':')[0] && pred.ServerPort == Convert.ToInt32(item.Split(':')[1])) == null)
                                {
                                    this.Servers.Add(new TokenServer(item.Split(':')[0], Convert.ToInt32(item.Split(':')[1])));
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                try
                {
                    if (this.Servers != null && this.Servers.Count > 0)
                    {
                       // this.Servers.Shuffle();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                if (this.Servers != null)
                {
                    if (this.Servers.Count > 0)
                    {
                        foreach (TokenServer selectedServer in this.Servers)
                        {
                            selectedServer.InitializeTimer(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public void GetPorts(TokenServer server)
        {
            //try
            //{
            //    using (TcpClient client = new TcpClient(server.ServerIP, server.ServerPort))
            //    {
            //        NetworkStream stream = client.GetStream();

            //        TCPClient.PortResponse msg = new TCPClient.PortResponse();
            //        msg.command = "getPorts";
            //        msg.HardDiskSerial = this.HardDiskSerial;
            //        msg.LicenseCode = this.LicenseCode;
            //        msg.ProcessorID = this.ProcessorID;
            //        msg.AppPrefix = this.AppPreFix;

            //        byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

            //        stream.Write(buffer, 0, buffer.Length);
            //        string message = ReadMessage(stream);

            //        message = TCPEncryptor.Decrypt(message);

            //        if (server.PortList != null)
            //        {
            //            server.PortList.Ports.Clear();
            //        }

            //        server.PortList = JsonConvert.DeserializeObject<TCPClient.PortResponse>(message);

            //        if (server.PortList.LeaseTime > 0)
            //        {
            //            server.LeaseTime = server.PortList.LeaseTime;
            //        }
            //        else
            //        {
            //            server.LeaseTime = 15;
            //        }

            //        try
            //        {
            //            if (server.PortList.Ports.Count > 0)
            //            {
            //                server.PortList.Ports.Shuffle();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Debug.WriteLine(ex.Message);
            //        }

            //        Debug.WriteLine("-------------------------------------------------------------------");
            //        Debug.WriteLine(server.ServerIP + ":Ports Count:" + server.PortList.Ports.Count);

            //        List<String> dummy = new List<string>();
            //        foreach (String proxy in server.PortList.Ports)
            //        {
            //            dummy.Add(server.ServerIP + ":" + proxy.Split(':')[1]);
            //        }

            //        server.PortList.Ports.Clear();
            //        server.PortList.Ports = dummy;
            //    }
            //}
            //catch
            //{
            //    server.LeaseTime = 15;
            //}
        }

        //public IPPort getNextPort(Boolean recap)
        //{
        //    try
        //    {
        //        TokenServer selectedServer = null;
        //        IPPort selectedPort = null;

        //        if ((this.Servers != null) && (this.Servers.Count > 0))
        //        {
        //            //lock (this.Servers)
        //            {

        //                if (this.currentPortIndex >= this.Servers.Count)
        //                {
        //                    this.currentPortIndex = 0;
        //                }

        //            GetNextServer:

        //                selectedServer = this.Servers[this.currentPortIndex];

        //                if (selectedServer.PortList.Ports != null)
        //                {
        //                    if (selectedServer.PortList.Ports.Count == 0)
        //                    {
        //                        // this.currentPortIndex++;
        //                        Interlocked.Increment(ref this.currentPortIndex);
        //                        if (this.currentPortIndex >= this.Servers.Count)
        //                        {
        //                            this.currentPortIndex = 0;
        //                        }
        //                        goto GetNextServer;
        //                    }

        //                    if (selectedServer.PortList.Ports.Count > 0)
        //                    {
        //                        if (selectedServer.ServerCounter >= selectedServer.PortList.Ports.Count)
        //                        {
        //                            selectedServer.ServerCounter = 0;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Interlocked.Increment(ref this.currentPortIndex);
        //                    if (this.currentPortIndex >= this.Servers.Count)
        //                    {
        //                        this.currentPortIndex = 0;
        //                    }
        //                    goto GetNextServer;

        //                    //if (this.Servers.Count >= (this.currentPortIndex - 1))
        //                    //{
        //                    //    selectedServer = this.Servers[this.currentPortIndex - 1];

        //                    //    if (selectedServer.PortList.Ports != null)
        //                    //    {
        //                    //        if (selectedServer.ServerCounter >= selectedServer.PortList.Ports.Count)
        //                    //        {
        //                    //            selectedServer.ServerCounter = 0;
        //                    //        }
        //                    //    }
        //                    //}

        //                }

        //                selectedPort = new IPPort();
        //                selectedPort.IP = selectedServer.PortList.Ports[selectedServer.ServerCounter].Split(':')[0];
        //                selectedPort.Port = Convert.ToInt32(selectedServer.PortList.Ports[selectedServer.ServerCounter].Split(':')[1]);
        //                Debug.WriteLine("selected port:" + selectedPort.IP + ":" + selectedPort.Port);
        //                selectedServer.ServerCounter++;
        //                Interlocked.Increment(ref this.currentPortIndex);//this.currentPortIndex++
        //            }
        //        }

        //        return selectedPort;

        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public IPPort getNextPort()
        {
            try
            {
                IPPort selectedPort = null;

                using (TcpClient _clientForLB = new TcpClient("mainserver.ticketpeers.com", 111))
                {
                    try
                    {
                        NetworkStream stream = _clientForLB.GetStream();

                        LoadBalancer.LBRequestMessage msg = new LoadBalancer.LBRequestMessage();

                        msg.Command = "getNextSolvemediaAddress";

                        byte[] buffer = Encoding.UTF8.GetBytes(LoadBalancer.TCPEncryptor.LBEncrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

                        stream.Write(buffer, 0, buffer.Length);

                        string message = ReadMessage(stream);

                        String response = LoadBalancer.TCPEncryptor.LBDecrypt(message);

                        if (!String.IsNullOrEmpty(response) && (!response.Equals("empty")))
                        {
                            LoadBalancer.LBResponseMessage lbReponse = JsonConvert.DeserializeObject<LoadBalancer.LBResponseMessage>(response);

                            if (lbReponse != null)
                            {
                                if (!String.IsNullOrEmpty(lbReponse.Server))
                                {
                                    Debug.WriteLine(lbReponse.Server);
                                    selectedPort = new IPPort();
                                    selectedPort.IP = lbReponse.Server.Split(':')[0];
                                    selectedPort.Port = Convert.ToInt32(lbReponse.Server.Split(':')[1]);
                                    stream.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                return selectedPort;
            }
            catch
            {
                return null;
            }
        }

        public IPPort getNextPort(Boolean ifRecap)
        {
            try
            {
                IPPort selectedPort = null;


                using (TcpClient _clientForLB = new TcpClient("mainserver.ticketpeers.com", 111))
                {
                    try
                    {
                        NetworkStream stream = _clientForLB.GetStream();

                        LoadBalancer.LBRequestMessage msg = new LoadBalancer.LBRequestMessage();

                        msg.Command = "getNextAAXRecaptchaV2Address";

                        byte[] buffer = Encoding.UTF8.GetBytes(LoadBalancer.TCPEncryptor.LBEncrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

                        stream.Write(buffer, 0, buffer.Length);

                        string message = ReadMessage(stream);

                        String response = LoadBalancer.TCPEncryptor.LBDecrypt(message);

                        if (!String.IsNullOrEmpty(response) && (!response.Equals("empty")))
                        {
                            LoadBalancer.LBResponseMessage lbReponse = JsonConvert.DeserializeObject<LoadBalancer.LBResponseMessage>(response);

                            if (lbReponse != null)
                            {
                                if (!String.IsNullOrEmpty(lbReponse.Server))
                                {
                                    Debug.WriteLine(lbReponse.Server);
                                    selectedPort = new IPPort();
                                    selectedPort.IP = lbReponse.Server.Split(':')[0];
                                    selectedPort.Port = Convert.ToInt32(lbReponse.Server.Split(':')[1]);
                                    stream.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }


                return selectedPort;
            }
            catch
            {
                return null;
            }
        }

        private void _clientForLBRead()
        {
            try
            {
                while (_clientForLB.Connected)
                {
                    try
                    {
                        string message = ReadMessage(_clientForLB.GetStream());

                        message = LoadBalancer.TCPEncryptor.LBDecrypt(message);

                        if (!String.IsNullOrEmpty(message) && message.Contains("pong") && !message.Equals("Zinda ho kay mar gayay") && message.Contains("8008"))
                        {

                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                        result = false;
                    }
                }

                if (_clientForLB != null && (_clientForLB.Connected))
                {
                    _clientForLB = new TcpClient("mainserver.ticketpeers.com", 111);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        static String ReadMessage(NetworkStream stream)
        {
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

        int countImages = 0;
        private static TcpClient _client = null;
        private static TcpClient _clientForLB = null;
        private bool result = false, isthrottleConnected = false;

        private static TcpClient _throttleclient = null;


        private bool _result = false, isAC1Connected = false;

        private static TcpClient _AC1client = null;

        private String _aC1Credential = String.Empty;

        public String AC1Credential
        {
            get { return _aC1Credential; }
            set { _aC1Credential = value; }
        }

        public Capsium getCaptcha(String key, String referer)
        {
            DateTime dt = DateTime.Now;
            IPPort selectedPort = null;
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                selectedPort = getNextPort();

                if (selectedPort != null && (!String.IsNullOrEmpty(selectedPort.IP)))
                {
                    using (client = new TcpClient(selectedPort.IP, selectedPort.Port))
                    {
                        try { Debug.WriteLine("Request sent->" + selectedPort.IP + ":" + selectedPort.Port); }
                        catch { }


                        stream = client.GetStream();
                        //stream.ReadTimeout = 60 * 1000;
                        //stream.WriteTimeout = 60 * 1000;

                        TCPClient.TokenMessage msg = new TCPClient.TokenMessage();
                        msg.Command = "getCaptcha";
                        msg.HardDiskSerial = this.HardDiskSerial;
                        msg.LicenseCode = this.LicenseCode;
                        msg.ProcessorID = this.ProcessorID;
                        msg.AppPrefix = this.AppPreFix;
                        msg.Type = key;
                        msg.Referer = referer;

                        byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

                        stream.Write(buffer, 0, buffer.Length);

                        string message = ReadMessage(stream);
                        message = TCPEncryptor.Decrypt(message);
                        Capsium Tokens = JsonConvert.DeserializeObject<Capsium>(message);
                        try { Debug.WriteLine(" --> Request received->" + selectedPort.IP + ":" + selectedPort.Port + "-> Total time taken ->" + (DateTime.Now - dt).TotalSeconds.ToString() + " ->" + Tokens.CValue); }
                        catch (Exception ex) { Debug.WriteLine(ex.Message); }
                        return Tokens;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                try
                {
                    Debug.WriteLine("TCP Client was open");
                    stream.Close();
                    client.Close();

                }
                catch (Exception)
                {
                    Debug.WriteLine("No TCP Client");
                }
                Debug.WriteLine("Request received->" + selectedPort.IP + ":" + selectedPort.Port + "-> Total time taken ->" + (DateTime.Now - dt).TotalSeconds.ToString() + " ->" + e.Message);
                return null;
            }
        }

        public Capsium getCaptcha(String key)
        {
            DateTime dt = DateTime.Now;
            IPPort selectedPort = null;
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                selectedPort = getNextPort(true);
                if (selectedPort != null && (!String.IsNullOrEmpty(selectedPort.IP)))
                {
                    using (client = new TcpClient(selectedPort.IP, selectedPort.Port))
                    {
                        try { Debug.WriteLine("Request sent->" + selectedPort.IP + ":" + selectedPort.Port); }
                        catch { }


                        stream = client.GetStream();
                        //stream.ReadTimeout = 60 * 1000;
                        //stream.WriteTimeout = 60 * 1000;

                        TCPClient.TokenMessage msg = new TCPClient.TokenMessage();
                        msg.Command = "getCaptcha";
                        msg.HardDiskSerial = this.HardDiskSerial;
                        msg.LicenseCode = this.LicenseCode;
                        msg.ProcessorID = this.ProcessorID;
                        msg.AppPrefix = this.AppPreFix;
                        msg.Type = key;

                        byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

                        stream.Write(buffer, 0, buffer.Length);

                        string message = ReadMessage(stream);
                        message = TCPEncryptor.Decrypt(message);
                        Capsium Tokens = JsonConvert.DeserializeObject<Capsium>(message);
                        try { Debug.WriteLine(" --> Request received->" + selectedPort.IP + ":" + selectedPort.Port + "-> Total time taken ->" + (DateTime.Now - dt).TotalSeconds.ToString() + " ->" + Tokens.CValue); }
                        catch (Exception ex) { Debug.WriteLine(ex.Message); }
                        return Tokens;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                try
                {
                    Debug.WriteLine("TCP Client was open");
                    stream.Close();
                    client.Close();
                }
                catch (Exception)
                {
                    Debug.WriteLine("No TCP Client");
                }
                Debug.WriteLine("Request received->" + selectedPort.IP + ":" + selectedPort.Port + "-> Total time taken ->" + (DateTime.Now - dt).TotalSeconds.ToString() + " ->" + e.Message);
                return null;
            }
        }

        #region For AC1 
        //public void getCaptcha(String key, CapsiumSharedMessages.JWTTokenMessage msg)
        //{

        //    try
        //    {
        //        if (msg.Audio)
        //        {
        //            StarClient.StarClient.requestCaptchaV2(new CaptchaV2Request()
        //            {
        //                JWTToken = jwtToken,
        //                Audio = true,
        //                LicenseCode = LicensingToken.Base64Decode(this.LicenseCode),
        //                CaptchaSiteKey = key,
        //                CaptchaWebSite = msg.Site,
        //                CaptchaService = msg.ServiceName,
        //                CaptchaServiceKey = msg.Key,
        //            });
        //        }
        //        else
        //        {
        //            StarClient.StarClient.requestCaptchaV2(new CaptchaV2Request()
        //            {
        //                JWTToken = jwtToken,
        //                Audio = false,
        //                LicenseCode = LicensingToken.Base64Decode(this.LicenseCode),
        //                CaptchaSiteKey = key,
        //                CaptchaWebSite = msg.Site,
        //                CaptchaService = msg.ServiceName,
        //                CaptchaServiceKey = msg.Key,
        //                CaptchaServiceIPAddress = msg.Ip,
        //                CaptchaServiceHost = msg.Host,
        //                CaptchaServicePassword = msg.Password,
        //                CaptchaServiceUsername = msg.Username,
        //                CaptchaServicePort = msg.Port

        //            });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        try
        //        {
        //            Debug.WriteLine("TCP Client was open:" + e.Message);

        //        }
        //        catch (Exception)
        //        {
        //            Debug.WriteLine("No TCP Client");
        //        }
        //    }
        //} 
        #endregion

     
        #endregion

        public void getCapsiumServer()
        {
            try
            {
                _client = new TcpClient("mainserver.ticketpeers.com", 44000);

                NetworkStream stream = _client.GetStream();

                ServerRequestMessage msg = new ServerRequestMessage();

                msg.Command = "getCapsiumServers";

                msg.AppPrefix = this.AppPreFix;

                msg.LicenseCode = this.LicenseCode;

                msg.ProcessorID = this.ProcessorID;

                msg.HardDiskSerial = this.HardDiskSerial;

                byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

                stream.Write(buffer, 0, buffer.Length);

                string message = ReadMessage(stream);

                this.GetPortsfromServer(TCPEncryptor.Decrypt(message));

                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            Task.Run(() => takeLatestServers());
        }

        private void takeLatestServers()
        {
            try
            {
                while (result)
                {
                    try
                    {
                        string message = ReadMessage(_client.GetStream());

                        message = TCPEncryptor.Decrypt(message);

                        if (!String.IsNullOrEmpty(message) && !message.Equals("Zinda ho kay mar gayay") && message.Contains("8008"))
                        {
                            this.GetPortsfromServer(message);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                        result = false;
                    }
                }
                if (!result)
                {
                    getCapsiumServer();
                }

            }
            catch (Exception e)
            {
                _client.Close();
                Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);

            }
        }

        public void stop()
        {
            try
            {
                _client.Close();

                if (_clientForLB != null)
                {
                    _clientForLB.Close();
                }

                if (_throttleclient != null)
                {
                    _throttleclient.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);

            }
        }

        #region Throttle Licensing

        public void getThrottleValue()
        {
            try
            {
                _throttleclient = new TcpClient("mainserver.ticketpeers.com", 44000);

                NetworkStream stream = _throttleclient.GetStream();

                ServerRequestMessage msg = new ServerRequestMessage();

                msg.Command = "getThrottleValue";

                msg.AppPrefix = this.AppPreFix;

                msg.LicenseCode = this.LicenseCode;

                msg.ProcessorID = this.ProcessorID;

                msg.HardDiskSerial = this.HardDiskSerial;

                byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

                stream.Write(buffer, 0, buffer.Length);

                string message = ReadMessage(stream);
                String value = TCPEncryptor.Decrypt(message);

                if (!String.IsNullOrEmpty(value))
                {
                    LoadBalancerWorkers = Convert.ToInt32(value);
                }

                isthrottleConnected = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            Task.Run(() => takeLatestThrottleValue());
        }

        private void takeLatestThrottleValue()
        {
            try
            {
                while (isthrottleConnected)
                {
                    try
                    {
                        string message = ReadMessage(_throttleclient.GetStream());

                        message = TCPEncryptor.Decrypt(message);

                        if (!String.IsNullOrEmpty(message) && !message.Equals("Zinda ho kay mar gayay"))
                        {
                            LoadBalancerWorkers = Convert.ToInt32(message);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                        isthrottleConnected = false;
                    }
                }
                if (!isthrottleConnected)
                {
                    getThrottleValue();
                }
            }
            catch (Exception e)
            {
                _throttleclient.Close();
                Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);

            }
        }

        #endregion

        #region AC1 value from Licensing

        public void getAC1Value()
        {
            try
            {
                _AC1client = new TcpClient("mainserver.ticketpeers.com", 44000);

                NetworkStream stream = _AC1client.GetStream();

                ServerRequestMessage msg = new ServerRequestMessage();

                msg.Command = "C1";

                msg.AppPrefix = this.AppPreFix;

                msg.LicenseCode = this.LicenseCode;

                msg.ProcessorID = this.ProcessorID;

                msg.HardDiskSerial = this.HardDiskSerial;

                byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");

                stream.Write(buffer, 0, buffer.Length);

                string message = ReadMessage(stream);

                AC1Credential = TCPEncryptor.Decrypt(message);

                isAC1Connected = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            Task.Run(() => takeLatestAC1Value());
        }

        private void takeLatestAC1Value()
        {
            try
            {
                while (isAC1Connected)
                {
                    try
                    {
                        string message = ReadMessage(_AC1client.GetStream());

                        message = TCPEncryptor.Decrypt(message);

                        if (!String.IsNullOrEmpty(message) && !message.Equals("Zinda ho kay mar gayay"))
                        {
                            AC1Credential = message;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                        isAC1Connected = false;
                    }
                }
                if (!isAC1Connected)
                {
                    getAC1Value();
                }

            }
            catch (Exception e)
            {
                _AC1client.Close();
                Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);

            }
        }

        #endregion

        #region JWTToken methods

        public void initializeLicenseToken()
        {
            try
            {
                String url = "http://license.ticketpeers.com:1235/users/token/generate";
                String post = "LicenseID=" + System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode))) + "&ProccessorID=" + System.Web.HttpUtility.UrlEncode(processorID) + "&HarddiskSerial=" + System.Web.HttpUtility.UrlEncode(hardDiskSerial);

                HttpWebRequest req = HttpWebRequest.CreateHttp(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = Encoding.ASCII.GetBytes(post).Length;
                req.GetRequestStream().Write(Encoding.ASCII.GetBytes(post), 0, Encoding.ASCII.GetBytes(post).Length);
                String result = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();

                if (!String.IsNullOrEmpty(result))
                {
                    this.jwtToken = result;
                    LicenseInvalid = true;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void InitializeTimerForJWTToken(int time)
        {
            this.jwtTokenTimer = time;
            TimeSpan ts = new TimeSpan(0, 0, this.jwtTokenTimer);
            this._refreshJWTTokenCallback = new System.Threading.Timer(new TimerCallback(RefreshJWTTokenCallback), null, ts, ts);
        }

        public void DisposeTimerForJWTToken()
        {
            this._refreshJWTTokenCallback.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void RefreshJWTTokenCallback(object obj)
        {
            try
            {
                if (!LicenseInvalid)
                {
                    if (0 == Interlocked.Exchange(ref usingRefreshJWTToken, 1))
                    {
                        initializeLicenseToken();
                    } 
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    //Release the lock
                    Interlocked.Exchange(ref usingRefreshJWTToken, 0);
                }
                catch (Exception)
                {

                }
            }
        }

        public Timer _refreshJWTTokenCallback = null;
        private int jwtTokenTimer;

        private bool licenseInvalid = false;

        public bool LicenseInvalid
        {
            get { return licenseInvalid; }
            set { licenseInvalid = value; }
        }

        #endregion
    }

    public class TokenServer
    {
        #region members
        int _servercounter;
        string serverIP;
        int serverPort;
        TCPClient.PortResponse ports;
        int leaseTime;
        private System.Threading.Timer _refreshPorts = null;

        public TCPClient.PortResponse PortList
        {
            get { return ports; }
            set { ports = value; }
        }

        public int ServerCounter
        {
            get { return _servercounter; }
            set { _servercounter = value; }
        }


        public string ServerIP
        {
            get { return serverIP; }
            set { serverIP = value; }
        }


        public int ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }

        public int LeaseTime
        {
            get { return leaseTime; }
            set { leaseTime = value; }
        }

        #endregion

        #region
        public TokenServer(string _serverIP, int _serverport)
        {
            this.ServerCounter = 0;
            this.ServerIP = _serverIP;
            this.serverPort = _serverport;
            this.PortList = new TCPClient.PortResponse();
        }
        #endregion

        #region methods

        public void InitializeTimer(int leasetime)
        {
            this._refreshPorts = null;
            this.LeaseTime = leasetime;
            TimeSpan ts = new TimeSpan(0, this.LeaseTime, 0);
            this._refreshPorts = new System.Threading.Timer(new TimerCallback(RefreshPortsCallback), null, ts, new TimeSpan(-1));
        }

        public void DisposeTimer()
        {
            this._refreshPorts.Change(Timeout.Infinite, Timeout.Infinite);
            this.PortList.Ports.Clear();
        }

        private void RefreshPortsCallback(object obj)
        {
            try
            {
                TimeSpan ts;
                this._refreshPorts = null;
               // ServerPortsPicker.ServerPortsPickerInstance.GetPorts(this);

                if (this.leaseTime > -1)
                {
                    ts = new TimeSpan(0, this.LeaseTime, 0);
                }
                else
                {
                    //TODO: Connection error,retry after 15 seconds
                    this.LeaseTime = 15;
                    ts = new TimeSpan(0, 0, this.LeaseTime);
                }

                this._refreshPorts = new System.Threading.Timer(new TimerCallback(RefreshPortsCallback), null, ts, new TimeSpan(-1));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        #endregion
    }

    public class IPPort
    {
        string ip;
        int port;

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
    }

    #region Capsium

    public class Capsium
    {
        public Byte[] Image;

        public bool FallBack
        {
            get;
            set;
        }

        public String CValue
        {
            get;
            set;
        }

        public DateTime ExpireTime
        {
            get;
            set;
        }

        public string ErrorMsg
        {
            get;
            set;
        }

        public bool Isexpired
        {
            get;
            set;
        }

        public String Referer { get; set; }

        public String RecapToken { get; set; }

        public String UserAgent { get; set; }

        public String RecaptchaJS { get; set; }

        public String PostBody
        {
            get;
            set;
        }
    }
    #endregion

    #region Fisher-Yates shuffle [reason: to distribute multiple clients requests]

    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

    //static class MyExtensions
    //{
    //    public static void Shuffle<T>(this IList<T> list)
    //    {
    //        int n = list.Count;
    //        while (n > 1)
    //        {
    //            n--;
    //            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
    //            T value = list[k];
    //            list[k] = list[n];
    //            list[n] = value;
    //        }
    //    }
    //}
    #endregion

    #region for AC1
    //public class starCapsium : IClientCommandProcessor
    //{
    //    public static int requests = 0;
    //    public static int delivered = 0;

    //    public void process(string message)
    //    {
    //        try
    //        {
    //            Console.WriteLine(message);

    //            String messageType = JObject.Parse(message)["messageType"].ToString();

    //            if (messageType.Equals("JWTAuthChallengeRequest"))
    //            {
    //                try
    //                {
    //                    JWTAuthChallengeResponse response = new JWTAuthChallengeResponse();

    //                    //Create JwtToken and send


    //                    response.JwtToken = ServerPortsPicker.ServerPortsPickerInstance.JwtToken;//LicensingToken.GetLicenseToken(ClientLicenseConfig.LicenseCode, ClientLicenseConfig.ProcessorID, ClientLicenseConfig.HardDiskSerial);
    //                    response.LicenseCode = Encoding.ASCII.GetString(Convert.FromBase64String(ClientLicenseConfig.LicenseCode));

    //                    String resMsg = JsonConvert.SerializeObject(response);

    //                    StarClient.StarClient.WriteMessage(resMsg);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.WriteLine(e.Message);
    //                    Tracer.TraceMessage(e.Message);
    //                }
    //            }

    //            else if (messageType.Equals("AuthConfirmation"))
    //            {
    //                try
    //                {
    //                    //You are now authenticated
    //                    AuthConfirmation authReply = JsonConvert.DeserializeObject<AuthConfirmation>(message);
    //                    StarClient.StarClient.onAuthConfirmationMessage(authReply);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.WriteLine(e.Message);
    //                    Tracer.TraceMessage(e.Message);
    //                }
    //            }
    //            else if (messageType.Equals("CaptchaV2Response"))
    //            {
    //                try
    //                {
    //                    //Captcha Recieved, do something
    //                    CaptchaV2Response captchaMsg = JsonConvert.DeserializeObject<CaptchaV2Response>(message);
    //                    System.Threading.Interlocked.Increment(ref delivered);

    //                    Capsium capsium = new Capsium();
    //                    capsium.RecapToken = captchaMsg != null ? captchaMsg.Captcha : String.Empty;
    //                    capsium.ErrorMsg = captchaMsg.error;
    //                    capsium.FallBack = true;
    //                    ServerPortsPicker.ServerPortsPickerInstance.web.Add(capsium);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.WriteLine(e.Message);
    //                    Tracer.TraceMessage(e.Message);
    //                }
    //            }
    //            else if (messageType.Equals("CaptchaV1Response"))
    //            {
    //                try
    //                {
    //                    //Captcha Recieved, do something
    //                    CaptchaV1Response captchaMsg = JsonConvert.DeserializeObject<CaptchaV1Response>(message);
    //                    System.Threading.Interlocked.Increment(ref delivered);
    //                    //Console.Title = String.Format("{0} {1}", requests, delivered);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.WriteLine(e.Message);
    //                    Tracer.TraceMessage(e.Message);
    //                }
    //            }
    //            else if (messageType.Equals("WaitMessage"))
    //            {
    //                try
    //                {
    //                    //RateLimit exceeded.
    //                    WaitMessage waitMsg = JsonConvert.DeserializeObject<WaitMessage>(message);
    //                    StarClient.StarClient.onWaitMessage(waitMsg);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.WriteLine(e.Message);
    //                    Tracer.TraceMessage(e.Message);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine(ex.Message);
    //            Tracer.TraceMessage(ex.Message);
    //        }
    //    }

    //} 
    #endregion
}
