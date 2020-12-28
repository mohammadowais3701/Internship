using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_Day5
{
    public delegate void MyDelegate(String str);
    class Program
    {

        static void Main(string[] args)

        {
            try
            {
                MyDelegate d1 = new MyDelegate(Method1);
                d1.Invoke("Hello");
                MyDelegate d2 = (string str) => Console.WriteLine("Lamda Equation " + str);
                d2.Invoke("hello");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            
            }
        }


        static public void Method1(string str){
            Console.WriteLine("In Method 1="+str);

        
        
        }
        static public String Method2(string str) {

            return "In Method 2" + str;
        
        }
    }
}
