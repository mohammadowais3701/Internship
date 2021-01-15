using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {

            for (int i = 0; i < 10; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(MyMethod), i);
                Thread.Sleep(1000);
            }
            Console.Read();
        }
          public static void MyMethod(object obj)
        {
            Thread thread = Thread.CurrentThread;
            string message = "Background:"+ thread.IsBackground +"Thread Pool:"+ thread.IsThreadPoolThread +", Thread ID:"+ obj.ToString();
            Console.WriteLine(message);
        }
    }
}
