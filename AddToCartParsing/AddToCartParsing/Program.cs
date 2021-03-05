using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace AddToCartParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            CartDetails();
        
        }
        static void CartDetails() {
            String offerType = String.Empty;
            Int32 quantity = 0;
            Dictionary<string, string> details = new Dictionary<string, string>();
            string jsonString = "{\"results\":[{\"offerID\":\"850639192\",\"productID\":\"850639106\",\"offerGroupID\":\"850639193\",\"success\":true}],\"cart\":{\"cartID\":1008480,\"expiration\":1614756113647,\"subTotal\":6750,\"feeTotal\":1555,\"donationTotal\":0,\"grandTotal\":8305,\"selections\":[{\"category\":\"ADMISSIONS\",\"isUpsell\":false,\"offerType\":\"STANDARD\",\"offerID\":\"850639192\",\"offerGroupID\":\"850639193\",\"productID\":\"850639106\",\"eventID\":\"2796\",\"types\":[{\"priceTypeID\":\"45068\",\"priceLevelID\":\"3964\",\"label\":\"Regular 2\",\"unitPrice\":6750,\"quantity\":1,\"priceTotal\":6750,\"items\":[{\"sectionID\":\"21\",\"sectionLabel\":\"GAFLOOR\",\"rowID\":\"21\",\"rowLabel\":\"GA\",\"seatID\":\"432\",\"seatLabel\":\"GA\",\"info1\":\"\",\"info2\":\"\",\"deliveryMethodId\":0,\"assignedCustomerId\":0,\"neighborhoodPrintDescription\":\"Admissions\",\"sectionPrintDescription\":\"GA FLOOR\"}],\"preTaxUnitPrice\":6750,\"taxes\":[],\"fullPriceCodeInfo\":{\"unitPrice\":null,\"label\":null}}],\"quantity\":1,\"priceTotal\":6750,\"deliveryMethodID\":\"3621\"}],\"fees\":[{\"id\":\"850936929\",\"label\":\"Convenience Fee\",\"unitPrice\":1555,\"quantity\":1,\"total\":1555,\"taxes\":[],\"preTaxUnitPrice\":0}],\"taxes\":[],\"paymentMethods\":[{\"id\":\"101\",\"code\":\"VI\",\"token\":null,\"type\":\"CreditCard\"},{\"id\":\"102\",\"code\":\"MC\",\"token\":null,\"type\":\"CreditCard\"},{\"id\":\"100\",\"code\":\"AX\",\"token\":null,\"type\":\"CreditCard\"},{\"id\":\"103\",\"code\":\"DI\",\"token\":null,\"type\":\"CreditCard\"},{\"id\":\"2006\",\"code\":\"PayPal\",\"token\":\"eyJ2ZXJzaW9uIjoyLCJhdXRob3JpemF0aW9uRmluZ2VycHJpbnQiOiJleUowZVhBaU9pSktWMVFpTENKaGJHY2lPaUpGVXpJMU5pSXNJbXRwWkNJNklqSXdNVGd3TkRJMk1UWXRjSEp2WkhWamRHbHZiaUlzSW1semN5STZJbWgwZEhCek9pOHZZWEJwTG1KeVlXbHVkSEpsWldkaGRHVjNZWGt1WTI5dEluMC5leUpsZUhBaU9qRTJNVFE0TkRJeU1UUXNJbXAwYVNJNklqRTFPR0V5TWpoaExUUXdOVFF0TkRKbFppMWhOekV3TFdabU5qVXpOamt5TVRBMVpDSXNJbk4xWWlJNkltWmtlalI0TmpRMmRtTm9lblJxTW5RaUxDSnBjM01pT2lKb2RIUndjem92TDJGd2FTNWljbUZwYm5SeVpXVm5ZWFJsZDJGNUxtTnZiU0lzSW0xbGNtTm9ZVzUwSWpwN0luQjFZbXhwWTE5cFpDSTZJbVprZWpSNE5qUTJkbU5vZW5ScU1uUWlMQ0oyWlhKcFpubGZZMkZ5WkY5aWVWOWtaV1poZFd4MElqcG1ZV3h6Wlgwc0luSnBaMmgwY3lJNld5SnRZVzVoWjJWZmRtRjFiSFFpWFN3aWMyTnZjR1VpT2xzaVFuSmhhVzUwY21WbE9sWmhkV3gwSWwwc0ltOXdkR2x2Ym5NaU9udDlmUS44bmtJbnByR2ZKZDlBNkZGbnQyVzJGdTEzanlTYlRyZzhXOGZEdTUxQndSOUtqSjVNSHV0Tk1xdGZDd2E1bXl3YlJ0eVNreHBydW52aUVtblVkaWtnZyIsImNvbmZpZ1VybCI6Imh0dHBzOi8vYXBpLmJyYWludHJlZWdhdGV3YXkuY29tOjQ0My9tZXJjaGFudHMvZmR6NHg2NDZ2Y2h6dGoydC9jbGllbnRfYXBpL3YxL2NvbmZpZ3VyYXRpb24iLCJncmFwaFFMIjp7InVybCI6Imh0dHBzOi8vcGF5bWVudHMuYnJhaW50cmVlLWFwaS5jb20vZ3JhcGhxbCIsImRhdGUiOiIyMDE4LTA1LTA4IiwiZmVhdHVyZXMiOlsidG9rZW5pemVfY3JlZGl0X2NhcmRzIl19LCJjbGllbnRBcGlVcmwiOiJodHRwczovL2FwaS5icmFpbnRyZWVnYXRld2F5LmNvbTo0NDMvbWVyY2hhbnRzL2ZkejR4NjQ2dmNoenRqMnQvY2xpZW50X2FwaSIsImVudmlyb25tZW50IjoicHJvZHVjdGlvbiIsIm1lcmNoYW50SWQiOiJmZHo0eDY0NnZjaHp0ajJ0IiwiYXNzZXRzVXJsIjoiaHR0cHM6Ly9hc3NldHMuYnJhaW50cmVlZ2F0ZXdheS5jb20iLCJhdXRoVXJsIjoiaHR0cHM6Ly9hdXRoLnZlbm1vLmNvbSIsInZlbm1vIjoib2ZmIiwiY2hhbGxlbmdlcyI6W10sInRocmVlRFNlY3VyZUVuYWJsZWQiOmZhbHNlLCJhbmFseXRpY3MiOnsidXJsIjoiaHR0cHM6Ly9jbGllbnQtYW5hbHl0aWNzLmJyYWludHJlZWdhdGV3YXkuY29tL2ZkejR4NjQ2dmNoenRqMnQifSwicGF5cGFsRW5hYmxlZCI6dHJ1ZSwicGF5cGFsIjp7ImJpbGxpbmdBZ3JlZW1lbnRzRW5hYmxlZCI6dHJ1ZSwiZW52aXJvbm1lbnROb05ldHdvcmsiOmZhbHNlLCJ1bnZldHRlZE1lcmNoYW50IjpmYWxzZSwiYWxsb3dIdHRwIjpmYWxzZSwiZGlzcGxheU5hbWUiOiJBWFMgR3JvdXAgTExDIiwiY2xpZW50SWQiOiJBVFVXYkVwOFNWR0hzdGFrZ3g5UERjY25vYjJ5eHNvSkdCM3lGM2RMWWZGVGduc0N4Q1psUFVxV2JESGlSQmV0V195Ql9EQUNQZzhYSFFqNyIsInByaXZhY3lVcmwiOiJodHRwczovL3d3dy5heHMuY29tL2Fib3V0LXByaXZhY3ktcG9saWN5X1VTX3YyLmh0bWwiLCJ1c2VyQWdyZWVtZW50VXJsIjoiaHR0cHM6Ly93d3cuYXhzLmNvbS9hYm91dC10ZXJtcy1vZi11c2VfVVNfdjEuaHRtbCIsImJhc2VVcmwiOiJodHRwczovL2Fzc2V0cy5icmFpbnRyZWVnYXRld2F5LmNvbSIsImFzc2V0c1VybCI6Imh0dHBzOi8vY2hlY2tvdXQucGF5cGFsLmNvbSIsImRpcmVjdEJhc2VVcmwiOm51bGwsImVudmlyb25tZW50IjoibGl2ZSIsImJyYWludHJlZUNsaWVudElkIjoiQVJLcllSRGgzQUdYRHpXN3NPXzNiU2txLVUxQzdIR191V05DLXo1N0xqWVNETlVPU2FPdElhOXE2VnBXIiwibWVyY2hhbnRBY2NvdW50SWQiOiJwcF9heHNfZGVmYXVsdCIsImN1cnJlbmN5SXNvQ29kZSI6IlVTRCJ9fQ==\",\"type\":\"PayPal\"}],\"salesTaxTotal\":null,\"taxCompProfiles\":[],\"deliveryMethods\":{\"850639192\":[{\"id\":\"3621\",\"description\":\"AXS Mobile ID - Free (Recommended)\",\"process\":\"FlashSeats\",\"specialInstructions\":\"<!DOCTYPE HTML PUBLIC -//W3C//DTD HTML 4.01 Transitional//EN http://www.w3.org/TR/html4/loose.dtd><p>\n\t<strong>AXS Mobile ID</strong><br />\n\tShow your AXS Mobile ID in the AXS app then get it scanned to enter. Your AXS Mobile ID is the secure and unique code for all of your tickets, and all you need is the AXS app to use it. No paper tickets required.<br />\n\t&nbsp;<br />\n\tTo use your tickets:<br />\n\t1 &ndash; Download the AXS Mobile App <strong>(</strong><a href=https://itunes.apple.com/us/app/axs/id679805463 target=_blank><strong>iOS&nbsp;</strong></a><strong>&nbsp;or&nbsp;</strong><a href=https://play.google.com/store/apps/details?id=com.axs.android&amp;hl=en target=_blank><strong>Android</strong></a><strong>).</strong><br />\n\t2 &ndash; Open the app and sign in to view your AXS Mobile ID and ticket info.<br />\n\t3 &ndash; Show your AXS Mobile ID in the app at the door to scan and enter.<br />\n\t<br />\n\tBuying tickets for a group? Make sure everyone enters together or, for events with transfer enabled,&nbsp;use the app to transfer tickets to everyone&nbsp;before the event.</p>\n<p>\n\t<strong>NOTE: Ticket delivery or transfer may be delayed or disabled for some events.</strong></p>\n\"},{\"id\":\"7242\",\"description\":\"Standard Mail - $15.00\",\"process\":\"StandardShipping\",\"specialInstructions\":\"<!DOCTYPE HTML PUBLIC -//W3C//DTD HTML 4.01 Transitional//EN http://www.w3.org/TR/html4/loose.dtd><p>\n\t<strong>STANDARD MAIL: </strong>On the next business day after your purchase, your tickets will be sent to you via USPS (United States Postal Service) and will arrive in 5-10 business days. For assistance please visit <a href=http://support.axs.com/>http://support.axs.com</a>.</p>\n\"},{\"id\":\"3624\",\"description\":\"Will Call - $6.00\",\"process\":\"WillCall\",\"specialInstructions\":\"<!DOCTYPE HTML PUBLIC -//W3C//DTD HTML 4.01 Transitional//EN http://www.w3.org/TR/html4/loose.dtd><p>\n\t<span><strong><span><strong><span><strong><span><strong><span><strong>WILL CALL: </strong></span></strong></span></strong></span></strong></span></strong><span><span><span><span>Please present a valid photo ID, confirmation number/email, and the credit card used for purchase. For assistance please visit <a href=http://support.axs.com>http://support.axs.com</a>.</span></span></span></span></span></p>\n<p>\n\t&nbsp;</p>\n\"}]},\"isEMV\":false,\"donations\":[]},\"message\":\"New cart successfully created.\",\"status\":{\"status\":201,\"message\":\"Created\"},\"sessionExpireAt\":1614757613647}";
            JObject obj1 = JObject.Parse(jsonString);

            //   JArray cart = JArray.Parse(obj1["cart"].ToString());
            JObject obj2 = JObject.Parse(obj1["cart"].ToString());
            Double total_price = Convert.ToDouble(obj2["grandTotal"]) / 100;
            details.Add("Total_Price", Convert.ToString(total_price));
            if (obj2.Property("selections") != null)
            {
                JArray sel = JArray.Parse(obj2["selections"].ToString());
                obj2 = JObject.Parse(sel[0].ToString());
                //   quantity=Convert.ToInt32();
                details.Add("Qunatity", Convert.ToString(obj2["types"][0]["quantity"]));
                details.Add("offerType", Convert.ToString(obj2["offerType"]));
                offerType = Convert.ToString(obj2["offerType"]);
                details.Add("Section", Convert.ToString(obj2["types"][0]["items"][0]["sectionLabel"]));
                details.Add("Row", Convert.ToString(obj2["types"][0]["items"][0]["rowID"]));
                details.Add("Seat", Convert.ToString(obj2["types"][0]["items"][0]["seatID"]));
                details.Add("Admission", Convert.ToString(obj2["types"][0]["items"][0]["seatLabel"]));

            }
            foreach (KeyValuePair<String, String> item in details) {
                Console.WriteLine(item.Key+"="+item.Value);
            }
            

        }
    }
}
