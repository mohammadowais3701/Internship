using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCF_NETPIPE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {



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

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "WcfServiceLibrary1.ContractType".
    [DataContract]
    public class Info
    {
        string name = "";
        int age = 0;
        [DataMember]
        public string myName
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public int myAge
        {
            get { return age; }
            set { age = value; }
        }

    }
    [DataContract]
    public class Encoded
    {
        string text = "";
        int key = 0;

        [DataMember]
        public string myText
        {
            get
            {
                char[] str = text.ToCharArray();
                for (int i = 0; i < (text.ToCharArray()).Length; i++)
                {
                    int k = Convert.ToInt32(str[i]) + key;
                    str[i] = Convert.ToChar(k);

                }
                return new String(str);
            }
            set
            {
                text = value;
            }
        }
        [DataMember]
        public int Key
        {
            set { key = value; }
            get { return key; }
        }

    }
}
