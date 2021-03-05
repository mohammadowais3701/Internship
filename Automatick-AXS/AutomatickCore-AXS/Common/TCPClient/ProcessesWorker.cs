using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Automatick.Core
{
    public class ProcessesWorker
    {
        Boolean ifShowing = false;
        Process process = null;
        Dictionary<int, Process> _processes = null;
        int processId;
        Boolean _isWorking;
        System.Threading.Timer _timerValidateEachProxyServerLife = null;
        System.Threading.Timer _timerResetEachProxyServer = null;
        Process _informer = null;
        IMainForm _mainForm = null;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern int FindWindowEx(int hwndParent, int hwndEnfant, int lpClasse, string lpTitre);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public Dictionary<int, Process> Processes
        {
            get { return _processes; }
        }

        public Boolean IsWorking
        {
            get { return _isWorking; }
        }
        public IMainForm MainForm
        {
            get
            {
                return this._mainForm;
            }
        }

        public ProcessesWorker(IMainForm mainform)
        {
            this._mainForm = mainform;
            _processes = new Dictionary<int, Process>();
            _isWorking = false;
        }

        public void stop()
        {
            this._isWorking = false;

            if (this._processes != null)
            {
                lock (this._processes)
                {
                    Process processToStop = this._processes[processId];

                    if (!processToStop.HasExited)
                    {
                        processToStop.Kill();
                    }

                }
            }

            if (this._informer != null)
            {
                try
                {
                    if (!_informer.HasExited)
                    {
                        _informer.Kill();
                    }

                    _informer.Dispose();
                    GC.SuppressFinalize(_informer);
                }
                catch (Exception e)
                {
                    //Log e
                }
            }

            if (this._timerResetEachProxyServer != null)
            {
                this._timerResetEachProxyServer.Dispose();
                GC.SuppressFinalize(this._timerResetEachProxyServer);
            }

            if (this._timerValidateEachProxyServerLife != null)
            {
                this._timerValidateEachProxyServerLife.Dispose();
                GC.SuppressFinalize(this._timerValidateEachProxyServerLife);
            }

            // GC.Collect();
        }

        public void start()
        {

            this._isWorking = true;

            if (this._processes == null)
            {
                this._processes = new Dictionary<int, Process>();
            }

            string FileName = MainForm.AppStartUp.DefaultBucketFile;
            process = new Process();
            process.StartInfo.Arguments = "\"" + FileName + "\""; 
            process.StartInfo.FileName = Environment.CurrentDirectory + @"\Bucket.exe"; //@"C:\bots\Bucket\Bucket\bin\x86\Debug\Bucket.exe";//
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            process.EnableRaisingEvents = true;
            if (!this.ifShowing)
            {
                //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            process.Exited += new EventHandler(process_Exited);
            process.Start();
            processId = process.Id;

            lock (this._processes)
            {
                int index = this._processes.Count();
                this._processes.Add(processId, process);
            }

        }
        public void showHideConsoleWindow()
        {
            try
            {
                if (!this.ifShowing)
                {
                    IntPtr hWnd = IntPtr.Zero;

                    hWnd = (IntPtr)FindWindowEx(0, 0, 0, "TixToxSmrtick");

                    ShowWindow(hWnd, SW_SHOW);

                    this.ifShowing = true;
                }
                else
                {
                    IntPtr hWnd = IntPtr.Zero;

                    hWnd = (IntPtr)FindWindowEx(0, 0, 0, "TixToxSmrtick");

                    ShowWindow(hWnd, SW_HIDE);
                    this.ifShowing = false;

                }
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.Message);
            }
        }


        public void exit()
        {
            try
            {
                foreach (KeyValuePair<int, Process> item in this._processes)
                {
                    if (item.Key == processId)
                    {
                        EventArgs e = new EventArgs();
                        process_Exited(item.Value, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void process_Exited(object sender, System.EventArgs e)
        {
            // if (_isWorking)
            {
                Process exitedProcess = (Process)sender;
                if (exitedProcess != null)
                {
                    KeyValuePair<int, Process> processToStart = new KeyValuePair<int, Process>();
                    try
                    {
                        processToStart = this._processes.FirstOrDefault(p => p.Key.Equals(processId));
                    }
                    catch
                    { }
                    if (processToStart.Key != null && processToStart.Value != null)
                    {
                        try
                        {
                            if (!processToStart.Value.HasExited)
                            {
                                processToStart.Value.Kill();
                            }

                            lock (this._processes)
                            {
                                this._processes.Remove(processToStart.Key);
                            }
                            processToStart.Value.Dispose();
                            GC.SuppressFinalize(processToStart.Key);
                            //GC.Collect();
                            //this.ifShowing = true;
                            // this.MainForm.startTixTox();
                        }
                        catch
                        { }
                    }
                }
            }
        }

    }
    public enum WindowShowStyle : uint
    {
        Hide = 0,
        ShowNormal = 1,
        ShowMinimized = 2,
        ShowMaximized = 3,
        Maximize = 3,
        ShowNormalNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActivate = 7,
        ShowNoActivate = 8,
        Restore = 9,
        ShowDefault = 10,
        ForceMinimized = 11
    }
}
