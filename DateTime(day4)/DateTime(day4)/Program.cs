using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTime_day4_
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DateTime d1 = new DateTime(2016,8,6,11,49,00);
                DateTime d6 =new DateTime(2101,1,1);
                DateTime d2 = new DateTime(2006, 5, 2);
                DateTime d3 = new DateTime(2005, 12, 31, 15, 12, 20);
                DateTime d4 = new DateTime(2002, 10, 25, 12, 2, 23, DateTimeKind.Utc);
                TimeSpan ts = new TimeSpan(d6.Subtract(d1).Ticks * 100);
               // Console.WriteLine(ts);
                
               
              //  Console.WriteLine(d6.Ticks);
               // Console.WriteLine(DateTime.UtcNow.Ticks);

                //Console.WriteLine(DateTime.UtcNow.Subtract(d1).Ticks);
              //  Console.WriteLine(DateTime.UtcNow.ToLocalTime());
              //  Console.WriteLine(d3);
               // Console.WriteLine(d4);
                Console.WriteLine(d1);
                Console.WriteLine(d1.Date.ToString("d"));
                Console.WriteLine(d1.ToLocalTime());
                Console.WriteLine(d1.ToLocalTime().ToString("H:m"));
                Console.WriteLine(d1.ToShortDateString());
              //  Console.WriteLine(DateTime.Now.ToShortDateString());
                Console.WriteLine();




            }

            catch (Exception e) {
                Console.WriteLine(e.Message);
            
            }
           
            
            Console.WriteLine();
        }
    }
}
