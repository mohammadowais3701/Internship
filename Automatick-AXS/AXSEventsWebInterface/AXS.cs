using Nancy;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AXSEventsWebInterface
{
    public class AXS : NancyModule
    {
        public AXS()
            : base("/events")
        {
            Get["/"] = parameters =>
            {
                return View["events.html"];
            };

            Post["/add"] = parameters =>
            {
                String url = System.Web.HttpUtility.UrlDecode(Request.Form.url);
                String password = System.Web.HttpUtility.UrlDecode(Request.Form.password);

                String result = String.Empty;
                HttpStatusCode status = HttpStatusCode.BadRequest;

                if (!String.IsNullOrEmpty(url))
                {
                    AXSUtils.PostUrlToService(url, password);
                    
                    result = "Received : " + url;
                    status = HttpStatusCode.Accepted;                    
                }
                else
                {
                    result = "Error";
                    status = HttpStatusCode.BadRequest;
                }

                return new TextResponse(status, result);
            };  
        }

    }
}
