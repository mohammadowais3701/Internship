using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCF_BASICHTTP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public string hello() { 
         return "{\"Text\":\"hello World\"}";
        }
       public string dataInsert(string name, Stream xyz) {
           Console.WriteLine(xyz);
           return "Data is Posted on server side";
         
       }
        public int add(string num1, string num2)
        {
           
            Console.WriteLine(string.Format("You entered: {0} {1}", num1, num2));
            return Convert.ToInt32(num1) + Convert.ToInt32(num2);
        }
        public int sub(string num1, string num2)
        {
            return Convert.ToInt32(num1) + Convert.ToInt32(num2);
        }
        public int mul(int num1, int num2)
        {
            return num1 * num2;
        }
        public int div(int num1, int num2)
        {
            try
            {
                return num1 / num2;
            }
            catch (ArithmeticException ex)
            {
                num2 = 1;
                return num1 / num2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public Info GetInfo(Info info)
        {
           Console.WriteLine("Client Name:{0}",info.myName);
           Console.WriteLine("Client Age:{0}",info.myAge);
            return info;
        }
        public String encoded(Encoded e)
        {
            return e.myText;
        }
    }
}
