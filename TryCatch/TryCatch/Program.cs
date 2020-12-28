using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryCatch
{
    class Program
    {
        static void Main(string[] args)
        {
            int c=0,a=2,b=0;
            try
            {
                c = a / b;

            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine("Caught={0}",e.Data);
            }
            finally{
            Console.WriteLine("Result={0}",c);
            }
        }
    }
}
