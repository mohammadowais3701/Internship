using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace AsyncDelegates_15_01_2021_
{
    class Program
    {
        public delegate int myCalc(int a);
        static void Main(string[] args)
        {
            try
            {
                Calculation obj = new Calculation();
                myCalc cal = new myCalc(obj.add);
                myCalc mul = new myCalc(obj.factorial);
                IAsyncResult res = cal.BeginInvoke(10, new AsyncCallback(mycallback),"This is callback after addition");
                //while (!res.IsCompleted) {
                //      Console.WriteLine("In Process");
            
                //  }
                IAsyncResult res1 = mul.BeginInvoke(5, new AsyncCallback(mycallback), "This is call back After factorial");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();

        }
        static void mycallback(IAsyncResult res) {
            try
            {
                String str = (String)(res.AsyncState);
                Console.WriteLine(str);
                AsyncResult r = (AsyncResult)res;   
                myCalc del = (myCalc)r.AsyncDelegate;
                int result = del.EndInvoke(res);
                Console.WriteLine(result);
               
            }
            catch(Exception ex) {

                Console.WriteLine(ex.Message);
            }
        
        }

     
    }
    public class Calculation
    {

        public int add(int n)
        {
            int sum = 0;
            try
            {
                for (int i = 1; i <= n; i++)
                {
                    sum += i;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sum;
        }
        public int factorial(int n)
        {
            int factorial = 1;
            try
            {
                for (int i = 1; i <= n; i++)
                {
                    factorial *= i;
                   // Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return factorial;
        }

    } 

}
