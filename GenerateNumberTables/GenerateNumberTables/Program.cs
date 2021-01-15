using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GenerateNumberTables
{
    class Program
    {
        static void Main(string[] args)
        {
            generateTable();
        }

        static void generateTable()
        {
            int n, th;
     
            try
            {
                Console.WriteLine("Enter Number of threads");
                th = Convert.ToInt32(Console.ReadLine());
                 ThreadPool.SetMinThreads(1,1);
                 ThreadPool.SetMaxThreads(th, th);  
               
                    ThreadPool.QueueUserWorkItem(new WaitCallback(mytable1), 1);
                    Thread.Sleep(1000);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(mytable2), 2);
                    Thread.Sleep(1000);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(mytable3), 3);
                    Thread.Sleep(1000);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(mytable4), 4);


                    Console.ReadKey();
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void mytable1(Object i)
        {
          // Thread.CurrentThread.IsBackground = false;
            Console.WriteLine("Thread={0}",i);
           
          //  Console.WriteLine(Thread.CurrentThread);
            Int32 k = Convert.ToInt32(i);

            for (int m = 1; m <= 10; m++)
            {
                Console.WriteLine("Thread={0}-->{1}", i, m);
                Thread.Sleep(500);      
           }
        }
        static void mytable2(Object i)
        {
          // Thread.CurrentThread.IsBackground = false;
            Console.WriteLine("Thread={0}", i);

            //  Console.WriteLine(Thread.CurrentThread);
            Int32 k = Convert.ToInt32(i);

            for (int m = 1; m <= 10; m++)
            {
                Console.WriteLine("Thread={0}-->{1}", i, m);
                Thread.Sleep(500);      
            }
        }
        static void mytable3(Object i)
        {
            //Thread.CurrentThread.IsBackground = false;
            Console.WriteLine("Thread={0}", i);
            Int32 k = Convert.ToInt32(i);
            for (int m = 1; m <= 10; m++)
            {
                Console.WriteLine("Thread={0}-->{1}", i, m);
                Thread.Sleep(500);      
            }
        }
        static void mytable4(Object i)
        {
          //  Thread.CurrentThread.IsBackground = false;
            Console.WriteLine("Thread={0}", i);
            Int32 k = Convert.ToInt32(i);
            for (int m = 1; m <= 10; m++)
            {
                Console.WriteLine("Thread={0}-->{1}", i, m);
                Thread.Sleep(500);      
                }
        }
    }
}
