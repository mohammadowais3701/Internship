namespace LoadBalancer
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class LBRequestMessage
    {
        public String Command
        {
            get;
            set;
        }
    }

    public class LBResponseMessage
    {
        public String Server { get; set; }
    }

    public class IPPort
    {
        string ip;
        int port;

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
    }

    public sealed class TCPEncryptor
    {
        public static String LBEncrypt(String textToEncrypt)
        {
            String encryptedText = String.Empty;

            try
            {
                String datetime = DateTime.Now.ToString("hh:mm:ss:fffff^dd-MM-yyyy");
                encryptedText = LBEncrypt(datetime + "^" + textToEncrypt, lbGenKey());
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                //throw e;
            }

            return encryptedText;
        }

        public static String LBDecrypt(String textToDecrypt)
        {
            String decryptedText = String.Empty;

            try
            {
                decryptedText = LBDecrypt(textToDecrypt, lbGenKey());
                String[] components = decryptedText.Split('^');

                if (components != null)
                {
                    if (components.Length == 3)
                    {
                        decryptedText = components[2];
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                //throw e;
            }

            return decryptedText;
        }

        private static string LBEncrypt(string strText, string strEncrypt)
        {
            byte[] byKey = new byte[20];
            byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputArray = System.Text.Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, dv), CryptoStreamMode.Write);
                cs.Write(inputArray, 0, inputArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string LBDecrypt(string strText, string key)
        {

            byte[] bKey = new byte[20];

            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            try
            {
                bKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                Byte[] inputByteArray = inputByteArray = Convert.FromBase64String(strText);

                MemoryStream ms = new MemoryStream();

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bKey, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                System.Text.Encoding encoding = System.Text.Encoding.UTF8;

                return encoding.GetString(ms.ToArray());
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static String lbGenKey()
        {
            return "lbsecretkey";
        }
    }
}