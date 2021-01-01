using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_Day8
{
    public delegate void dele(string str);
    public delegate int mydele();
    class Program
    {

       
        static void Main(string[] args)
        {
           dele d1 = A.func;
           dele d2 = B.func;
           dele d3 = d1 + d2;
          // d3.Invoke("Hello");

           dele d4 = (string s) => Console.WriteLine("Lamda Expression:"+s);

           d3 += d4;
          // d3("Hello ");

           Console.WriteLine();
           d3 = d3 - d2;
          // d3.Invoke("After Delete d2");
           mydele d5= A.func1;
           mydele d6 = B.func1;
           mydele d7 = d5 + d6;
           Console.WriteLine(d7());
          // d1.Invoke("Hello");
         //  d2("Hello");
        }
        
    }

    class A {

        static public void func(string str) {

            Console.WriteLine("In Class A:"+str);
        }
        static public int func1()
        {

            Console.WriteLine("In Class A:" );
            return 2;
        }

    }
    class B {
        static public void func(string str) {

            Console.WriteLine("In Class B:"+str);
        
        }
        static public int func1()
        {

            Console.WriteLine("In Class B:");
            return 4;
        }
    
    }
}
