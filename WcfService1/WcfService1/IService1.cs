using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

      

        // TODO: Add your service operations here
        [OperationContract]
        int add(int value1, int value2);
        [OperationContract]
        int sub(int val1, int val2);
        [OperationContract]
        int mul(int val1, int val2);
        [OperationContract]
        int div(int val1, int val2);
        [OperationContract]
        Info GetInfo(Info info);
        [OperationContract]
        String encoded(Encoded en);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Info {
        string name = "";
        int age = 0;
         [DataMember]
        public string myName {
            get { return "My Name is " + name; }
            set { name = value; }
        }
        [DataMember]
        public int myAge {
            get { return age; }
            set { age = value; }
        }
    
    }
    public class Encoded {
        string text = "";
        int key = 0;
        [DataMember]
        public string myText {
            get {
                char[] str = text.ToCharArray();
                for (int i = 0; i < (text.ToCharArray()).Length; i++) {
                    int k= Convert.ToInt32(str[i])+key;
                    str[i] = Convert.ToChar(k);

                }
                return new String(str);
            }
            set {
                text = value;
            }
        }
        [DataMember]
        public int Key {
          set { key = value; }
          get { return key; }
        }

    }
}
