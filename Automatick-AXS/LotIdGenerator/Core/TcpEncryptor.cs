﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LotIdGenerator
{
    public sealed class TCPEncryptor
    {
        public static String Encrypt(String textToEncrypt)
        {
            String encryptedText = String.Empty;

            try
            {
                String datetime = DateTime.Now.ToString("hh:mm:ss:fffff^dd-MM-yyyy");
                encryptedText = Encrypt(datetime + "^" + textToEncrypt, genKey());
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                //throw e;
            }

            return encryptedText;
        }

        public static String Decrypt(String textToDecrypt)
        {
            String decryptedText = String.Empty;

            try
            {
                decryptedText = Decrypt(textToDecrypt, genKey());
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

        private static string Encrypt(string strText, string strEncrypt)
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

        private static string Decrypt(string strText, string key)
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

        private static String genKey()
        {
            return "85secretkey";
        }
    }
}
