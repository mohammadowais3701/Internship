using AmazonInstanceHandler;
using Gecko;
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
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace LotBrowser
{
    public partial class LotBrowserGecko : Form, nsIPromptService2, nsIPrompt, nsIObserver
    {
        GeckoWebBrowser browser = null;

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

        private static PromptService _promptService = new PromptService();

        public void Alert(nsIDOMWindow aParent, string aDialogTitle, string aText)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public LotBrowserGecko()
        {
            InitializeComponent();

            Gecko.Xpcom.Initialize();

            Xpcom.GetService<nsIObserverService>("@mozilla.org/observer-service;1").AddObserver(this, "http-on-modify-request", false);

            PromptFactory.PromptServiceCreator = () => this;

            browser = new GeckoWebBrowser()//("https://tickets.axs.com/shop/#/71eacb26-ec2c-4baa-8947-349e5212c44d/waiting-room/pick-tickets?skin=thefonda&lang=en&locale=en_us&preFill=1&eventid=313681&ec=GFT161217&src=AEGAXS1_WMAIN&fbShareURL=www.axs.com%2Fevents%2F313681%2Fan-evening-with-chris-robinson-brotherhood-tickets%3F%26ref%3Devs_fb")
            {
                Dock = DockStyle.Fill,
                AutoScrollOffset = new Point(0, 0),
            };

            CertOverrideService.GetService().ValidityOverride += (s, ex) =>
            {
                ex.OverrideResult = CertOverride.Mismatch | CertOverride.Time | CertOverride.Untrusted;
                ex.Temporary = true;
                ex.Handled = true;
            };

            this.browser.Navigate("https://tickets.axs.com/shop/#/71eacb26-ec2c-4baa-8947-349e5212c44d/waiting-room/pick-tickets?skin=thefonda&lang=en&locale=en_us&preFill=1&eventid=313681&ec=GFT161217&src=AEGAXS1_WMAIN&fbShareURL=www.axs.com%2Fevents%2F313681%2Fan-evening-with-chris-robinson-brotherhood-tickets%3F%26ref%3Devs_fb");

            browser.Navigated += browser_Navigated;
            browser.DocumentCompleted += browser_DocumentCompleted;
            browser.Navigating += browser_Navigating;

            this.VerticalScroll.Value = 0;
            this.Controls.Add(browser);
        }

        public LotBrowserGecko(String url, String appPrefix)
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

                Gecko.Xpcom.Initialize();

                Xpcom.GetService<nsIObserverService>("@mozilla.org/observer-service;1").AddObserver(this, "http-on-modify-request", false);

                PromptFactory.PromptServiceCreator = () => this;

                GeckoPreferences.User["general.useragent.override"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";//= "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Version/7.0 Mobile/11D257 Safari/9537.53";

                browser = new GeckoWebBrowser()
                {
                    Dock = DockStyle.Fill,
                    AutoScrollOffset = new Point(0, 0),
                };

                CertOverrideService.GetService().ValidityOverride += (s, ex) =>
                {
                    ex.OverrideResult = CertOverride.Mismatch | CertOverride.Time | CertOverride.Untrusted;
                    ex.Temporary = true;
                    ex.Handled = true;
                };

                browser.Navigated += browser_Navigated;
                browser.DocumentCompleted += browser_DocumentCompleted;
                browser.Navigating += browser_Navigating;
                browser.FrameNavigating += browser_FrameNavigating;

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

                    if (this.URL.Contains("/shop/#/") || this.URL.Contains("/#/"))
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

                if (this.URL.Contains("o2priority") && (this.URL.Contains("/shop/") || this.URL.Contains("/#/")))
                {
                    browser.Navigate(this.URL, Gecko.GeckoLoadFlags.None, "http://www.axs.com/uk", null, null);
                }
                else
                {
                    browser.Navigate(this.URL);
                }

                this.VerticalScroll.Value = 0;
                this.Controls.Add(browser);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        void browser_FrameNavigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            if (browser.Document != null)
            {
                if (!browser.Document.Cookie.Contains("1_" + WR + "={}"))
                {
                    browser.Document.Cookie = "1_" + WR + "={}";
                }
            }
        }

        public LotBrowserGecko(String url, String appPrefix, String proxy)
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

                if (_proxy.Length > 2)
                {
                    userName = _proxy[2];
                    password = _proxy[3];
                }

                Gecko.Xpcom.Initialize();

                try
                {
                    GeckoPreferences.User["browser.privatebrowsing.autostart"] = true;

                    GeckoPreferences.User["browser.cache.disk.enable"] = false;
                    GeckoPreferences.User["places.history.enabled"] = false;
                    GeckoPreferences.User["browser.cache.memory.enable"] = false;
                    GeckoPreferences.User["browser.cache.offline.enable"] = false;
                    GeckoPreferences.User["accessibility.disablecache"] = true;

                    nsICookieManager CookieMan;
                    CookieMan = Xpcom.GetService<nsICookieManager>("@mozilla.org/cookiemanager;1");
                    CookieMan = Xpcom.QueryInterface<nsICookieManager>(CookieMan);
                    CookieMan.RemoveAll();
                }
                catch (Exception ex)
                {

                }

                Xpcom.GetService<nsIObserverService>("@mozilla.org/observer-service;1").AddObserver(this, "http-on-modify-request", false);

                PromptFactory.PromptServiceCreator = () => this;

                GeckoPreferences.User["network.proxy.type"] = 1;
                GeckoPreferences.User["network.proxy.http"] = _proxy[0];
                GeckoPreferences.User["network.proxy.http_port"] = int.Parse(_proxy[1]);
                GeckoPreferences.User["network.proxy.ssl"] = _proxy[0]; ;
                GeckoPreferences.User["network.proxy.ssl_port"] = int.Parse(_proxy[1]);
                GeckoPreferences.User["network.proxy.ftp"] = _proxy[0]; ;
                GeckoPreferences.User["network.proxy.ftp_port"] = int.Parse(_proxy[1]);
                GeckoPreferences.User["network.proxy.socks"] = _proxy[0]; ;
                GeckoPreferences.User["network.proxy.socks_port"] = int.Parse(_proxy[1]);

                GeckoPreferences.User["general.useragent.override"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";//"Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Version/7.0 Mobile/11D257 Safari/9537.53";

                //browser = new ChromiumWebBrowser("http://whatismyipaddress.com/")
                browser = new GeckoWebBrowser()
                {
                    Dock = DockStyle.Fill,
                    AutoScrollOffset = new Point(0, 0)
                };

                CertOverrideService.GetService().ValidityOverride += (s, ex) =>
                {
                    ex.OverrideResult = CertOverride.Mismatch | CertOverride.Time | CertOverride.Untrusted;
                    ex.Temporary = true;
                    ex.Handled = true;
                };

                browser.Navigated += browser_Navigated;
                browser.DocumentCompleted += browser_DocumentCompleted;
                browser.Navigating += browser_Navigating;

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

                    if (this.URL.Contains("/shop/#/") || this.URL.Contains("/#/"))
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

                if (this.URL.Contains("o2priority") && this.URL.Contains("/shop/"))
                {
                    browser.Navigate(this.URL, Gecko.GeckoLoadFlags.None, "http://www.axs.com/uk", null, null);
                }
                else
                {
                    browser.Navigate(this.URL);
                }

                this.VerticalScroll.Value = 0;
                this.Controls.Add(browser);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        void browser_Navigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            try
            {
                if (browser.Document != null)
                {
                    if (!browser.Document.Cookie.Contains("1_" + WR + "={}"))
                    {
                        browser.Document.Cookie = "1_" + WR + "={}";
                    }
                }
                if (e.Uri.AbsoluteUri.ToLower().Contains("methodname=getphase") && e.Uri.AbsoluteUri.ToLower().Contains("lotid"))
                {
                    Uri uri = new Uri(e.Uri.AbsoluteUri);

                    String[] _query = uri.Query.Split('&');

                    foreach (String item in _query)
                    {
                        String name = item.Split('=')[0];
                        String value = item.Split('=')[1];

                        if (name.ToLower().Contains("lotid") && !value.ToLower().Contains("nolot"))
                        {
                            Console.WriteLine(e.Uri.AbsoluteUri);
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
                else if (e.Uri.AbsoluteUri.ToLower().Contains("hash=") && e.Uri.AbsoluteUri.ToLower().Contains("ts="))
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
                else if (e.Uri.AbsoluteUri.ToLower().Contains("getphase") && (this.URL.Contains("/shop/") || this.URL.Contains("/#/")))
                {
                    String[] _splits = e.Uri.AbsoluteUri.Split('/');
                    //MessageBox.Show(request.Url);
                    //MessageBox.Show(_splits.ElementAt(_splits.Length - 1));
                    String LotID = _splits[_splits.Length - 1];

                    try
                    {
                        if (!LotID.ToLower().Contains("lotid") && !LotID.ToLower().Contains("nolot"))
                        {
                            Console.WriteLine(e.Uri.AbsoluteUri);
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
                    catch (Exception exx)
                    {
                        //MessageBox.Show(e.StackTrace);
                    }
                }
                else if (e.Uri.AbsoluteUri.ToLower().Contains("sessioncreatew"))
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
                else if (e.Uri.AbsoluteUri.ToLower().Contains("lotid=nolotid"))
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
        }

        void browser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            try
            {
                if (browser.Document != null)
                {
                    if (!browser.Document.Cookie.Contains("1_" + WR + "={}"))
                    {
                        browser.Document.Cookie = "1_" + WR + "={}";
                    }
                }

                if (e.Uri.AbsoluteUri.ToLower().Contains("methodname=getphase") && e.Uri.AbsoluteUri.ToLower().Contains("lotid"))
                {
                    Uri uri = new Uri(e.Uri.AbsoluteUri);

                    String[] _query = uri.Query.Split('&');

                    foreach (String item in _query)
                    {
                        String name = item.Split('=')[0];
                        String value = item.Split('=')[1];

                        if (name.ToLower().Contains("lotid") && !value.ToLower().Contains("nolot"))
                        {
                            Console.WriteLine(e.Uri.AbsoluteUri);
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
                else if (e.Uri.AbsoluteUri.ToLower().Contains("hash=") && e.Uri.AbsoluteUri.ToLower().Contains("ts="))
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
                else if (e.Uri.AbsoluteUri.ToLower().Contains("getphase") && (this.URL.Contains("/shop/") || this.URL.Contains("/#/")))
                {
                    String[] _splits = e.Uri.AbsoluteUri.Split('/');
                    //MessageBox.Show(request.Url);
                    //MessageBox.Show(_splits.ElementAt(_splits.Length - 1));
                    String LotID = _splits[_splits.Length - 1];

                    try
                    {
                        if (!LotID.ToLower().Contains("lotid") && !LotID.ToLower().Contains("nolot"))
                        {
                            Console.WriteLine(e.Uri.AbsoluteUri);
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
                    catch (Exception exx)
                    {
                        //MessageBox.Show(e.StackTrace);
                    }
                }
                else if (e.Uri.AbsoluteUri.ToLower().Contains("sessioncreatew"))
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
                else if (e.Uri.AbsoluteUri.ToLower().Contains("lotid=nolotid"))
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
        }

        void browser_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            try
            {
                if (browser.Document != null)
                {
                    if (!browser.Document.Cookie.Contains("1_" + WR + "={}"))
                    {
                        browser.Document.Cookie = "1_" + WR + "={}";
                    }
                }
                //if (e.Url.Equals(this.url))
                //if ()
                {
                    Uri uri = e.Uri;

                    try
                    {
                        if (uri.AbsoluteUri.ToLower().Contains("methodname=getphase") && uri.AbsoluteUri.ToLower().Contains("lotid"))
                        {
                            String[] _query = uri.Query.Split('&');

                            foreach (String item in _query)
                            {
                                String name = item.Split('=')[0];
                                String value = item.Split('=')[1];

                                if (name.ToLower().Contains("lotid") && !value.ToLower().Contains("nolot"))
                                {
                                    Console.WriteLine(uri.AbsoluteUri);
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
                        else if (uri.AbsoluteUri.ToLower().Contains("hash=") && uri.AbsoluteUri.ToLower().Contains("ts="))
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
                        else if (uri.AbsoluteUri.ToLower().Contains("getphase") && (this.URL.Contains("/shop/") || this.URL.Contains("/#/")))
                        {
                            String[] _splits = uri.AbsoluteUri.Split('/');
                            //MessageBox.Show(request.Url);
                            //MessageBox.Show(_splits.ElementAt(_splits.Length - 1));
                            String LotID = _splits[_splits.Length - 1];

                            try
                            {
                                if (!LotID.ToLower().Contains("lotid") && !LotID.ToLower().Contains("nolot"))
                                {
                                    Console.WriteLine(uri.AbsoluteUri);
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
                            catch (Exception exx)
                            {
                                //MessageBox.Show(e.StackTrace);
                            }
                        }
                        else if (uri.AbsoluteUri.ToLower().Contains("sessioncreatew"))
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
                        else if (uri.AbsoluteUri.ToLower().Contains("lotid=nolotid"))
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

                    String[] _qu = uri.Query.Split('&');

                    foreach (String item in _qu)
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

        #region nsIPromptService2, nsIPrompt

        public void Alert(string dialogTitle, string text)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void AlertCheck(string dialogTitle, string text, string checkMsg, ref bool checkValue)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public bool Confirm(string dialogTitle, string text)
        {
            throw new NotImplementedException();
        }

        public bool ConfirmCheck(string dialogTitle, string text, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public int ConfirmEx(string dialogTitle, string text, uint buttonFlags, string button0Title, string button1Title, string button2Title, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool Prompt(string dialogTitle, string text, ref string value, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool PromptPassword(string dialogTitle, string text, ref string password, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool PromptUsernameAndPassword(string dialogTitle, string text, ref string username, ref string password, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool Select(string dialogTitle, string text, uint count, IntPtr[] selectList, ref int outSelection)
        {
            throw new NotImplementedException();
        }

        public void AlertCheck(nsIDOMWindow aParent, string aDialogTitle, string aText, string aCheckMsg, ref bool aCheckState)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public nsICancelable AsyncPromptAuth(nsIDOMWindow aParent, nsIChannel aChannel, nsIAuthPromptCallback aCallback, nsISupports aContext, uint level, nsIAuthInformation authInfo, string checkboxLabel, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool Confirm(nsIDOMWindow aParent, string aDialogTitle, string aText)
        {
            throw new NotImplementedException();
        }

        public bool ConfirmCheck(nsIDOMWindow aParent, string aDialogTitle, string aText, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public int ConfirmEx(nsIDOMWindow aParent, string aDialogTitle, string aText, uint aButtonFlags, string aButton0Title, string aButton1Title, string aButton2Title, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public bool Prompt(nsIDOMWindow aParent, string aDialogTitle, string aText, ref string aValue, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public bool PromptAuth(nsIDOMWindow aParent, nsIChannel aChannel, uint level, nsIAuthInformation authInfo, string checkboxLabel, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool PromptPassword(nsIDOMWindow aParent, string aDialogTitle, string aText, ref string aPassword, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public bool PromptUsernameAndPassword(nsIDOMWindow aParent, string aDialogTitle, string aText, ref string aUsername, ref string aPassword, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public bool Select(nsIDOMWindow aParent, string aDialogTitle, string aText, uint aCount, IntPtr[] aSelectList, ref int aOutSelection)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region nsIObserver

        public void Observe(nsISupports aSubject, string aTopic, string aData)
        {
            try
            {
                var request = Xpcom.QueryInterface<nsIHttpChannel>(aSubject);
                var x = request.GetURIAttribute();
                Uri url = x.ToUri();
                Debug.WriteLine(url.AbsoluteUri);

                if (browser.Document != null)
                {
                    if (!browser.Document.Cookie.Contains("1_" + WR + "={}"))
                    {
                        browser.Document.Cookie = "1_" + WR + "={}";
                    }
                }

                try
                {
                    if (url.AbsoluteUri.ToLower().Contains("methodname=getphase") && url.AbsoluteUri.ToLower().Contains("lotid"))
                    {
                        String[] _query = url.Query.Split('&');

                        foreach (String item in _query)
                        {
                            String name = item.Split('=')[0];
                            String value = item.Split('=')[1];

                            if (name.ToLower().Contains("lotid") && !value.ToLower().Contains("nolot"))
                            {
                                Console.WriteLine(url.AbsoluteUri);
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
                    else if (url.AbsoluteUri.ToLower().Contains("hash=") && url.AbsoluteUri.ToLower().Contains("ts="))
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
                    else if (url.AbsoluteUri.ToLower().Contains("getphase") && (this.URL.Contains("/shop/") || this.URL.Contains("/#/")))
                    {
                        String[] _splits = url.AbsoluteUri.Split('/');

                        String LotID = _splits[_splits.Length - 1];

                        try
                        {
                            if (!LotID.ToLower().Contains("lotid") && !LotID.ToLower().Contains("nolot"))
                            {
                                Console.WriteLine(url.AbsoluteUri);
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
                        catch (Exception exx)
                        {
                            //MessageBox.Show(e.StackTrace);
                        }
                    }
                    else if (url.AbsoluteUri.ToLower().Contains("sessioncreatew"))
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
                    else if (url.AbsoluteUri.ToLower().Contains("lotid=nolotid"))
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
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
