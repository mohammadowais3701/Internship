using Automatick.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using TCPClient;

namespace Automatick.Core
{
    public class ClientResponse
    {
        public String Token
        {
            get;
            set;
        }
    }

    public class MessageRequest
    {
        /// <summary>
        /// for Client LicenceCode to verify
        /// </summary>
        public String LicenseCode
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the person using, might be typer or client
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// for client -> getRecaptchaToken
        /// for typer -> sendRecaptchaToken
        /// </summary>
        public String Command
        {
            get;
            set;
        }

        /// <summary>
        /// Request message to/for specific site
        /// </summary>
        public String AppPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// Request message to/for specific site key
        /// </summary>
        public String Key
        {
            get;
            set;
        }

        /// <summary>
        /// Server usageValue
        /// </summary>
        public String Token
        {
            get;
            set;
        }

        /// <summary>
        /// Client has to set his IP
        /// </summary>
        public String IP
        {
            get;
            set;
        }

        /// <summary>
        /// For ServerUse only
        /// </summary>
        public Boolean IsUseToken
        {
            get;
            set;
        }

        public int Need
        {
            get;
            set;
        }
    }

    public class Captchator//: IAutoCaptchaService
    {
        static Captchator _captchator = null;

        static int counter;

        //public static int CaptchatorWorkers;

        public long CaptchatorWorkers;

        public long Requested = 0;

        private Boolean isRunning = false;

        private static Boolean result = false;

        private static TcpClient _client;

        protected Task worker;

        public CancellationTokenSource cancelSource;

        public static MessageRequest _msgReq;

        public ConcurrentBag<String>  recaptokens { get; set; }

        public static TcpClient Client
        {
            get { return _client; }
            set { _client = value; }
        }
       
        private static String localIP()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        public AutoCaptchaServices autoCaptchaServices
        {
            get;
            set;
        }

        public static Captchator CaptchatorInstance
        {
            get
            {
                if (_captchator == null)
                {
                   // throw new Exception("captchator Object is not created");
                }
                return _captchator;

            }
        }

        public Captchator(AutoCaptchaServices _autoCaptchaServices, int requests)
        {
            CaptchatorWorkers = requests;
            autoCaptchaServices = _autoCaptchaServices;
            this.cancelSource = new CancellationTokenSource();
            recaptokens = new ConcurrentBag<String>();
            Task.Run(() => makeConnection());
        }

        public static void createCaptchatorInstance(AutoCaptchaServices _autoCaptchaServices, int requests)
        {
            _captchator = new Captchator(_autoCaptchaServices, requests);
        }

        public async void getRecapToken(string key)
        {
            try
            {
                if (_client.Connected)
                {
                    Interlocked.Increment(ref counter);
                    _msgReq.Name = autoCaptchaServices.CTRUserName.Trim() + " " + autoCaptchaServices.CTRPassword.Trim();
                    _msgReq.Key = key;
                    byte[] buffer = Encoding.UTF8.GetBytes(TCPCaptchatorEncryptor.Encrypt(JsonConvert.SerializeObject(_msgReq)) + "<EOF>");
                    _client.GetStream().Write(buffer, 0, buffer.Length);
                    _client.GetStream().Flush();
                    result = true;
                    Debug.WriteLine("Request send to captchator");
                }
                else
                {
                    makeConnection();
                }
            }
            catch (Exception)
            {
                makeConnection();
            }

            if (!isRunning)
            {
                await Task.Run(() => takeRecaptchaToken());
            }

        }

        public void makeConnection()
        {
            try
            {
                if (!String.IsNullOrEmpty(autoCaptchaServices.CTRUserName) && !String.IsNullOrEmpty(autoCaptchaServices.CTRPassword))
                {
                    if ((_client == null) || (!_client.Connected))
                    {
                        _client = new TcpClient(autoCaptchaServices.CTRIP, Convert.ToInt32(autoCaptchaServices.CTRPort));
                    }

                    if (_msgReq == null)
                    {
                        _msgReq = new MessageRequest();
                        _msgReq.Name = autoCaptchaServices.CTRUserName.Trim() + " " + autoCaptchaServices.CTRPassword.Trim();
                        _msgReq.Command = "getRecaptchaToken";
                        _msgReq.AppPrefix = "ATM";
                        _msgReq.Need = 1;
                    }
                    _msgReq.IP = localIP();
                }

            }
            catch
            {
                Thread.Sleep(500);
                makeConnection();
            }

        }

        private String takeRecaptchaToken()
        {
            try
            {
                while (result)
                {
                    try
                    {
                        Thread.Sleep(1000);
                        isRunning = true;
                        string message =Reader.ReadMessage(_client.GetStream());

                        if (!TCPCaptchatorEncryptor.Decrypt(message).ToLower().Equals("ping"))
                        {
                            ClientResponse realMsg = JsonConvert.DeserializeObject<ClientResponse>(TCPCaptchatorEncryptor.Decrypt(message));

                            if ((realMsg != null) && (!String.IsNullOrEmpty(realMsg.Token)))
                            {
                                 recaptokens.Add(realMsg.Token);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result = false;
                        isRunning = false;
                    }
                }

                return string.Empty;
            }
            catch (Exception e)
            {
                _client.Close();
                isRunning = false;
                Interlocked.Decrement(ref counter);
               // Statistic.incrementFailed();
                if (counter < 0) counter = 0;
                Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                return string.Empty;
            }
        }

        protected async void work()
        {
            while (true)
            {
                try
                {
                    if (counter < CaptchatorWorkers) //--- have to change this CaptchatorWorkshop.Instance.Storage.Count()
                    {
                        if (worker != null)
                        {
                            try
                            {
                                await Task.Run(() => { getRecapToken(""); });

                                if (this.cancelSource.Token.IsCancellationRequested)
                                {
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.ToString());
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (this.cancelSource.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    Debug.WriteLine(e.ToString());
                }

                await Task.Delay(500);
            }
        }

        public void start()
        {
            try
            {
                this.worker = Task.Factory.StartNew(() => work(),
                                                    this.cancelSource.Token,
                                                    TaskCreationOptions.LongRunning,
                                                    TaskScheduler.Current
                );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public void stop()
        {
            try
            {
                this.cancelSource.Cancel();
                isRunning = false;
                this.worker = null;
            }
            catch
            { }
        }

        public void Remove(List<Captcha> list)
        {
            try
            {
                Parallel.ForEach(list, (c) =>
                {
                    if ((c != null) && (c.captchaentered != null))
                    {
                        list.Remove(c);
                    }
                });
            }
            catch
            { }
        }

        public string getCaptchatorToken()
        {
            try
            {
                lock (this.recaptokens)
                {
                    if (this.recaptokens.Count > 0)
                    {
                        string c;
                        this.recaptokens.TryTake(out  c);
                        return c;
                    }
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
