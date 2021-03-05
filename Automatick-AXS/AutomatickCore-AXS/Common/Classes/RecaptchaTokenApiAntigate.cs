using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPClient;
using CaptchaFactory;

namespace Automatick.Core
{
    public class RecaptchaTokenApiAntigate
    {
        private const string APIHost = "api.anti-captcha.com";

        public String GetRecaptchaToken(string keyAntigate, string captchakey, string host, ClientProxy proxy)
        {
            string recaptchaToken = string.Empty;

            try
            {
                var task = AnticaptchaApiWrapper.CreateNoCaptchaTask(APIHost, keyAntigate, "http://" + host,
              captchakey,
              AnticaptchaApiWrapper.ProxyType.http,
              proxy.Host,
              Convert.ToInt32(proxy.Port),
              proxy.Username,
               proxy.Password,
              "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36"
              );

                recaptchaToken = PollRequest(host, keyAntigate, task);

                return recaptchaToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return string.Empty;
        }
        public String GetHCaptchaToken(string keyAntigate, string captchakey, string host)
        {
            string recaptchaToken = string.Empty;

            try
            {
                var task = AnticaptchaApiWrapper.CreateHCaptchaTaskProxyless(APIHost, keyAntigate, "http://" + host,
                captchakey,
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36"
                );

                recaptchaToken = PollRequest(host, keyAntigate, task);

                return recaptchaToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return string.Empty;
        }
        public String GetRecaptchaTokenProxyLess(string keyAntigate, string captchakey, string host)
        {
            string recaptchaToken = string.Empty;

            try
            {
                var task = AnticaptchaApiWrapper.CreateNoCaptchaTaskProxyless(APIHost, keyAntigate, "http://" + host,
              captchakey,
              "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36"
              );

                recaptchaToken = PollRequest(host, keyAntigate, task);

                return recaptchaToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return string.Empty;
        }

        private String PollRequest(string host, string clientKey, AnticaptchaTask task)
        {
            AnticaptchaResult response;

            do
            {
                response = AnticaptchaApiWrapper.GetTaskResult(APIHost, clientKey, task);

                if (response.GetStatus().Equals(AnticaptchaResult.Status.ready))
                {
                    break;
                }

                Debug.WriteLine("Not done yet, waiting...");
                Thread.Sleep(1000);

            } while (response != null && response.GetStatus().Equals(AnticaptchaResult.Status.processing));

            if (response == null || response.GetSolution() == null)
            {
                Debug.WriteLine("Unknown error occurred...");
                Debug.WriteLine("Response dump:");
                Debug.WriteLine(response);
            }
            else
            {
                Debug.WriteLine("The answer is '" + response.GetSolution() + "'");
            }

            return response.GetSolution();
        }

    }
}
