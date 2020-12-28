using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThreading
{
    class Program
    {
      

        static void Main(string[] args)
        {
            Thread t1, t2, t3;
            try
            {
                 t1 = new Thread(Display1);
                 t1.Start();
                 t1.IsBackground = true;
                 Thread.Sleep(3000);
                
            }
            catch (Exception e) {

                Console.WriteLine(e.Message);
            
            }

            try
            {
                t2 = new Thread(Display2);
                t2.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem in 2nd Thread");
                Console.WriteLine(e.Message);

            }

            try
            {

                t3 = new Thread(Display3);
                t3.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem in 3rd Thread");
                Console.WriteLine(e.Message);

            }
            
                 
        
          

            Console.WriteLine("Main Thread Ends here");

            //Console.ReadKey();
            
           


        }
     static   void Display1() {
         
         Console.WriteLine("In Display 1");

         try
         {
             for (int i = 1; i <= 5; i++)
             {
                 Console.WriteLine("Display1={0}", i);
                 Thread.Sleep(1000);


             }
             Console.WriteLine("Display 1 Ends here");
         }
         catch (Exception e) {
             Console.WriteLine(e.Message);
         
         
         }
        
        }
    static    void Display2() {

        Console.WriteLine("In Display 2");
        try
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine("Display2={0}", i);
                Thread.Sleep(1400);

            }
            Console.WriteLine("Display 2 Ends here");
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
        
        }

        }
    static void Display3() {
        Console.WriteLine("In Display 3");
        try
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine("Display3={0}", i);
                Thread.Sleep(1600);

            }
            Console.WriteLine("Display 3 Ends here");
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
            
        }
    
    }

    }
}
