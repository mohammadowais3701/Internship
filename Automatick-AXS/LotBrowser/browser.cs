﻿using AmazonInstanceHandler;
using Awesomium.Core;
using LotID_Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerAutomation.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LotBrowser
{
    public partial class browser : Form, IResourceInterceptor
    {
        String serverAddress = "mainserver.ticketpeers.com";
        String URL = String.Empty;
        String LotID = String.Empty;
        private Boolean _isRunning = false;
        private Boolean _isMouseClicked = false;
        private Boolean _isOpened = false;

        public static string RijendealPassword = String.Empty;
        public static string RijendealInitVector = String.Empty;

        public static ChannelFactory<IMainService> _factory = null;
        public static IMainService _client = null;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int X, int Y);

        public Boolean IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }
        public Boolean IsMouseClicked
        {
            get { return _isMouseClicked; }
            set { _isMouseClicked = value; }
        }
        public Boolean IsOpened
        {
            get { return _isOpened; }
            set { _isOpened = value; }
        }

        TcpListener _listener = null;
        Thread _listenerThread = null;

        int lastX = Cursor.Position.X;
        int lastY = Cursor.Position.Y;

        Boolean performClick = false;

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private ClientRequest ClientRequest = null;

        public void DoMouseClick(int x, int y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
        }

        private void createBrowser()
        {
            performClick = false;
            WebPreferences pref = new WebPreferences() { ProxyConfig = "127.0.0.1:8877" };
            this.webControl1.WebSession = WebCore.CreateWebSession(pref);
        }

        public browser()
        {
            this.performClick = false;
            InitializeComponent();
            createBrowser();

            InitializeHandler();
            this.InitializeResource();
        }

        public browser(ClientRequest req)
        {
            InitializeComponent();
            this.ClientRequest = req;
            WebPreferences pref = new WebPreferences() { ProxyConfig = "127.0.0.1:8877" };
            this.webControl1.WebSession = WebCore.CreateWebSession(pref);
            this.URL = req.URL;

            InitializeHandler();
            this.InitializeResource();
            this.webControl1.Source = new Uri(this.URL);
            this.webControl1.Refresh();
        }

        public browser(String fileName)
        {
            InitializeComponent();

            String json = string.Empty;
            if (File.Exists(fileName))
            {
                try
                {
                    using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (TextReader _reader = new StreamReader(stream))
                        {
                            json = _reader.ReadToEnd();
                        }
                    }

                    File.Delete(fileName);

                    this.ClientRequest = JsonConvert.DeserializeObject<ClientRequest>(json);
                    if (this.ClientRequest != null)
                    {
                        WebPreferences pref = new WebPreferences() { ProxyConfig = "127.0.0.1:8877" };
                        this.webControl1.WebSession = WebCore.CreateWebSession(pref);
                        this.URL = this.ClientRequest.URL;

                        InitializeHandler();
                        InitializeResource();

                        this.webControl1.Source = new Uri(this.URL);
                        this.webControl1.Refresh();

                        if (this.URL.ToLower().Contains("priority"))
                        {
                            sendMinimizeToLotGenerator();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void InitializeHandler()
        {
            try
            {
                Uri _serverAddress = new Uri("net.tcp://" + serverAddress + ":44100/ServerAutomationDatabaseService");
                this._isRunning = true;

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

                IPAddress ipAddress = IPAddress.Loopback;
                _listener = new TcpListener(ipAddress, Convert.ToInt32(Process.GetCurrentProcess().Id));
                _listener.Start();

                _listenerThread = new Thread(new ParameterizedThreadStart(work));
                _listenerThread.Start(_listener);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                performClick = false;
                this.webControl1.Source = new Uri(this.URL);
                this.webControl1.Refresh();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void InitializeResource()
        {
            try
            {
                if (WebCore.ResourceInterceptor == null)
                {
                    WebCore.ResourceInterceptor = this;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public bool OnFilterNavigation(NavigationRequest request)
        {
            return false;
        }

        public ResourceResponse OnRequest(ResourceRequest request)
        {
            try
            {
                if (request.Url.AbsoluteUri.ToLower().Contains("showshop.pricetablew") && !performClick)
                {
                    //PerformClicking();
                }
                else if (request.Url.AbsoluteUri.Contains("sessionCreateW"))
                {
                    String[] split = request.Url.Query.Split('&');

                    foreach (String item in split)
                    {
                        if (item.Contains("wrLotID"))
                        {
                            String _LotID = item.Split('=')[1];
                            Debug.WriteLine(_LotID);
                            if (!_LotID.ToLower().Contains("nolotid"))
                            {
                                LotID = _LotID;

                                this._isRunning = false;
                                break;
                            }
                        }
                    }
                }
                else if (String.IsNullOrEmpty(LotID) && request.Url.AbsoluteUri.ToLower().Contains("getphase") && !request.Url.AbsoluteUri.ToLower().Contains("nolotid"))
                {
                    String[] split = request.Url.Query.Split('&');

                    foreach (String item in split)
                    {
                        if (item.ToLower().Contains("lotid"))
                        {
                            LotID = item.Substring(item.IndexOf('=') + 1); ;

                            if (!LotID.ToLower().Contains("nolotid"))
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }

        private void work(object obj)
        {
            TcpListener serverListener = (TcpListener)obj;

            try
            {
                while (this._isRunning)
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
                Debug.WriteLine(e.Message);
            }
        }

        private void handleClient(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            try
            {
                WaitingResponse response = JsonConvert.DeserializeObject<WaitingResponse>(ReadMessage(stream));

                try
                {
                    if (response.isMouseClicked)
                    {
                        PerformClicking();
                        Thread.Sleep(5000);
                        sendMinimizeToLotGenerator();

                        return;
                    }
                    else if (response.isWaiting)
                    {
                        LotID = response.LotID;

                        LicenseRights _licRights = new LicenseRights();
                        if (!String.IsNullOrEmpty(ClientRequest.AppPrefix))
                        {
                            _licRights.AppPrefix = ClientRequest.AppPrefix;
                            _licRights.Key = "getAAXMagic";
                            _licRights.lastUpdateTime = DateTime.Now;
                            _licRights.Value = LotID;

                            byte[] encrptedMessage = encryptMessage(_licRights);

                            try
                            {
                                _client.SetLicnenseRightsFromBrowser(encrptedMessage);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("[Handle Client()] Could not connect to main service.");
                            }

                            //Commented because of doing this work on Main Service directly
                            ////Doing this for Appending Lot id for other App

                            //if (ClientRequest.AppPrefix.ToLower().Equals("aax"))
                            //    _licRights.AppPrefix = "DAX";
                            //else if (ClientRequest.AppPrefix.ToLower().Equals("dax"))
                            //    _licRights.AppPrefix = "AAX";
                            //else if (ClientRequest.AppPrefix.ToLower().Equals("aek"))
                            //    _licRights.AppPrefix = "DEK";
                            //else if (ClientRequest.AppPrefix.ToLower().Equals("dek"))
                            //    _licRights.AppPrefix = "AEK";
                            if (ClientRequest.AppPrefix.ToLower().Equals("amx"))
                                _licRights.AppPrefix = "DMX";
                            else if (ClientRequest.AppPrefix.ToLower().Equals("dmx"))
                                _licRights.AppPrefix = "AMX";

                            //byte[] encrptedMessageForOtherApp = encryptMessage(_licRights);

                            //try
                            //{
                            //    _client.SetLicnenseRightsFromBrowser(encrptedMessageForOtherApp);
                            //}
                            //catch (Exception ex)
                            //{
                            //    throw new Exception("[Handle Client()] Could not connect to main service.");
                            //}
                        }

                        this.IsRunning = false;
                        this.performClick = false;
                        //this.Close();
                    }
                    else if (response.isHashGenerated)
                    {
                        this.IsRunning = false;
                        this.performClick = false;

                        IPAddress ipAddress = IPAddress.Loopback;
                        ipAddress = getLocalIpAddress();
                        using (TcpClient localClient = new TcpClient(ipAddress.ToString(), 44500))
                        {
                            NetworkStream streamBrowser = localClient.GetStream();

                            BrowserRequest br = new BrowserRequest();
                            br.IsMinimized = true;
                            br.WaitingURL = this.URL;
                            br.ProcessID = Process.GetCurrentProcess().Id.ToString();

                            byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(br)) + "<EOF>");

                            streamBrowser.Write(buffer, 0, buffer.Length);
                            streamBrowser.Close();
                        }

                        this.Close();
                    }
                    else
                    {
                        this.TopMost = false;
                        this.IsRunning = false;
                        this.performClick = false;

                        Thread.Sleep(1000);

                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.WindowState = FormWindowState.Minimized;
                        }));

                        IPAddress ipAddress = IPAddress.Loopback;
                        ipAddress = getLocalIpAddress();
                        using (TcpClient localClient = new TcpClient(ipAddress.ToString(), 44500))
                        {
                            NetworkStream streamBrowser = localClient.GetStream();

                            BrowserRequest br = new BrowserRequest();
                            br.IsMinimized = true;
                            br.ProcessID = Process.GetCurrentProcess().Id.ToString();

                            byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(br)) + "<EOF>");

                            streamBrowser.Write(buffer, 0, buffer.Length);
                            streamBrowser.Close();
                        }

                        Thread.Sleep(1000);
                        this.Close();
                    }

                }
                catch (Exception ex)
                {
                    this.TopMost = false;

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Out.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                this.TopMost = false;

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Out.WriteLine(ex.Message);
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }

        private void sendMinimizeToLotGenerator()
        {
            try
            {
                try
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }));

                    this.IsMouseClicked = true;
                    this.TopMost = false;
                }
                catch (Exception ex)
                {
                }

                IPAddress ipAddress = IPAddress.Loopback;
                ipAddress = getLocalIpAddress();
                using (TcpClient localClient = new TcpClient(ipAddress.ToString(), 44500))
                {
                    NetworkStream streamBrowser = localClient.GetStream();

                    BrowserRequest br = new BrowserRequest();
                    br.IsMinimized = true;
                    br.WaitingURL = this.URL;
                    br.ProcessID = Process.GetCurrentProcess().Id.ToString();

                    byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(br)) + "<EOF>");

                    streamBrowser.Write(buffer, 0, buffer.Length);
                    streamBrowser.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string ReadMessage(NetworkStream stream)
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

        private void WebControl_LoadingFrameComplete(object sender, FrameEventArgs e)
        {
            try
            {
                if (e.IsMainFrame)
                {
                    if (e.Url.AbsoluteUri.ToLower().Contains("getphase") && !performClick)
                    {
                        try
                        {
                            //String str = this.webControl1.ExecuteJavascriptWithResult("document.documentElement.outerHTML");
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                if (e.Url.AbsoluteUri.Contains("enternl"))
                {

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void PerformClicking()
        {
            try
            {
                lastX = Cursor.Position.X;
                lastY = Cursor.Position.Y;

                for (int i = 0; i < 100; i++)
                {
                    SetCursorPos(this.Location.X + 670, this.Location.Y + 300 + i);
                    DoMouseClick(this.Location.X + 670, this.Location.Y + 300 + i);
                }

                try
                {
                    //SendKeys.Send("{ENTER}");
                }
                catch (Exception)
                {
                    Debug.WriteLine("SendKeys.Send");
                }
                SetCursorPos(lastX, lastY);
                performClick = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

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

        private void frmBrowser_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void showForm(ClientRequest req)
        {
            try
            {
                this.ClientRequest = req;
                this.URL = req.URL;
                performClick = false;
                this.webControl1.Source = new Uri(this.URL);
                this.webControl1.Refresh();

                //this.Invoke(new MethodInvoker(delegate
                {
                    this.Show();
                };//));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void showForm()
        {
            try
            {
                performClick = false;
                this.webControl1.Source = new Uri(this.URL);
                this.webControl1.Refresh();

                //this.Invoke(new MethodInvoker(delegate
                {
                    this.Show();
                    this.IsOpened = true;
                    this.IsRunning = true;
                };//));

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }));
                }
                else
                    this.WindowState = FormWindowState.Maximized;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        internal void hideForm()
        {
            try
            {
                this.Hide();
                Debug.WriteLine(this.LotID);
                if (this._listener != null)
                {
                    this._listener.Stop();
                    this._listener = null;
                }

                this._isRunning = false;
                this.performClick = false;
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

        private void browser_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_listener != null)
                    _listener.Stop();
            }
            catch (Exception ex)
            {

            }
        }

        private void WebControl_ShowJavascriptDialog(object sender, JavascriptDialogEventArgs e)
        {
            e.Handled = true;
            e.Cancel = false;
        }
    }
}