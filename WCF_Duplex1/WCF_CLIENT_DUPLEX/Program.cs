using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using WCF_Duplex1;
using System.IO;

namespace WCF_CLIENT_DUPLEX
{
    class Program
    {
        public class HelloWorldServiceCallback : IHelloWorldServiceCallback
        {
            public void greetings(string message)
            {
                Console.WriteLine(message);
            }
        }
        static void Main(string[] args)
        {
            try
            {
                EndpointAddress endpointAddress = new EndpointAddress("net.pipe://localhost/WCF_Duplex1/Service1");
                var callback = new HelloWorldServiceCallback();
                var context = new InstanceContext(callback);
                var binding = new NetNamedPipeBinding();
                var factory = new DuplexChannelFactory<IService1>(context, binding, endpointAddress);
                var service = factory.CreateChannel();
                service.Registered();
                string textFile = "E:\\textFile.txt";
          if (File.Exists(textFile))
                {
                    // Read a text file line by line.  
                    string[] lines = File.ReadAllLines(textFile);
                    while (true)
                    {
                        service.ReceiveFile(lines);
                        Thread.Sleep(2000);

                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
