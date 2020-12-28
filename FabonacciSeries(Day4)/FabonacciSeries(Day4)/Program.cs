using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabonacciSeries_Day4_
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 0, b = 1, c;

            for (int i = 0; i < 20; i++) {

                if (a == 0 && b==1)
                {
                    Console.WriteLine(a);
                    Console.WriteLine(b);
                }
               

                    c = a + b;
                    a = b;
                    b = c;
                    Console.WriteLine(c);
                
              
            }
        }

        


    }
}
