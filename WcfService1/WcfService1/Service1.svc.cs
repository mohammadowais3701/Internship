using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
        public int add(int num1, int num2) {

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
            catch (Exception ex) {
                return 0;
            
            }
        }
        public Info GetInfo(Info info){
            return info;
        }
        public String encoded(Encoded e) {
            return e.myText;
        
        }
  
    }
}
