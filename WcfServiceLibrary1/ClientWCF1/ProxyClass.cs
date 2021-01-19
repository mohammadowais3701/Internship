using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using WcfServiceLibrary1;

namespace ClientWCF1
{
    public class ProxyClass : ClientBase<IService1>,
    IService1 {
        public int add(int num1, int num2) { 
        
         return   base.Channel.add( num1,num2);
        
        }
        public int sub(int num1, int num2)
        {

            return base.Channel.add(num1, num2);

        }
        public int mul(int num1, int num2)
        {

            return base.Channel.add(num1, num2);

        }
        public int div(int num1, int num2)
        {

            return base.Channel.add(num1, num2);

        }
    }
}
