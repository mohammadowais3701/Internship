using LotGenerator_Core;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefBrowser_LotIDGenerator
{
    public partial class LotController : Form
    {
        List<String> _proxy = null;
        List<int> _processID = null;
        int _totalBrowsers = 0;
        ConcurrentDictionary<int, String> _processProxies = null;
        ConcurrentDictionary<int, DateTime> _processTime = null;
        CancellationTokenSource cancellationToken = null;

        List<String> eventsToMonitor = null;

        public LotController()
        {
            InitializeComponent();

            _proxy = new List<string>();
            _processID = new List<int>();
            eventsToMonitor = new List<String>();
            _processProxies = new ConcurrentDictionary<int, string>();
            _processTime = new ConcurrentDictionary<int, DateTime>();

            //String config = Util.getConfig();

            //if (String.IsNullOrEmpty(config))
            //{
            //    throw new Exception("Unable to get config data from server");
            //}

            //String[] configuration = config.Split('&');

            //foreach (String item in configuration)
            //{
            //    String key = item.Split('=')[0];
            //    String value = item.Split('=')[1];

            //    if (key.Equals("browsers"))
            //    {
            //        this._totalBrowsers = int.Parse(value);
            //    }
            //    else if (key.Equals(""))
            //    {

            //    }
            //}

            btnOK_Click(this.btnOK, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            stopped = false;
            this.btnOK.Enabled = false;
            this.btnStop.Enabled = true;
            try
            {
                String result = Util.InitializeClient();
                cancellationToken = new CancellationTokenSource();

                if (!String.IsNullOrEmpty(result))
                {
                    this._totalBrowsers = Convert.ToInt32(result);
                }
                else
                {
                    MessageBox.Show("Unable to start. Try again.");
                    this.btnOK.Enabled = true;
                    this.btnStop.Enabled = false;
                    return;
                }

                //if (_totalBrowsers > 0)
                //{
                //    for (int i = 0; i < _totalBrowsers; i++)
                //    {
                Task mainTask = new Task(() =>
                {
                    //Task continuousCollector = new Task(() => collectEventsToMonitorContinuously(), TaskCreationOptions.LongRunning);
                    //continuousCollector.Start();

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            BrowserRequest request = null;

                            Util.ClientRequests.TryDequeue(out request);
                            //List<String> eventKeysToMonitor = eventsToMonitor.ToList();

                            //for (int x = eventKeysToMonitor.Count - 1; x >= 0; x--)
                            {
                                //String req = eventKeysToMonitor[x];

                                //eventKeysToMonitor.Remove(req);
                                //eventsToMonitor.Remove(req);

                                //String url = String.Empty;
                                //String proxy = String.Empty;

                                //ClientRequest request = JsonConvert.DeserializeObject<ClientRequest>(req);

                                //if (this._processID.Count < _totalBrowsers)
                                {
                                    if (request != null)
                                    {
                                        StartBrowserProcess(request.URL, request.AppPrefix, request.Proxy);
                                    }
                                }
                                //else
                                //{
                                //    //MessageBox.Show("Less");
                                //}
                            }
                        }
                        catch (Exception exe)
                        {
                            Debug.WriteLine(exe.Message);
                        }
                    }
                }, cancellationToken.Token, TaskCreationOptions.LongRunning);

                mainTask.Start();

                //        //String str = getNextProxy();

                //        ////MessageBox.Show(str);
                //        ////str = "216.45.55.52:80:myuser33:abc123";
                //        //////new LotBrowser(this.txtURL.Text, str).Show();
                //        ////this.txtURL.Text = "http://whatismyipaddress.com/";
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void StartBrowserProcess(String url, String proxy)
        {
            try
            {
                if (!String.IsNullOrEmpty(url) && !String.IsNullOrEmpty(proxy))
                {
                    Process process = new Process();
                    process.StartInfo.Arguments = url + " " + proxy;
                    process.StartInfo.FileName = System.Windows.Forms.Application.StartupPath + @"\CefLotBrowser.exe";
                    process.StartInfo.WorkingDirectory = System.Windows.Forms.Application.StartupPath;
                    process.EnableRaisingEvents = true;
                    process.Exited += process_Exited;
                    process.Start();

                    this._processID.Add(process.Id);
                    this._processProxies.TryAdd(process.Id, proxy);
                    this._processTime.TryAdd(process.Id, DateTime.Now);
                }
                else
                {
                    //MessageBox.Show("Eorrrrrr");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void StartBrowserProcess(String url, String appPrefix, String proxy)
        {
            try
            {
                if (!String.IsNullOrEmpty(url))// && !String.IsNullOrEmpty(proxy))
                {
                    Process process = new Process();
                    process.StartInfo.Arguments = url + " " + appPrefix + " " + proxy;
                    process.StartInfo.FileName = System.Windows.Forms.Application.StartupPath + @"\CefLotBrowser.exe";
                    process.StartInfo.WorkingDirectory = System.Windows.Forms.Application.StartupPath;
                    process.EnableRaisingEvents = true;
                    process.Exited += process_Exited;
                    process.Start();

                    this._processID.Add(process.Id);
                    this._processProxies.TryAdd(process.Id, proxy);
                    this._processTime.TryAdd(process.Id, DateTime.Now);
                }
                else
                {
                    //MessageBox.Show("Less");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void process_Exited(object sender, EventArgs e)
        {
            try
            {
                Process x = (Process)sender;
                this._processID.Remove(x.Id);

                String Proxy = String.Empty;
                DateTime dt = DateTime.Now;

                this._processProxies.TryRemove(x.Id, out Proxy);

                Util.ReleaseProxy(Proxy);

                this._processTime.TryRemove(x.Id, out dt);
            }
            catch (Exception ex)
            {

            }
        }

        long P_INC = 0;

        long C_S_COUNT = 0;

        private object locker = new object();
        bool stopped = false;

        public string getNextProxy()
        {
            string port = String.Empty;
            try
            {
                lock (locker)
                {
                    if (Interlocked.Read(ref P_INC) >= _proxy.Count())
                    {
                        Interlocked.Exchange(ref P_INC, 0);
                    }

                    port = _proxy[Convert.ToInt32(P_INC)];
                    Interlocked.Increment(ref P_INC);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return port;
        }

        private void collectEventsToMonitorContinuously()
        {
            try
            {
                while (stopped == false)
                {
                    try
                    {
                        String eventResult = Util.getEvent();

                        if (!String.IsNullOrEmpty(eventResult))
                        {
                            if (!eventResult.ToLower().Contains("not available"))
                            {
                                eventsToMonitor.Add(eventResult);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    Task.Delay(3000).Wait();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnOK.Enabled = true;
            this.btnStop.Enabled = false;

            cancellationToken.Cancel();

            Util.CloseClient();

            stopped = true;
        }
    }
}
