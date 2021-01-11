using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace testThread
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(print);
            t1.Start();
            
            Thread.Sleep(1000);
            Console.WriteLine(t1.ThreadState);
        }

       static void print(){
           Thread.Sleep(5000);
           Console.WriteLine("hello");
       }
    }
}
