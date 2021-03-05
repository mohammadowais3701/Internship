using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCPClient;
using System.IO;
using Newtonsoft.Json;
using Automatick.Core;
using System.Diagnostics;
namespace TCPClient
{
    [Serializable]
    public class ValidationMessage
    {
        #region memebers
        private String command;   
        private int tokensCount;
        private String licenseCode;
        private String hardDiskSerial;
        private String processorID;
        private String appPrefix;
        private String servers;
        private int capsiumWorkersCount;
        private int specialServiesWorkersCount;

        private static ValidationMessage _tixToxInstance;
        private AutoCaptchaServices _autocaptchaservices = null;

        public String Command
        {
            get { return command; }
            set { command = value; }
        }

        public String LicenseCode
        {
            get { return licenseCode; }
            set { licenseCode = value; }
        }

        public String HardDiskSerial
        {
            get { return hardDiskSerial; }
            set { hardDiskSerial = value; }
        }

        public String ProcessorID
        {
            get { return processorID; }
            set { processorID = value; }
        }

        public String AppPrefix
        {
            get { return appPrefix; }
            set { appPrefix = value; }
        }

        public String Servers
        {
            get { return servers; }
            set { servers = value; }
        }
        public int CapsiumWorkersCount
        {
            get { return capsiumWorkersCount; }
            set { capsiumWorkersCount = value; }
        }

        public int SpecialServicesWorkerCount
        {
            get { return specialServiesWorkersCount; }
            set { specialServiesWorkersCount = value; }
        }

        public static ValidationMessage TixToxInstance
        {
            get
            {
                if (_tixToxInstance == null)
                {
                    throw new Exception("Proxy picker Object not created");
                }
                return _tixToxInstance;
            }
        }

        private List<ClientProxy> proxies;

        public List<ClientProxy> Proxies
        {
            get { return proxies; }
            set { proxies = value; }
        }
       
        public int TokensCount
        {
            get { return tokensCount; }
            set { tokensCount = value; }
        }
        public AutoCaptchaServices AutoCaptchaServices
        {
            get { return _autocaptchaservices; }

            set { _autocaptchaservices = value; }
        }
        #endregion

        #region constructors
        public ValidationMessage(LicenseCore lic)
        {
            this.Proxies = new List<ClientProxy>();
            this.licenseCode = lic.LicenseCode;
            this.HardDiskSerial = lic.HardDiskSerial;
            this.ProcessorID = lic.ProcessorID;
            this.AppPrefix = lic.AppPreFix;
        }
     
        #endregion

        #region methods

        public static void createTixToxSetting(LicenseCore lic)
        {
            _tixToxInstance = new ValidationMessage(lic);
        }


        public void save()
        {
            TCPEncryptor encryptor=new TCPEncryptor();
            try
            {
                String json = JsonConvert.SerializeObject(this);
             
                String fileName = Environment.CurrentDirectory + @"\TixToxSetting.txt";
                if (!File.Exists(fileName))
                {
                    //JsonConvert.SerializeObject(this);
                    json = TCPEncryptor.Encrypt(json);
                    System.IO.File.WriteAllText(fileName, json);
                }
                else
                {
                    File.Delete(fileName);
                  //  JsonConvert.SerializeObject(this);
                    json = TCPEncryptor.Encrypt(json);
                    System.IO.File.WriteAllText(fileName, json);
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public List<ClientProxy> ProxiesToClientProxy(List<Proxy> Proxies)
        {
            try
            {
                List<Proxy> templist = Proxies.Where(p => p.IfLuminatiProxy).ToList();  //-- remove luminati proxies

                this.Proxies = new List<ClientProxy>();

                foreach (Proxy p in templist)
                {
                    ClientProxy cp = new ClientProxy();
                    cp.Host = p.Address;
                    cp.Port = p.Port;
                    cp.Username = p.UserName;
                    cp.Password = p.Password;
                    proxies.Add(cp);
                }

                return proxies;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
