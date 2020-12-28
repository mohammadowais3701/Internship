using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack1
{
    class Program
    {
      
        static void Main(string[] args)
        {

            myStack s1 = new myStack();

            s1.push(2);
            s1.push(3);
            s1.push(4);
            s1.push(5);
          
            Console.WriteLine(s1.pop());




           



        }

        
    }
}
