using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WCFLib_TCP;

namespace WCFHOST1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Create a URI to serve as the base address.

            Uri baseAddress = new Uri("net.tcp://localhost:8523/WCFLib_TCP/Service1/");
            // Step 2: Create a ServiceHost instance.
            ServiceHost selfHost = new ServiceHost(typeof(Service1), baseAddress);
            try
            {
                // Step 3: Add a service endpoint.
                selfHost.AddServiceEndpoint(typeof(IService1), new NetTcpBinding(), "Service1");
                // Step 4: Enable metadata exchange.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = false;
                selfHost.Description.Behaviors.Add(smb);
                // Step 5: Start the service.
                selfHost.Open();
                Console.WriteLine("The service is ready.");
                // Close the ServiceHost to stop the service.
                Console.WriteLine("Press <Enter> to terminate the service.");
                Console.WriteLine();
                Console.ReadLine();
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}
