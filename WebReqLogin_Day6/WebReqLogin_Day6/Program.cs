using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;   

namespace WebReqLogin_Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WebRequest req = WebRequest.Create("http://www.contoso.com/");
                WebResponse res = req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());
            }
            catch { 
            
            
            }

           
        }
    }
}
