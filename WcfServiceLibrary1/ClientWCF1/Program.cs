using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ServiceModel;

//using ClientWCF1.ServiceReference1;

namespace ClientWCF1
{
    
    class Program
    {
        
        static void Main(string[] args)
        {
           /* string address = "http://localhost:8733/WcfServiceLibrary1/Service1";
            string binding = "wsHttpBinding";
            string contract = "IService1";*/
            WSHttpBinding wsBindind = new WSHttpBinding();
            EndpointAddress endpointAddress = new EndpointAddress("http://localhost:8733/WcfServiceLibrary1/Service1");
           // MyContractClient proxy = new MyContractClient(""); 
            
            /*Info info;
            Service1Client client;
            try
            {
                client= new Service1Client();


                int a, b, age;
                string name, text;
                try
                {
                    Console.WriteLine("Enter 1st Number");
                    a = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter 2nd Number");
                    b = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Addition Result is:{0}", client.add(a, b));
                    Console.WriteLine("Subtraction Result is:{0}", client.sub(a, b));
                    Console.WriteLine("Multiplication Result is:{0}", client.mul(a, b));
                    Console.WriteLine("Divide Result is:{0}", client.div(a, b));
                }
                catch (Exception ex) {
                    Console.WriteLine("In Calculator {0}",ex.Message);
                
                }
                try
                {
                    Console.WriteLine("Enter Your Name");
                    name = Console.ReadLine();
                    Console.WriteLine("Enter Your Age");
                    age = Convert.ToInt32(Console.ReadLine());
                    info = new Info();
                    info.myName = name;
                    info.myAge = age;
                    Info myinfo=client.GetInfo(info);
                    Console.WriteLine("Your Name is {0} and age is {1}",myinfo.myName,myinfo.myAge);
                }
                catch (Exception ex) {
                    Console.WriteLine("In Info,,{0}",ex.Message);
                }
               
                Console.WriteLine("\nPress <Enter> to terminate the client.");
                Console.ReadLine();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }*/
        }
    }
}
