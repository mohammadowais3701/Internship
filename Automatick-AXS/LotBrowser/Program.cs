using Awesomium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LotBrowser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            WebCore.Initialize(new WebConfig() { LogLevel = LogLevel.None });

            if (args != null && args.Length > 0)
                Application.Run(new browser(args[0]));

            try
            {
                if (WebCore.IsRunning)
                {
                    WebCore.Shutdown();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
