using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethodExample
{
   static class Class1
    {
    public  static  int add(this int a, int b, int c) {
          return b + c;
       }
    public   static int subtract(this int a, int b,int c)
      {
          return b - c;
      }
    
    }
}
