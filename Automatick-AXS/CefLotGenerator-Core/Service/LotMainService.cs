using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace LotGenerator_Core
{
    public class LotMainService : ILotMainService
    {
        public Stream getEvent()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            String result = String.Empty;

            ClientRequest request = null;

            try
            {
                bool dequeued = MainLotService.ClientRequests.TryDequeue(out request);

                if (dequeued)
                {
                    String proxy = String.Empty;
                    MainLotService.Proxies.TryDequeue(out proxy);

                    if (!String.IsNullOrEmpty(proxy))
                    {
                        MainLotService.Proxies.Enqueue(proxy);
                        request.Proxy = proxy;
                    }

                    //TopicsList.UsedContainer.Enqueue(result);
                    result = JsonConvert.SerializeObject(request);
                }
                else
                {
                    result = "Not available";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Sequence contains no matching element"))
                {
                    result = "Not available";
                }

                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (result == null)
                {
                    result = String.Empty;
                }
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }

        public Stream UpdateLotID(String lotID, String appPrefix)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            String result = "updated";

            try
            {
                MainLotService.Service.UpdateLotID(lotID, appPrefix);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Lot Updated: " + lotID);
            Console.ResetColor();
            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }

        public Stream GetConfig()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            String result = String.Empty;

            try
            {
                //If client will request in this format
                result = "browsers=" + MainLotService.BrowserCount;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }

        public Stream PostURL()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            String result = String.Empty;

            try
            {
                result = "<form action=\"http://" + MainLotService.LocalIPAddress() + ":9000/PostURLToBrowser\" method=\"POST\"><label>Enter URL: </label><br/><input type=\"text\" name=\"url\"><br/><input type=\"submit\" name=\"submit\"></form>";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }

        public Stream PostURLToBrowser(String url)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            String result = String.Empty;

            try
            {
                ClientRequest request = new ClientRequest();
                request.AppPrefix = "AAX";
                request.LicenseID = "System";
                request.URL = url;
                MainLotService.ClientRequests.Enqueue(request);
                result = "<html><head></head><body><p>Link Updated</p> </br><b><a href=\"http://" + MainLotService.LocalIPAddress() + ":9000/PostURL\">Add another Link</a></b></body></html>";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }
    }
}