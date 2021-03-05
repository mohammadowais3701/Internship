using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace Automatick.Core
{
    [Serializable]
    public class AutobuyLogs
    {
        public String TicketName { get; set; }

        public String TicketURL { get; set; }

        public String AccountName { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public Dictionary<String, String> Responses { get; set; }

        public static Object locker = new Object();

        public AutobuyLogs(String ticketName, String ticketUrl)
        {
            this.TicketName = ticketName;
            this.TicketURL = ticketUrl;
            Responses = new Dictionary<string, string>();
        }

        public override String ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public String MakeValidFileName(String name)
        {
            String invalidChars = Regex.Escape(new String(Path.GetInvalidFileNameChars()));
            String invalidReStr = String.Format(@"[{0}]+", invalidChars);
            String file = Regex.Replace(name, invalidReStr, "");
            if (file.Length > 250)
            {
                file = file.Substring(0, 250);
            }
            return file;
        }

        public void WriteToFile(String path)
        {
            try
            {
                lock (locker)
                {
                    String fileName = MakeValidFileName(this.TicketName + "__" + this.AccountName + ".txt");

                    File.AppendAllText(path + fileName, Environment.NewLine + DateTime.Now + " => " + this.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
