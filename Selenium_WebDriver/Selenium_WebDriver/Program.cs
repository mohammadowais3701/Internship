using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;


namespace Selenium_WebDriver
{
    class Program
    {
        static void Main(string[] args)
        {
           //new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            try
            {
                var chromeOptions = new ChromeOptions();
                //Create a new proxy object
                var proxy = new Proxy();
                //Set the http proxy value, host and port.
                //proxy.HttpProxy = "91.108.192.190:5432";
                
               // proxy.SocksUserName = "raheel";
              
                proxy.Kind = ProxyKind.Manual;
              //  proxy.IsAutoDetect = false;
         //        proxy.HttpProxy = "91.108.192.190:5432";
                proxy.SslProxy = "91.108.192.190:5432";
                chromeOptions.Proxy = proxy;
                chromeOptions.AddArgument("ignore-certificate-errors");
              chromeOptions.AddArguments("--proxy-server=https://raheel:sFD89zfsvw@91.108.192.190:5432");
                //proxy.SocksPassword = "sFD89zfsvw";
                //Set the proxy to the Chrome options
              
               // chromeOptions.Proxy = proxy;
                IWebDriver driver = new ChromeDriver("E:\\Work",chromeOptions);
              //   

                 driver.Navigate().GoToUrl("https://www.linkedin.com/login/");
              //   driver.Navigate().GoToUrl("http://raheel:sFD89zfsvw@icanhazip.com/");
                Thread.Sleep(3000);
                IWebElement ele = driver.FindElement(By.Id("username"));  
                ele.SendKeys("mohammadowais3701@gmail.com");
                ele = driver.FindElement(By.Id("password"));
                ele.SendKeys("regex123@");
                ele = driver.FindElement(By.ClassName("login__form_action_container"));
                ele.Click();
                

            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
            
        }
    }
}
