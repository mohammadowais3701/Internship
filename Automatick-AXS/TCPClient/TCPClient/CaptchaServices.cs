using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    [Serializable]
    public class CaptchaServices
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Host { get; set; }
        public String Port { get; set; }
        public String CaptchaService { get; set; }

        public CaptchaServices()
        { }

        public CaptchaServices(String _captchaService, String _userName, String _password, String _host, String _port)
        {
            this.CaptchaService = _captchaService;
            this.UserName = _userName;
            this.Password = _password;
            this.Port = _port;
        }
    }
}
