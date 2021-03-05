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
        static void Main(String[] args)
        {
            try
            {
                //MessageBox.Show("");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (args != null && args.Length > 0)
                {
                    if (args.Length == 3)
                    {
                        Application.Run(new LotBrowserGecko(args[0], args[1], args[2])); 
                    }
                    else
                    {
                        Application.Run(new LotBrowserGecko(args[0], args[1]));
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + args[0] + " " + args[1] + " " + args[2] + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
