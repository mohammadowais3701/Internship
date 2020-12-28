using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate1_Day5
{
    public delegate void mydel(String msg);
    class Program
    {
        static void Main(string[] args)
        {
            mydel d1 = A.Method1;
            callDelegate(d1);

        }

        static void callDelegate(mydel d1) {
            d1.Invoke("Hello");
        
        }
    }

    class A {
     static public void Method1(String str) {
         Console.WriteLine("In Method1 " + str);
        
        }
    
    }
}
