using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace StringRegex
{
    class Program
    {
        static void Main(string[] args)
        {
            string pattern = @"^The.*dog$";
            string str = "The quick brown fox jumps over the lazy dog";
            Regex reg = new Regex(pattern);
            var match = reg.Matches(str);
            Console.WriteLine(match[0].Value);
        }
    }
}
