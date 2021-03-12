using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCF_Duplex1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
   [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
    public class Service1 : IService1
    {
       IHelloWorldServiceCallback callback = null;
       public Service1() {

          callback= OperationContext.Current.GetCallbackChannel<IHelloWorldServiceCallback>();
       }
       
        public void ReceiveFile(string[] lines) {
                foreach (string line in lines)
                    Console.WriteLine(line);
            }
        public void Registered() {

            callback.greetings("Hello");
        }
  
        }


    }

