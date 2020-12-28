using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace test
{
    class Program
    {

      

        static void callToChildThread() {
            try
            {
                Console.WriteLine("Child Thread Start");
                for (int i = 0; i <= 10; i++) {
                    Thread.Sleep(500);
                    Console.WriteLine(i);
                
                }

            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine("Thread Abort Exception {0}", e);
            }
            finally {
                Console.WriteLine("Thread Exception did not occur");
            }
        }
        static void Main(string[] args)
        {
            //Thread t = Thread.CurrentThread;
            //t.Name="Main Thread";
          //  Console.WriteLine("{0}",t.ManagedThreadId);
           
            Console.WriteLine("Creating Child Thread");
            Thread childThread = new Thread(callToChildThread);
            Thread childThread2 = new Thread(callToChildThread);

            childThread.Start();
          //childThread2.Start();
           

           Thread.Sleep(3000);
           childThread2.Start();
         //   Console.WriteLine("Thread Aborted");
           // childThread.Abort();



        }
    }
}
