using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;

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
      public void push(Stream data) {
          Console.WriteLine("Called");
           try
           {
               
               int l = 0;
               using (FileStream writer = new FileStream("bin\\mongo.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
               {
                 

                   var buff = new byte[20000];
                   try
                   {
                       int c = 0;
                     
                       
                       while ((c = data.Read(buff, c, 10000)) > 0)
                       {
                           writer.Write(buff, 0, c);
                           l += c;
                           Console.WriteLine("here");

                       }
                       writer.Close();
                       Console.WriteLine("Copied");
                   }
                   catch(Exception ex) {
                       Console.WriteLine("In While");
                       Console.WriteLine(ex.Message);
                   }
               }
             /*  using (var file = File.Create("bin\\mydoc.txt"))
               {
                  
                   data.CopyTo(file);
                   Console.WriteLine("File_Copied");
               }*/
           }
           catch(Exception ex) {
               Console.WriteLine(ex.Message);
           }
       }
        public int add(Stream data)
        {
            StreamReader reader = new StreamReader(data);
            string str = reader.ReadToEnd();
            numbers num_obj = JsonConvert.DeserializeObject<numbers>(str);
            return(num_obj.num1+num_obj.num2);  
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
    public class numbers
    {

        public int num1
        {
            get;
            set;

        }

        public int num2
        {
            get;
            set;

        }
    }
}
