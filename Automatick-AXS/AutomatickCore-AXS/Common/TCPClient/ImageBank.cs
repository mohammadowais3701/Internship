using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    static class ImageBank
    {
        private static readonly Dictionary<string, string> imageBank = new Dictionary<string, string>
        {
            {"/m/0jbk", "animals"}, {"/m/0bt9lr", "dogs"}, {"/m/0gy7v", "guinea pigs"}, {"/m/01yrx", "cats"}, {"/m/05s2s", "plants"}, {"/m/06m11", "roses"}, {"/m/06m11-red", "red roses"}, {"/m/06m11-pink", "pink roses"}, {"/m/0gqbt", "shrubs"}, {"/m/01645p", "avocados"}, {"/m/025_v", "cactuses"}, {"/m/044plb", "barrel cactuses"}, {"/m/0m5w_", "saguaro cactuses"}, {"/m/025rm5", "prickly pear cactuses"}, {"/m/011s", "food or drink"}, {"/m/05yxcqj", "food"}, {"/m/0fszt", "cake"}, {"/m/03p1r4", "cupcakes"}, 
            {"/m/022p83", "wedding cakes"}, {"/m/02czv8", "birthday cakes"}, {"/m/09728", "bread"},{"/m/0l515", "sandwiches"}, {"/m/0cdn1", "hamburgers"}, {"/m/01j3zr", "burrito"}, {"/m/07pbfj", "fish"}, {"/m/0cxn2", "ice cream"}, {"/m/05z55", "pasta"}, {"/m/0grtl", "steak"}, {"/m/0663v", "pizza"}, {"/m/01z1m1x", "soup"}, {"/m/07030", "sushi"}, {"/m/09759", "rice dish"}, {"/m/01xs0yg", "drink"}, {"/m/01599", "beer"}, {"/m/081qc", "wine"}, {"/m/02vqfm", "coffee"}, {"/m/02wbm", "food"},{"/m/0grw1","salad"},
            {"/m/0pmbh","dim sum"},{"/m/07clx","tea"},{"/m/01z1kdw","juice"},{"/m/021mn","cookie"},{"/m/0271t","drinks"},
            {"/m/09d_r","mountains"},{"/m/03ktm1","bodies of water"},{"/m/06cnp","rivers"},{"/m/0b3yr","beaches"},{"/m/06m_p","the Sun"},{"/m/04wv_","the Moon"},
            {"/m/01bqvp","the sky"},{"/m/0c9ph5","flowers"},{"/m/07j7r","trees"} 
        };

        public static string SelectImagefromBank(string imageId)
        {
            try
            {
                KeyValuePair<String, String> selectedImage = imageBank.FirstOrDefault(p => p.Key.Contains(imageId));

                if (!String.IsNullOrEmpty(selectedImage.Value))
                {
                    return selectedImage.Value;
                }
                else
                {
                    return imageId;
                }

            }
            catch
            {
                return imageId;
            }
        }

        public static string SelectImagefromBank(string imageId, string url, Proxy proxy)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);

                if (proxy != null)
                    request.Proxy = proxy.toWebProxy();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                WebResponse response = request.GetResponse();
                string responseFromServer = string.Empty;

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }

                string getImageArraylist = responseFromServer;

                try
                {
                    getImageArraylist = getImageArraylist.Remove(0, getImageArraylist.IndexOf("this.type=\"beforeaction\"") + "this.type=\"beforeaction\"".Length);
                    getImageArraylist = getImageArraylist.Substring(0, getImageArraylist.IndexOf("id=\"rc-imageselect\"") + "id=\"rc-imageselect\"".Length);
                    getImageArraylist = getImageArraylist.Substring(getImageArraylist.IndexOf("{") + 1, (getImageArraylist.LastIndexOf("}") - 1) - getImageArraylist.IndexOf("{"));

                    string[] imageArraylist = getImageArraylist.Split(',');
                    if (imageArraylist.FirstOrDefault(p => p.Contains(imageId)) != null)
                    {
                        string imagetext = imageArraylist.FirstOrDefault(p => p.Contains(imageId));
                        return imagetext = imagetext.Split(':')[1].Replace("\"", "");
                    }
                    else
                    {
                        //-- extracted multiple cases
                        string getSwitchcaseslist = responseFromServer;
                        getSwitchcaseslist = getSwitchcaseslist.Remove(0, getSwitchcaseslist.IndexOf("id=\"rc-imageselect\"") + "id=\"rc-imageselect\"".Length);

                        if (getSwitchcaseslist.Contains("rc-imageselect-candidate"))
                        {
                            getSwitchcaseslist = getSwitchcaseslist.Substring(0, getSwitchcaseslist.IndexOf("class=\"rc-imageselect-clear\"") + "class=\"rc-imageselect-clear\"".Length);
                        }
                        else
                        {
                            getSwitchcaseslist = getSwitchcaseslist.Substring(0, getSwitchcaseslist.IndexOf("id=\"rc-imageselect-candidate\"") + "id=\"rc-imageselect-candidate\"".Length);
                        }
                        getSwitchcaseslist = getSwitchcaseslist.Substring(getSwitchcaseslist.IndexOf("{") + 1, (getSwitchcaseslist.LastIndexOf("}") - 1) - getSwitchcaseslist.IndexOf("{"));
                        string[] casearray = getSwitchcaseslist.Split(';');

                        List<String> arraylist = new List<string>();

                        foreach (string item in casearray)
                        {
                            if (item.Contains("case"))
                            {

                                string id = string.Empty;
                                string value = string.Empty;

                                try
                                {
                                    if (item.Contains("switch"))
                                    {
                                        string removeit = item.Substring(0, item.IndexOf("{"));
                                        string temp = item.Replace(removeit, "");
                                        id = temp.Split(':')[0].Remove(0, temp.Split(':')[0].IndexOf("case"));

                                        try
                                        {
                                            value = (temp.Split(':')[1]).Substring((temp.Split(':')[1]).IndexOf('>') + 1, temp.Split(':')[1].LastIndexOf('<') - ((temp.Split(':')[1]).IndexOf('>') + 1));

                                        }
                                        catch
                                        {
                                            value = temp.Split(':')[1].Replace("c+=", "").Replace("\"", "").Trim();
                                        }
                                    }
                                    else
                                    {

                                        id = item.Split(':')[0].Remove(0, item.Split(':')[0].IndexOf("case"));

                                        try
                                        {
                                            value = (item.Split(':')[1]).Substring((item.Split(':')[1]).IndexOf('>') + 1, item.Split(':')[1].LastIndexOf('<') - ((item.Split(':')[1]).IndexOf('>') + 1));

                                        }
                                        catch
                                        {
                                            value = item.Split(':')[1].Replace("c+=", "").Replace("\"", "").Trim();
                                        }
                                    }
                                }
                                catch { }

                                id = id.Replace("case", "").Replace("\"", "").Trim();
                                arraylist.Add(id + "," + value);
                            }
                        }

                        string imagetext = arraylist.FirstOrDefault(p => p.Contains(imageId));
                        return imagetext = imagetext.Split(',')[1].Replace("\"", "");
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return imageId;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return imageId;
            }

        }

        static int index = 0;
    }
}
