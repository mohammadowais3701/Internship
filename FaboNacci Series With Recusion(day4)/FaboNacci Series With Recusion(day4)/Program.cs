using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaboNacci_Series_With_Recusion_day4_
{
    class Program
    {
        static void Main(string[] args)
        {

            int n;
            Console.WriteLine("How many Numbers do you want to print?");

            try
            {
                n = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception e){
                Console.WriteLine("Something was wrong");
                Console.WriteLine(e.Message);
                n = 10;
            
            }
            int a = 0, b = 1;
            Console.WriteLine(a);
            Console.WriteLine(b);
            fab(a, b, 0, n-2);
        }

     static  void fab(int a, int b,int s, int n) {
            int c;
            if (s==n) {
                return ;
            
            }


            c = a + b;
          Console.WriteLine(c);
            fab(b, c,s+1, n);
           
        
        
        
        }


    }
}
