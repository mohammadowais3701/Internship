using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCF_NETPIPE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public int add(int num1, int num2)
        {
            Console.WriteLine(string.Format("You entered: {0} {1}", num1, num2));
            return num1 + num2;
        }
        public int sub(int num1, int num2)
        {
            return num1 - num2;
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
            // Console.WriteLine(info.myName);
            return info;
        }
        public String encoded(Encoded e)
        {
            return e.myText;
        }
    }
}
