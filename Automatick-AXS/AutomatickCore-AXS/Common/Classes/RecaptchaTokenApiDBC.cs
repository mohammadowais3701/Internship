using DeathByCaptcha;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class RecaptchaTokenApiDBC
    {
        public String GetRecaptchaToken(string username, string password, string captchakey, string host, string proxy, string proxyType)  //--- on request
        {
            string recaptchaToken = string.Empty;

            DeathByCaptcha.Client client = (Client)new HttpClient(username.Trim(), password.Trim());

            string tokenParams = GetTokenParams(proxy, proxyType, captchakey, "http://" + host);

            DeathByCaptcha.Captcha captcha = null;

            //while (captcha == null)
            {
                captcha = client.Decode(120,
                new Hashtable(){
                    { "type", 4 },
                    {"token_params", tokenParams}
                });
            }


            if (null != captcha)
            {
                // Poll for the CAPTCHA status until it's solved.
                // Wait at least a few seconds between poll or you'll get
                // banned as abuser.
                while (captcha.Uploaded && !captcha.Solved)
                {
                    System.Threading.Thread.Sleep(Client.DefaultPollInterval * 1000);
                    captcha = client.GetCaptcha(captcha.Id);
                }

                if (captcha.Solved)
                {
                    Console.WriteLine("CAPTCHA solved: {0}",
                                       captcha.Text);

                    // Report an incorrectly solved CAPTCHA.  Make sure the
                    // CAPTCHA was in fact incorrectly solved, do not just
                    // report them all or at random, or you might be banned
                    // as abuser.

                }
                else
                {
                    Console.WriteLine("CAPTCHA was not solved");
                }
            }
            else
            {
                Console.WriteLine("CAPTCHA was not uploaded");
            }


            return captcha.Text;
        }

        string GetTokenParams(string proxy, string proxyType, string googlekey, string pageurl)
        {
            return "{\"proxy\": \"" + proxy + "\"," +
                    "\"proxytype\": \"" + proxyType + "\"," +
                    "\"googlekey\": \"" + googlekey + "\"," +
                    "\"pageurl\": \"" + pageurl + "\"}";
        }
    }
}
