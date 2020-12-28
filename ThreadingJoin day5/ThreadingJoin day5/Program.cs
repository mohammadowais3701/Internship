using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadingJoin_day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(MethodA);
            Thread t2 = new Thread(MethodB);
            t1.Start();
            t1.Join();
            t2.Start();

            Console.WriteLine("Main Thread Ended here");
        }

        static void MethodA() {
            for (int i = 0; i < 10; i++) {
                Console.WriteLine("Method A  "+i);
                Thread.Sleep(1000);
            }
        
        }
        static void MethodB()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Method B  " + i);
                Thread.Sleep(1000);
            }

        }
    }
}
