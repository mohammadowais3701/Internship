using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace LotGenerator_Core
{
    [ServiceContract]
    public interface ILotMainService
    {
        [OperationContract]
        [WebGet]
        Stream getEvent();

        [OperationContract]
        [WebGet]
        Stream UpdateLotID(String lotID, String appPrefix);

        [OperationContract]
        [WebGet]
        Stream GetConfig();

        [OperationContract]
        [WebGet]
        Stream PostURL();

        [OperationContract]
        //[WebGet(UriTemplate = "PostURLToBrowser?url={url}")]
        [WebInvoke(Method = "POST")]//, UriTemplate = "PostURLToBrowser")]
        Stream PostURLToBrowser(String url);
    }
}
