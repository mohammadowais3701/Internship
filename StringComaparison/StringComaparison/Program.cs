using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringComaparison
{
    class Program
    {
        static void Main(string[] args)
        {
            String str1="dfd",str2="ghi";
            try
            {
                Console.WriteLine(String.Compare(str1,str2));


            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            
            }
            Console.ReadKey();
        }
    }
}
