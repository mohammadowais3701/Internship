using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LotIdGenerator
{
    public partial class frmMain : Form
    {
        //System.Threading.Timer _refreshCountdownTimer = null;
        System.Windows.Forms.Timer _refreshCountdownTimer = null;
        private ConcurrentDictionary<String, ClientRequest> _clientUrlList;
        private string port = "44500";
        frmBrowser frmBrowser = null;
        List<frmBrowser> listBrowser = null;

        TcpListener _listener = null;
        Thread _listenerThread = null;

        private static int usingResourceForTokensGenerationsOnRequest = 0;
        private static int usingResourceOnRequest = 0;

        private int getTokenOnRequestCount
        {
            get
            {
                int tokenCount = 0;// Convert.ToInt32(InitialTokenPool) + 1;

                if (!this._clientUrlList.IsEmpty)
                {
                    if (0 == Interlocked.Exchange(ref usingResourceOnRequest, 1))
                    {
                        tokenCount = this._clientUrlList.Count + usingResourceForTokensGenerationsOnRequest;

                        //Release the lock
                        Interlocked.Exchange(ref usingResourceOnRequest, 0);
                    }
                }

                return tokenCount;
            }
        }

        public Boolean generateMoreOnRequestToken
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

        public frmMain()
        {
            _clientUrlList = new ConcurrentDictionary<String, ClientRequest>();
            listBrowser = new List<frmBrowser>();
            InitializeComponent();
            intializeListener();
            //this.notifyIcon1 = new NotifyIcon();
            this.notifyIcon1.Visible = true;
        }

        private void intializeListener()
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
                //this.Visible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private IPAddress getLocalIpAddress()
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

        private void work(object obj)
        {
            TcpListener serverListener = (TcpListener)obj;

            try
            {
                //_timer.Start();
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
                Debug.WriteLine(e.Message);
            }
        }

        private void handleClient(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            try
            {
                string messageData = TCPEncryptor.Decrypt(ReadMessage(stream));
                ClientRequest msg = JsonConvert.DeserializeObject<ClientRequest>(messageData);

                if (!String.IsNullOrEmpty(msg.URL))
                {
                    if (!this._clientUrlList.ContainsKey(msg.URL))
                    {
                        this._clientUrlList.TryAdd(msg.URL, msg);
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

        public void load()
        {
            try
            {
                if (this._refreshCountdownTimer == null)
                {
                    //this._refreshCountdownTimer = new System.Threading.Timer(this.refreshCountdownTimerCallBackHandler, null, new TimeSpan(0, 0, 20), new TimeSpan(0, 0, 20));
                    this._refreshCountdownTimer = new System.Windows.Forms.Timer();
                    this._refreshCountdownTimer.Tick += this.refreshCountdownTimerCallBackHandler;
                    this._refreshCountdownTimer.Interval = 1000 * 2;
                    this._refreshCountdownTimer.Start();
                    //this._refreshCountdownTimer
                }
                //else if (this._refreshBrowserTimer == null)
                //{
                //    this._refreshBrowserTimer = new System.Windows.Forms.Timer();
                //    this._refreshBrowserTimer.Tick += this.refreshBrowserTimerCallBackHandler;
                //    this._refreshBrowserTimer.Interval = 1000 * 3;
                //    this._refreshBrowserTimer.Start();
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void refreshCountdownTimerCallBackHandler(object sender, EventArgs e)
        {
            try
            {
                if (this.generateMoreOnRequestToken)
                {
                    //if (this.frmBrowser == null)
                    {
                        String key = this._clientUrlList.First().Key;
                        ClientRequest req = null;
                        Boolean result = this._clientUrlList.TryRemove(key, out req);

                        if (req != null)
                        {
                            frmBrowser tempBrowser = new frmBrowser();
                            tempBrowser.ClientRequest = req;
                            this.listBrowser.Add(tempBrowser);
                            //this.frmBrowser.showForm(req);
                        }
                    }
                    //else
                    //{
                    //    if (!this.frmBrowser.IsRunning)
                    //    {
                    //        this.frmBrowser.hideForm();
                    //        this.frmBrowser.Dispose();
                    //        this.frmBrowser = null;
                    //    }
                    //}
                }
                //else
                //{
                //    if (this.frmBrowser != null)
                //    {
                //        if (!this.frmBrowser.IsRunning)
                //        {
                //            this.frmBrowser.hideForm();
                //            this.frmBrowser.Dispose();
                //            this.frmBrowser = null;
                //        }
                //    }
                //}
                refreshBrowserTimerCallBackHandler();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void refreshBrowserTimerCallBackHandler()
        {
            try
            {
                if (this.listBrowser != null && this.listBrowser.Count > 0)
                {
                    foreach (frmBrowser itemBrowser in listBrowser)
                    {
                        if (!itemBrowser.IsOpened)
                        {
                            Debug.WriteLine("Opened");
                            itemBrowser.showForm();
                            break;
                        }
                        else if (itemBrowser.IsRunning && !itemBrowser.IsMouseClicked)
                        {
                            Debug.WriteLine("Running but Mouse not clicked");
                            break;
                        }
                        else if (!itemBrowser.IsRunning)
                        {
                            Debug.WriteLine("Not Running Mouse clicked - Remove it");
                            itemBrowser.hideForm();
                            itemBrowser.Dispose();
                        }
                    }

                    try
                    {
                        this.listBrowser.RemoveAll(pred => pred.IsOpened && !pred.IsRunning);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                //this.notifyIcon1.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void frmMain_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.Visible)
                {
                    if (FormWindowState.Minimized == WindowState)
                        Hide();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
