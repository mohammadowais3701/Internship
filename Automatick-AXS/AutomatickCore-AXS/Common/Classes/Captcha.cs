using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.IO;

namespace Automatick.Core
{
    [Serializable]
    public class Captcha
    {
        public AutoResetEvent captchaentered = new AutoResetEvent(false);
        Bitmap _CaptchaImage;
        String _captchaWords;
        private string _imageUrl = String.Empty;
        private string _ques = String.Empty;
        public String CaptchaWords
        {
            get { return _captchaWords; }
            set { _captchaWords = value; }
        }
        public Bitmap CaptchaImage
        {
            get {
                try
                {
                    if (this._CaptchaImage == null)
                    {
                        using (MemoryStream ms = new MemoryStream(this.CaptchesBytes))
                        {
                            _CaptchaImage = new Bitmap(ms);
                        }
                    }
                }
                catch
                {
                    _CaptchaImage = null;
                }

                return _CaptchaImage;             
            }
            set { _CaptchaImage = value; }
        }

        public byte[] CaptchesBytes
        {
            get;
            set;
        }

        public Captcha(byte[] captchesBytes)
        {
            this.CaptchesBytes = captchesBytes;
            this._captchaWords = String.Empty;
            this._CaptchaImage = null;
        }

        public String CValue
        {
            get;
            set;
        }

        public String ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public String Question
        {
            get { return _ques; }
            set { _ques = value; }
        }
    }
}
