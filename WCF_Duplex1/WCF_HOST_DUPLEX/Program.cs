using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using WCF_Duplex1;


namespace WCF_HOST_DUPLEX
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("net.pipe://localhost/WCF_Duplex1/");
            // Step 2: Create a ServiceHost instance.
            ServiceHost selfHost = new ServiceHost(typeof(Service1), baseAddress);
            try
            {
                // Step 3: Add a service endpoint.
                selfHost.AddServiceEndpoint(typeof(IService1), new NetNamedPipeBinding(), "Service1");
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
