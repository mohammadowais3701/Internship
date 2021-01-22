using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using WCF_BASICHTTP;

namespace WCFHOST1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Create a URI to serve as the base address.
            Uri baseAddress = new Uri("http://10.0.0.209:8735/");
            // Step 2: Create a ServiceHost instance.
            WebServiceHost selfHost = new WebServiceHost(typeof(Service1), baseAddress);
           
            try
            {
                // Step 3: Add a service endpoint.
                 selfHost.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(WebHttpSecurityMode.None), "");
                // Step 4: Enable metadata exchange.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
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
