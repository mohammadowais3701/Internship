using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Automatick.Core
{
    [Serializable]
    public class AutoCaptchaServices
    {
        public String BPCKey
        {
            get;
            set;
        }

        public String DBCUserName
        {
            get;
            set;
        }

        public String DBCPassword
        {
            get;
            set;
        }

        public String RDUserName
        {
            get;
            set;
        }

        public String RDPassword
        {
            get;
            set;
        }

        public String CPTUserName
        {
            get;
            set;
        }

        public String CPTPassword
        {
            get;
            set;
        }

        public String RDCUserName
        {
            get;
            set;
        }

        public String RDCPassword
        {
            get;
            set;
        }

        public String NewRDUserName
        {
            get;
            set;
        }

        public String NewRDPassword
        {
            get;
            set;
        }

        public String NewCPTUserName
        {
            get;
            set;
        }

        public String NewCPTPassword
        {
            get;
            set;
        }

        public String DCUserName
        {
            get;
            set;
        }

        public String DCPassword
        {
            get;
            set;
        }

        public String DCPort
        {
            get;
            set;
        }


        public String OCRIP
        {
            get;
            set;
        }

        public String OCRPort
        {
            get;
            set;
        }
        public String SOCRIP
        {
            get;
            set;
        }

        public String SOCRPort
        {
            get;
            set;
        }

        public String SOCRCaptchaURL
        {
            get;
            set;
        }

        public String CUserName
        {
            get;
            set;
        }

        public String CPassword
        {
            get;
            set;
        }

        public String CHost
        {
            get;
            set;
        }

        public String CPort
        {
            get;
            set;
        }

        public String ROCRIP
        {
            get;
            set;
        }

        public String ROCRPort
        {
            get;
            set;
        }

        public String ROCRUsername
        {
            get;
            set;
        }

        public String ROCRPassword
        {
            get;
            set;
        }

        public String BOLOIP
        {
            get;
            set;
        }

        public String BOLOPORT
        {
            get;
            set;
        }

        public String C2Key
        {
            get;
            set;
        }

        public String AC1Key
        {
            get;
            set;
        }

        public String CTRUserName
        {
            get;
            set;
        }

        public String CTRPassword
        {
            get;
            set;
        }

        public String CTRIP
        {
            get;
            set;
        }

        public String CTRPort
        {
            get;
            set;
        }
        public String AntigateKey
        {
            get;
            set;
        }

        public Boolean ifAntigate
        {
            get;
            set;
        }

        public Boolean ifCaptchator
        {
            get;
            set;
        }


        public Boolean ifUseGoogle
        {
            get;
            set;
        }

        public Boolean ifUseRecaptcha
        {
            get;
            set;
        }

        public Boolean ifAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifBPCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifRDAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifRDCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifCPTAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifDBCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifDCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifOCR
        {
            get;
            set;
        }
        public Boolean ifROCR
        {
            get;
            set;
        }
        public Boolean ifBoloOCR
        {
            get;
            set;
        }
        public Boolean ifCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean if2CAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifCRAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifNoCaptchaOCRAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifAC1AutoCaptcha
        {
            get;
            set;
        }


        public void retrieveNewRDUserNameAndPassword()
        {
            Thread th = new Thread(new ParameterizedThreadStart(this.retrieveNewRDUserNameAndPasswordThread));
            th.Priority = ThreadPriority.Highest;
            th.SetApartmentState(ApartmentState.STA);
            th.IsBackground = true;
            th.Start(this);
        }

        public void retrieveNewRDUserNameAndPasswordThread(object autoCaptchaServicesObject)
        {
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 100;

                AutoCaptchaServices obj = (AutoCaptchaServices)autoCaptchaServicesObject;
                if (obj != null)
                {
                    if (!String.IsNullOrEmpty(obj.RDUserName) && !String.IsNullOrEmpty(obj.RDPassword))
                    {
                        String result = "";
                        RDCaptchaService rdCaptcha = new RDCaptchaService(this);
                        result = rdCaptcha.PostUsernameAndPasswordRD();

                        if (!String.IsNullOrEmpty(result))
                        {
                            string[] strtmp = result.Split(',');
                            if (strtmp.Length >= 2)
                            {
                                obj.NewRDUserName = strtmp[0];
                                obj.NewRDPassword = strtmp[1];
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void retrieveNewCPTUserNameAndPassword()
        {
            Thread th = new Thread(new ParameterizedThreadStart(this.retrieveNewCPTUserNameAndPasswordThread));
            th.Priority = ThreadPriority.Highest;
            th.SetApartmentState(ApartmentState.STA);
            th.IsBackground = true;
            th.Start(this);
        }

        public void retrieveNewCPTUserNameAndPasswordThread(object autoCaptchaServicesObject)
        {
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 100;

                AutoCaptchaServices obj = (AutoCaptchaServices)autoCaptchaServicesObject;
                if (obj != null)
                {
                    if (!String.IsNullOrEmpty(obj.CPTUserName) && !String.IsNullOrEmpty(obj.CPTPassword))
                    {
                        String result = "";
                        CPTCaptchaService cptCaptcha = new CPTCaptchaService(this);
                        result = cptCaptcha.PostUsernameAndPasswordCPT();

                        if (!String.IsNullOrEmpty(result))
                        {
                            string[] strtmp = result.Split(',');
                            if (strtmp.Length >= 2)
                            {
                                obj.NewCPTUserName = strtmp[0];
                                obj.NewCPTPassword = strtmp[1];
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void retrieveDCUserNameAndPasswordThread(object autoCaptchaServicesObject)
        {
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 100;

                AutoCaptchaServices obj = (AutoCaptchaServices)autoCaptchaServicesObject;
                if (obj != null)
                {
                    if (!String.IsNullOrEmpty(obj.DCUserName) && !String.IsNullOrEmpty(obj.DCPassword) && !String.IsNullOrEmpty(obj.DCPort))
                    {
                        DeCaptchaService cptCaptcha = new DeCaptchaService(this);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void retrieveCUserNameAndPasswordThread(object autoCaptchaServicesObject)
        {
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 100;

                AutoCaptchaServices obj = (AutoCaptchaServices)autoCaptchaServicesObject;
                if (obj != null)
                {
                    if (!String.IsNullOrEmpty(obj.CUserName) && !String.IsNullOrEmpty(obj.CPassword) && !String.IsNullOrEmpty(obj.CPort))
                    {
                        CustomCaptchaService cptCaptcha = new CustomCaptchaService(this);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public string BPCUserName { get; set; }

        public string BPCPassword { get; set; }
    }
}
