using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EvenOdd_MultiThreaded
{
    class Program
    {   static int check=1;
        static void Main(string[] args)
        {
            try
            {
                Thread t1 = new Thread(Odd);
                Thread t2 = new Thread(Even);
                t1.Start();
                t2.Start();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
        static void Even() {
            for (int i = 2; i <= 20;) {
                if (check == 2)
                {
                    Console.WriteLine(i);
                    i = i + 2;
                    check = 1;
                    Thread.Sleep(100);
                }  
            }
        }
        static void Odd() {
            for (int i = 1; i <= 20; )
            {
                if (check == 1)
                {
                    Console.WriteLine(i);
                    i = i + 2;
                    check = 2;
                    Thread.Sleep(100);
                }
            }
        }
    }
}
