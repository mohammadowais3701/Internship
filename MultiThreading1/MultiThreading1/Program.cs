using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThreading1
{
    class Program
    {
       
      static  string str = "";
        static void Main(string[] args)
        {
            Thread t1 = new Thread(Method1);
            Thread t2 = new Thread(Method2);
            t1.Start();
            t2.Start();

            Thread.Sleep(5000);
            t1.Abort();
           
            t2.Abort();

            Console.WriteLine("Main Thread Ends Here");
            Console.ReadKey();

        }
    static  void Method1()
        {
            Console.WriteLine("In Method 1");
            try
            {
                while (true)
                {
                    str = "Method1";

                    lock (str)
                    {
                        if (String.Equals(str,"Method2"))
                        {
                            str = "Method1";
                        }


                        Console.WriteLine("Method 1-->{0}", str);

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);


            }





        }

    static  void Method2() {
            Console.WriteLine("In Method 2");
            try
            {
                while (true)
                {

                    lock (str)
                    {
                        if (String.Equals(str, "Method1"))
                        {
                            str = "Method2";
                        }


                        Thread.Sleep(1000);

                        Console.WriteLine("Method 2-->{0}", str);

                    }

                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            
            
            }
            

        
        
        
        }
    }
}
