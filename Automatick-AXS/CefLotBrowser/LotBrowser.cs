using AmazonInstanceHandler;
using CefSharp;
using CefSharp.WinForms;
using LotGenerator_Core;
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
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace LotBrowser
{
    public partial class LotBrowser : Form, IJsDialogHandler, IRequestHandler
    {
        ChromiumWebBrowser browser = null;

        String userName = String.Empty;
        String password = String.Empty;
        String eventID = String.Empty;
        String WR = String.Empty;
        String AppPrefix = String.Empty;
        String URL = String.Empty;

        private static int MainPort = 44500;

        private String _serverAutomationAddress = "mainserver.ticketpeers.com";
        private Uri _serverAddress = null;

        public static ChannelFactory<IMainService> _factory = null;
        public static IMainService _client = null;
        public static string RijendealPassword = String.Empty;
        public static string RijendealInitVector = String.Empty;

        Boolean _Close = false;
        private Boolean _isLotUpdated = false;

        public LotBrowser()
        {
            InitializeComponent();

            CefSettings settings = new CefSettings();
            settings.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Version/7.0 Mobile/11D257 Safari/9537.53";// "NokiaN97/21.1.107 (SymbianOS/9.4; Series60/5.0 Mozilla/5.0; Profile/MIDP-2.1 Configuration/CLDC-1.1) AppleWebkit/525 (KHTML, like Gecko) BrowserNG/7.1.4";
            //settings.CefCommandLineArgs.Add("proxy-server", "127.0.0.1:8888");
            //settings.CefCommandLineArgs.Add("proxy-server", "216.45.55.52:80");
            settings.CefCommandLineArgs.Add("proxy-server", "64.71.78.210:9970");
            //settings.CachePath = "";

            Cef.Initialize(settings, true, false);
            this.URL = "https://tickets.axs.com/shop/#/71eacb26-ec2c-4baa-8947-349e5212c44d/waiting-room/pick-tickets?skin=thefonda&lang=en&locale=en_us&preFill=1&eventid=313681&ec=GFT161217&src=AEGAXS1_WMAIN&fbShareURL=www.axs.com%2Fevents%2F313681%2Fan-evening-with-chris-robinson-brotherhood-tickets%3F%26ref%3Devs_fb";
            //browser = new ChromiumWebBrowser("http://icanhazip.com/")
            browser = new ChromiumWebBrowser("https://tickets.axs.com/shop/#/71eacb26-ec2c-4baa-8947-349e5212c44d/waiting-room/pick-tickets?skin=thefonda&lang=en&locale=en_us&preFill=1&eventid=313681&ec=GFT161217&src=AEGAXS1_WMAIN&fbShareURL=www.axs.com%2Fevents%2F313681%2Fan-evening-with-chris-robinson-brotherhood-tickets%3F%26ref%3Devs_fb")
            {
                Dock = DockStyle.Fill,
                AutoScrollOffset = new Point(0, 0),
                BrowserSettings = new BrowserSettings()
                {
                    ApplicationCacheDisabled = true
                }
            };

            (browser as IWebBrowser).JsDialogHandler = this;
            (browser as IWebBrowser).RequestHandler = this;
            (browser as IWebBrowser).FrameLoadEnd += LotBrowser_FrameLoadEnd;

            this.VerticalScroll.Value = 0;
            this.Controls.Add(browser);
        }

        public LotBrowser(String url, String appPrefix)
        {
            try
            {
                InitializeComponent();
                this.URL = url;
                this.AppPrefix = appPrefix;
                _serverAddress = new Uri("net.tcp://" + _serverAutomationAddress + ":44100/ServerAutomationDatabaseService");
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

                CefSettings settings = new CefSettings();
                settings.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Version/7.0 Mobile/11D257 Safari/9537.53";// "NokiaN97/21.1.107 (SymbianOS/9.4; Series60/5.0 Mozilla/5.0; Profile/MIDP-2.1 Configuration/CLDC-1.1) AppleWebkit/525 (KHTML, like Gecko) BrowserNG/7.1.4";

                Cef.Initialize(settings, true, false);

                //browser = new ChromiumWebBrowser("http://whatismyipaddress.com/")
                browser = new ChromiumWebBrowser(this.URL)
                {
                    Dock = DockStyle.Fill,
                    AutoScrollOffset = new Point(0, 0),
                    BrowserSettings = new BrowserSettings()
                    {
                        ApplicationCacheDisabled = true,

                    }
                };

                (browser as IWebBrowser).JsDialogHandler = this;
                (browser as IWebBrowser).RequestHandler = this;
                (browser as IWebBrowser).FrameLoadEnd += LotBrowser_FrameLoadEnd;

                String _url = this.URL;
                _url = _url.Replace(";;;slash;;;", "/");
                _url = _url.Replace("%2F", "/");
                String[] query = _url.Split('&');

                foreach (var item in query)
                {
                    if (item.Split('=')[0].ToLower().Equals("eventid"))
                    {
                        this.eventID = item.Split('=')[1];
                        break;
                    }
                    else if (item.Split('=')[0].ToLower().Equals("event"))
                    {
                        this.eventID = item.Split('=')[1];
                        break;
                    }
                }

                this.WR = HttpUtility.ParseQueryString((new Uri(this.URL)).Query).Get("wr");

                if (string.IsNullOrEmpty(this.WR))
                {
                    string[] breakforWroom = this.URL.Split('?');
                    string[] split = breakforWroom[1].Split('&');

                    if (this.URL.Contains("/shop/#/"))
                    {
                        String w = this.URL.Substring(this.URL.IndexOf("#") + 2);
                        split = w.Split('/');
                        if (split[0].Contains("?"))
                        {
                            split = split[0].Split('?');
                            this.WR = split[0];
                        }
                        else
                        {
                            this.WR = split[0];
                        }
                    }

                    if (String.IsNullOrEmpty(this.WR))
                    {
                        foreach (var item in split)
                        {
                            if (item.Contains("wr="))
                            {
                                this.WR = item.Replace("wr=", "");
                                break;
                            }
                        }
                    }
                }

                this.VerticalScroll.Value = 0;
                this.Controls.Add(browser);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void LotBrowser_LoadError(object sender, LoadErrorEventArgs e)
        {
            try
            {
                //browser.Load(e.FailedUrl);
            }
            catch (Exception ex)
            {

            }
        }

        public LotBrowser(String url, String appPrefix, String proxy)
        {
            try
            {
                InitializeComponent();
                this.URL = url;
                this.AppPrefix = appPrefix;
                _serverAddress = new Uri("net.tcp://" + _serverAutomationAddress + ":44100/ServerAutomationDatabaseService");
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

                String[] _proxy = proxy.Split(':');
                String browserProxy = String.Empty;

                if (_proxy.Length == 2)
                {
                    browserProxy = _proxy[0] + ":" + _proxy[1];
                }
                else if (_proxy.Length == 4)
                {
                    browserProxy = _proxy[0] + ":" + _proxy[1];
                    userName = _proxy[2];
                    password = _proxy[3];
                }

                CefSettings settings = new CefSettings();
                settings.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Version/7.0 Mobile/11D257 Safari/9537.53";// "NokiaN97/21.1.107 (SymbianOS/9.4; Series60/5.0 Mozilla/5.0; Profile/MIDP-2.1 Configuration/CLDC-1.1) AppleWebkit/525 (KHTML, like Gecko) BrowserNG/7.1.4";
                settings.CefCommandLineArgs.Add("proxy-server", browserProxy);

                Cef.Initialize(settings, true, false);

                //browser = new ChromiumWebBrowser("http://whatismyipaddress.com/")
                browser = new ChromiumWebBrowser(this.URL)
                {
                    Dock = DockStyle.Fill,
                    AutoScrollOffset = new Point(0, 0),
                    BrowserSettings = new BrowserSettings()
                    {
                        ApplicationCacheDisabled = true,

                    }
                };

                (browser as IWebBrowser).JsDialogHandler = this;
                (browser as IWebBrowser).RequestHandler = this;
                (browser as IWebBrowser).FrameLoadEnd += LotBrowser_FrameLoadEnd;

                String _url = this.URL;
                _url = _url.Replace(";;;slash;;;", "/");
                _url = _url.Replace("%2F", "/");
                String[] query = _url.Split('&');

                foreach (var item in query)
                {
                    if (item.Split('=')[0].ToLower().Equals("eventid"))
                    {
                        this.eventID = item.Split('=')[1];
                        break;
                    }
                    else if (item.Split('=')[0].ToLower().Equals("event"))
                    {
                        this.eventID = item.Split('=')[1];
                        break;
                    }
                }

                this.WR = HttpUtility.ParseQueryString((new Uri(this.URL)).Query).Get("wr");

                if (string.IsNullOrEmpty(this.WR))
                {
                    string[] breakforWroom = this.URL.Split('?');
                    string[] split = breakforWroom[1].Split('&');

                    if (this.URL.Contains("/shop/#/"))
                    {
                        String w = this.URL.Substring(this.URL.IndexOf("#") + 2);
                        split = w.Split('/');
                        if (split[0].Contains("?"))
                        {
                            split = split[0].Split('?');
                            this.WR = split[0];
                        }
                        else
                        {
                            this.WR = split[0];
                        }
                    }

                    if (String.IsNullOrEmpty(this.WR))
                    {
                        foreach (var item in split)
                        {
                            if (item.Contains("wr="))
                            {
                                this.WR = item.Replace("wr=", "");
                                break;
                            }
                        }
                    }
                }

                this.VerticalScroll.Value = 0;
                this.Controls.Add(browser);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        void LotBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            try
            {
                //if (e.Url.Equals(this.url))
                if (e.IsMainFrame)
                {
                    Uri uri = new Uri(e.Url);

                    String[] _query = uri.Query.Split('&');

                    foreach (String item in _query)
                    {
                        String name = item.Split('=')[0];
                        String value = item.Split('=')[1];

                        if (name.ToLower().Contains("hash"))
                        {
                            _Close = true;
                        }
                    }

                    if (_Close)
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                this.Close();
                            }));
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #region IJsDialogHandler

        bool IJsDialogHandler.OnJSAlert(IWebBrowser browser, string url, string message)
        {
            return true;
        }

        bool IJsDialogHandler.OnJSBeforeUnload(IWebBrowser browser, string message, bool isReload, out bool allowUnload)
        {
            allowUnload = true;
            return true;
        }

        bool IJsDialogHandler.OnJSConfirm(IWebBrowser browser, string url, string message, out bool retval)
        {
            retval = true;
            return true;
        }

        bool IJsDialogHandler.OnJSPrompt(IWebBrowser browser, string url, string message, string defaultValue, out bool retval, out string result)
        {
            retval = true;
            result = "";
            return true;
        }
        #endregion

        #region IRequestHandler
        bool IRequestHandler.GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            bool handled = false;

            username = this.userName;
            password = this.password;
            handled = true;

            return handled;
        }

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect, bool isMainFrame)
        {
            return false;
        }

        bool IRequestHandler.OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, WebPluginInfo info)
        {
            return false;
        }

        CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browser, IRequest request, bool isMainFrame)
        {
            try
            {
                if (request.Url.ToLower().Contains("methodname=getphase") && request.Url.ToLower().Contains("lotid"))
                {
                    Uri uri = new Uri(request.Url);

                    String[] _query = uri.Query.Split('&');

                    foreach (String item in _query)
                    {
                        String name = item.Split('=')[0];
                        String value = item.Split('=')[1];

                        if (name.ToLower().Contains("lotid") && !value.ToLower().Contains("nolot"))
                        {
                            Console.WriteLine(request.Url);
                            String lotID = System.Web.HttpUtility.UrlDecode(value);

                            if (lotID.Contains(" "))
                            {
                                lotID = lotID.Replace(' ', '+');
                            }

                            Console.WriteLine(lotID);

                            if (!_isLotUpdated)
                            {
                                lotID = WR + "$" + eventID + "$" + lotID;
                                
                                UpdateLotID(lotID, AppPrefix);

                                try
                                {
                                    using (TcpClient client = new TcpClient("127.0.0.1", 12345))
                                    {
                                        BrowserRequest req = new BrowserRequest();
                                        req.LotID = lotID;
                                        req.AppPrefix = AppPrefix;
                                        req.Command = "updateLot";

                                        NetworkStream stream = client.GetStream();
                                        byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(req)) + "<EOF>");
                                        stream.Write(buffer, 0, buffer.Length);
                                        stream.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                }
                                _isLotUpdated = true;
                            }
                        }
                        else if (name.ToLower().Contains("hash"))
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    this.Close();
                                }));
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                    }
                }
                else if (request.Url.ToLower().Contains("hash=") && request.Url.ToLower().Contains("ts="))
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.Close();
                        }));
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else if (request.Url.ToLower().Contains("getphase") && this.URL.Contains("/shop/"))
                {
                    String[] _splits = request.Url.Split('/');
                    //MessageBox.Show(request.Url);
                    //MessageBox.Show(_splits.ElementAt(_splits.Length - 1));
                    String LotID = _splits[_splits.Length - 1];

                    try
                    {
                        if (!LotID.ToLower().Contains("lotid") && !LotID.ToLower().Contains("nolot"))
                        {
                            Console.WriteLine(request.Url);
                            Console.WriteLine(LotID);
                            String lotID = System.Web.HttpUtility.UrlDecode(LotID);

                            if (lotID.Contains(" "))
                            {
                                lotID = lotID.Replace(' ', '+');
                            }
                            //MessageBox.Show(lotID + " " + AppPrefix);
                            if (!_isLotUpdated)
                            {
                                Console.WriteLine(lotID);
                                lotID = WR + "$" + eventID + "$" + lotID;

                                UpdateLotID(lotID, AppPrefix);

                                String result = String.Empty;

                                try
                                {
                                    //TODO: update server address here

                                    using (TcpClient client = new TcpClient("127.0.0.1", 12345))
                                    {
                                        BrowserRequest req = new BrowserRequest();
                                        req.LotID = lotID;
                                        req.AppPrefix = AppPrefix;
                                        req.Command = "updateLot";

                                        NetworkStream stream = client.GetStream();
                                        byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(req)) + "<EOF>");
                                        stream.Write(buffer, 0, buffer.Length);
                                        stream.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                }

                                _isLotUpdated = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(e.StackTrace);
                    }
                }
                else if (request.Url.ToLower().Contains("sessioncreatew"))
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.Close();
                        }));
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else if (request.Url.ToLower().Contains("lotid=nolotid"))
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.Close();
                        }));
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return CefReturnValue.Continue;
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            return true;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            try
            {
                browser.Load(this.URL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {

        }
        #endregion

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
