using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StopWatch_Day4_
{
    class Program
    {
        static DateTime d1;

        static int s = 2;

        static void Main(string[] args)
        {

            UI();

        }
        static void StopWatch()
        {

            while (true)
            {
                if (s == 1)
                {
                    try
                    {
                        Console.SetCursorPosition(15, 16);
                        d1 = d1.AddSeconds(1);
                        Console.WriteLine(d1.ToString("HH:mm:ss"));
                        Console.SetCursorPosition(0, 8);
                        Thread.Sleep(1000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Problem in condition 1");
                        Console.WriteLine(e.Message);
                    }
                }
                else if (s == 2)
                {
                    try
                    {
                        Console.SetCursorPosition(15, 16);
                        Console.WriteLine(d1.ToString("HH:mm:ss"));
                        Console.SetCursorPosition(0, 8);
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Problem in condition 2");
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (s == 3)
                {
                    try
                    {
                        d1 = new DateTime(0001, 1, 1, 0, 0, 0);
                        Console.SetCursorPosition(15, 16);
                        Console.WriteLine(d1.ToString("HH:mm:ss"));
                        Console.SetCursorPosition(0, 8);
                        Thread.Sleep(100);
                        s = 1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Problem in condition 3");
                        Console.WriteLine(e.Message);
                    }
                }

            }

        }
        static void UI()
        {

            Console.SetCursorPosition(15, 0);
            Console.WriteLine(" Enter Number \n\t\t1 for Start\n\t\t2 for Stop \n\t\t3 for Reset");
            Console.Clear();
            Thread t2 = new Thread(StopWatch);
           
            try
            {
                t2.Start();
                t2.IsBackground = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error for Starting Stopwatch Thread");
                Console.WriteLine(e.Message);

            }
            while (s==1 || s==2 || s==3)
            {
                Console.Clear();
                Console.SetCursorPosition(15, 0);
                Console.WriteLine(" Enter Number \n\t\t1 for Start\n\t\t2 for Stop \n\t\t3 for Reset");
                s = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }
        }
    }

}
