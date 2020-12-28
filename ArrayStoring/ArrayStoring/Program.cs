using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayStoring
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = new int[] { 2, 3, 1, 7, 4, 2, 8, 9 };
            //Array.Sort(numbers);
     
            //Array.Reverse(numbers);
        
            Console.WriteLine(Array.BinarySearch(numbers,8));
        }
    }
}
