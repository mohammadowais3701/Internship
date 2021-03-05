using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXSEventsWebInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {                
                using (var host = new NancyHost(new Uri("http://axs1.ticketpeers.com:1235")))
                {
                    host.Start();
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
