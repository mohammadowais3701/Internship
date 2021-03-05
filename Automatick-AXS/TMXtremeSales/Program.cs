using Automatick.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TCPClient;
using GeneralBucket;
using System.IO;

namespace Automatick
{
    static class Program
    {
        static String urlPermission = "https://license.ticketpeers.com/LicensingSystem/ValidateLicense.asmx/getApplicationPermissions?LicenseID=";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            try
            {
                LicenseCore lic = new LicenseCore("AAX",Application.StartupPath + @"\lic.lic", true);


                if (lic.ValidateLicense())
                {
                    if (!lic.CheckVersionUpdate())
                    {
                        ServerPortsPicker.createServerPortsPickerInstance(lic);
                        ValidationMessage.createTixToxSetting(lic.LicenseCode, lic.HardDiskSerial, lic.ProcessorID, lic.AppPreFix);
                        AccessRights.ApplicationPermissions per = new AccessRights.ApplicationPermissions(lic.GetLicenseID(), urlPermission);
                        Application.Run(new frmMain(per,lic));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an issue with the registration, contact provider" + Environment.NewLine + ex.Message);
            }

            SingleInstance.Stop();

            try
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
                if (p != null)
                {
                    if (p.Threads.Count > 0 && !p.HasExited)
                    {
                        p.Kill();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
               // MessageBox.Show(e.ExceptionObject + Environment.NewLine + e.ToString());
                //Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                File.AppendAllText(Application.StartupPath + @"\exception.txt", Environment.NewLine + DateTime.Now + " && MyHandler()=> " + e.ExceptionObject + " - " + e.ToString());
                //Environment.Exit(0);                
            }
            catch (Exception ex)
            {
               // MessageBox.Show(e.ExceptionObject + Environment.NewLine + e.ToString());
               // Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                //Environment.Exit(0);
            }
        }
    }
}
