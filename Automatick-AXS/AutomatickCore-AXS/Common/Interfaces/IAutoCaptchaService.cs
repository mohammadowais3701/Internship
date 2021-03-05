using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public interface IAutoCaptchaService
    {
        String CaptchaError
        {
            get;
            set;
        }

        AutoCaptchaServices AutoCaptchaServices
        {
            get;
            set;
        }

        Boolean solve();
        Boolean reportBadCaptcha();

        void abort();
    }
}
