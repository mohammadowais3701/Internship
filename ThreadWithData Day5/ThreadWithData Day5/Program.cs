using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadWithData_Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(print);
        
            
            t1.Start("ABC World");
            t1.Join();
            for (int i = 0; i < 10; i++) {
               
                new Thread(()=>{
                    int temp = i;
                    Console.WriteLine(temp);
                  
                
                }).Start();
            
            }
            Console.ReadLine();
        }

        static void print(Object msg) {

            String str = (String)(msg);
            Console.WriteLine(str);
        
        }
    }
}
