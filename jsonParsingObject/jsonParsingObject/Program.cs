using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace jsonParsingObject
{
    class Program
    {
        static void Main(string[] args)
        {
            string str="{\"offerPrices\":[{\"offerID\":\"55032577\",\"offerGroupID\":\"55032578\",\"zonePrices\":[{\"productID\":55032545,\"zoneID\":1,\"eventID\":\"2796\",\"priceLevels\":[{\"label\":\"General Admission\",\"priceLevelID\":\"3599\",\"availability\":{\"amount\":304,\"sections\":[]},\"prices\":[{\"base\":3700,\"priceTypeID\":\"45001\"}],\"order\":1}],\"priceTypes\":[{\"priceTypeID\":\"45001\",\"label\":\"Advance\",\"minQty\":0,\"maxQty\":0,\"pricingMode\":\"PriceChart\",\"offerGroupID\":\"55032578\",\"isSubtractFromFree\":false,\"sortOrder\":1}],\"rawDynamicPrices\":{},\"rawDynamicPriceRanges\":{},\"isSoldOut\":false}]}],\"currency\":\"USD\"}";
            JObject obj = JObject.Parse(str);


            var offerPricesObj = obj["offerPrices"].FirstOrDefault();
            string offerId = String.Empty;
            string offerGroupID=String.Empty;
          
            if (offerPricesObj != null)
            {
                 offerId = Convert.ToString(offerPricesObj["offerID"]);
                 offerGroupID=Convert.ToString(offerPricesObj["offerGroupID"]);   
                 //   qty=Convert.ToInt32(offerPricesObj["order"]);
                var zonePricesobj=offerPricesObj["zonePrices"].FirstOrDefault();
                string productID=String.Empty;

                 if (zonePricesobj!= null){
                  productID=Convert.ToString(zonePricesobj["productID"]);
                  var priceLevelsObj=zonePricesobj["priceLevels"].FirstOrDefault();
                      int qty;
                      if (priceLevelsObj != null)
                      {
                          qty = Convert.ToInt32(priceLevelsObj["order"]);
                          var priceObj = priceLevelsObj["prices"].FirstOrDefault();
                          string priceTypeID = String.Empty;
                          if (priceObj != null)
                          {
                              priceTypeID = Convert.ToString(priceObj["priceTypeID"]);

                          }
                          var availabilityObj = priceLevelsObj["availability"].FirstOrDefault();
                          String section = String.Empty;
                          if (availabilityObj != null)
                          {
                              try
                              {
                                  section = Convert.ToString(availabilityObj.Children().ToList()[1]);
                              }
                              catch (Exception ex)
                              {
                                  Console.WriteLine(ex.Message);
                              }
                        str="{\"offerId\":\""+offerId+"\",\"offerGroupID\":\""+offerGroupID+"\",\"productID\":\""+productID+"\",\"qty\":\""+qty+"\",\"priceTypeID\":\""+priceTypeID+"\",\"section\":\""+section+"\"}";
                        obj = JObject.Parse(str);
                          }
                      }
                 }

            }
      

       /*    str=Convert.ToString(obj["offerPrices"]);
           str = str.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "");
           str = "{" + str + "}";
           str = str.Replace(" ", "");

           str = str.Replace("\r\n", "");


          Regex reg = new Regex(@":\s");
           str = reg.Replace(str, ":,");
           reg = new Regex("^[^\"]");
           str = reg.Replace(str, "");
           reg = new Regex(",\\W,");
           str = reg.Replace(str, "");
           str = "{" + str ;
           Console.WriteLine(str.Replace(" ","").Trim());
           str = str.Replace("\"zonePrices\":", "").Replace("\"priceLevels\":","").Replace("\"availability\":","").Replace("\"sections\":,\"prices\":","").Replace("\"priceTypes\":","").Replace("\"rawDynamicPrices\":,\"rawDynamicPriceRanges\":,","");
           obj = JObject.Parse(str);
      //    obj = JObject.Parse(Convert.ToString(obj["offerPrices"][0]));
        //   obj = obj = JObject.Parse(Convert.ToString(obj["zonePrices"][0]));

            cartData obj1 = JsonConvert.DeserializeObject<cartData>(str);*/

            
        }
    }
    class cartData
    {
        public string offerID { get; set; }
        public string offerGroupID { get; set; }
        public string productID { get; set; }
        public string label { get; set; }
        public string priceLevelID { get; set; }
        public string priceTypeID { get; set; }
        public string currenncy { get; set; }


    }
}
