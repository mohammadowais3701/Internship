using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Automatick.Core
{
    class ImageSenderForBolo
    {
         WebProxy _proxy = null;
        public String Host { get; set; }
        public Int32 Port { get; set; }
        public ImageSenderForBolo(String host, Int32 port, WebProxy proxy)
        {
            _proxy = proxy;
            Host = host;
            Port = port;
        }

        public string getAnswer(string host, Int32 port, byte[] imgBytes, ref bool answered)
        {
            string answer = "";

            answered = false;
            TcpClient client = new TcpClient(host, port);
            NetworkStream stream = client.GetStream();

            //Send the command code 0x00
            byte[] cmd = new byte[1];
            cmd[0] = 0x00;
            stream.Write(cmd, 0, 1);

            //Send the Image Bytes length
            byte[] imgLenBytes = BitConverter.GetBytes(imgBytes.Length);
            stream.Write(imgLenBytes, 0, 4);

            //Send the Image Bytes
            stream.Write(imgBytes, 0, imgBytes.Length);

            //Now lets wait for the 4 bytes response code
            byte[] respWord = new byte[4];
            stream.Read(respWord, 0, 4);
            Int32 respCode = BitConverter.ToInt32(respWord, 0);
            if (respCode != 1)
            {
                return "";
            }

            //Get the Answer Length
            byte[] answerLenBytes = new byte[4];
            stream.Read(answerLenBytes, 0, 4);
            Int32 answerLen = BitConverter.ToInt32(answerLenBytes, 0);
            if (answerLen < 1 || answerLen > 50)
            {
                return "";
            }

            //Get the Answer
            byte[] answerBytes = new byte[answerLen];
            stream.Read(answerBytes, 0, answerLen);
            answer = System.Text.Encoding.UTF8.GetString(answerBytes);
            answered = true;

            stream.Close();
            client.Close();

            return answer;
        }

        // public TcpClient connectViaHTTPProxy(string targetHost,int targetPort,string httpProxyHost,int httpProxyPort, string proxyUserName,string proxyPassword)
        public TcpClient connectViaHTTPProxy(string targetHost, int targetPort, string httpProxyHost, int httpProxyPort)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttp,
                Host = httpProxyHost,
                Port = httpProxyPort
            };

            var proxyUri = uriBuilder.Uri;

            var request = WebRequest.Create(
                "http://" + targetHost + ":" + targetPort);

            var webProxy = new WebProxy(proxyUri);

            request.Proxy = webProxy;
            request.Method = "CONNECT";

            //var credentials = new NetworkCredential(
            //    proxyUserName, proxyPassword);

            //webProxy.Credentials = credentials;

            var response = request.GetResponse();

            var responseStream = response.GetResponseStream();
            Debug.Assert(responseStream != null);

            const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var rsType = responseStream.GetType();
            var connectionProperty = rsType.GetProperty("Connection", Flags);

            var connection = connectionProperty.GetValue(responseStream, null);
            var connectionType = connection.GetType();
            var networkStreamProperty = connectionType.GetProperty("NetworkStream", Flags);

            var networkStream = networkStreamProperty.GetValue(connection, null);
            var nsType = networkStream.GetType();
            var socketProperty = nsType.GetProperty("Socket", Flags);
            var socket = (Socket)socketProperty.GetValue(networkStream, null);

            return new TcpClient { Client = socket };
        }

        public String SendBytes(byte[] bytes, int timeout = 10000)
        {
            if (_proxy != null)
            {
                var tcpClient = connectViaHTTPProxy(Host, Port, _proxy.Address.Host, _proxy.Address.Port);
                return PerformWorking(tcpClient, bytes);
            }
            else
            {
                var tcpClient = new TcpClient();
                tcpClient.Connect(Host, Port);
                return PerformWorking(tcpClient, bytes);
            }
        }

        public String PerformWorking(TcpClient tcpClient, byte[] bytes)
        {
            try
            {
                var sendStream = tcpClient.GetStream();
                sendStream.WriteByte(0);

                var fileSize = bytes.Length;
                var hexStr = fileSize.ToString("X");
                while (hexStr.Length < 4)
                {
                    hexStr = "0" + hexStr;
                }

                byte[] byteSize = new byte[2];

                if (hexStr.Length >= 3)
                {
                    String chr1 = Convert.ToString(hexStr[2]);
                    if (hexStr.Length >= 4)
                    {
                        chr1 += Convert.ToString(hexStr[3]);
                    }
                    //sendStream.WriteByte((Byte)Int32.Parse(chr1, NumberStyles.HexNumber));
                    byteSize[0] = (Byte)Int32.Parse(chr1, NumberStyles.HexNumber);
                }
                else
                {
                    sendStream.WriteByte(0);
                    byteSize[0] = 0;
                }

                if (hexStr.Length >= 1)
                {
                    String chr2 = Convert.ToString(hexStr[0]);
                    if (hexStr.Length >= 2)
                    {
                        chr2 += Convert.ToString(hexStr[1]);
                    }
                    //sendStream.WriteByte((Byte)Int32.Parse(chr2, NumberStyles.HexNumber));
                    byteSize[1] = (Byte)Int32.Parse(chr2, NumberStyles.HexNumber);
                }

                // file size 
                sendStream.Write(byteSize, 0, byteSize.Length);

                sendStream.WriteByte(0);
                sendStream.WriteByte(0);
                //var fileBytes = new Byte[bytes.Length];
                //fs.Read(fileBytes, 0, fileBytes.Length);

                sendStream.Write(bytes, 0, bytes.Length);

                while (true)
                {
                    var readByte =
                        sendStream.ReadByte();
                    if (readByte == 1)
                    {
                        var buffer = new byte[3];
                        sendStream.Read(buffer, 0, 3);
                        var dataLen = sendStream.ReadByte();
                        sendStream.Read(buffer, 0, 3);
                        var dataBuffer = new byte[dataLen];
                        sendStream.Read(dataBuffer, 0, dataLen);
                        var str = Encoding.ASCII.GetString(dataBuffer);
                        return str;

                    }
                    else if (readByte < 0)
                    {
                        return null;
                    }

                }
            }
            catch
            {
            }
            return null;
        }

        public String SendFile(String fileName, int timeout = 10000)
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(Host, Port);
            byte[] byteSize = new byte[2];

            if (tcpClient.Connected)
            {
                var sendStream = tcpClient.GetStream();

                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    sendStream.WriteByte(0);
                    var fileSize = fs.Length;
                    var hexStr = fileSize.ToString("X");
                    while (hexStr.Length < 4)
                    {
                        hexStr = "0" + hexStr;
                    }
                    if (hexStr.Length >= 3)
                    {
                        String chr1 = Convert.ToString(hexStr[2]);
                        if (hexStr.Length >= 4)
                        {
                            chr1 += Convert.ToString(hexStr[3]);
                        }
                        //sendStream.WriteByte((Byte)Int32.Parse(chr1, NumberStyles.HexNumber));
                        byteSize[0] = (Byte)Int32.Parse(chr1, NumberStyles.HexNumber);
                    }
                    else
                    {
                        sendStream.WriteByte(0);
                        byteSize[0] = 0;
                    }
                    if (hexStr.Length >= 1)
                    {
                        String chr2 = Convert.ToString(hexStr[0]);
                        if (hexStr.Length >= 2)
                        {
                            chr2 += Convert.ToString(hexStr[1]);
                        }
                        //sendStream.WriteByte((Byte)Int32.Parse(chr2, NumberStyles.HexNumber));
                        byteSize[1] = (Byte)Int32.Parse(chr2, NumberStyles.HexNumber);
                    }

                    // file size 
                    sendStream.Write(byteSize, 0, byteSize.Length);

                    sendStream.WriteByte(0);
                    sendStream.WriteByte(0);
                    var fileBytes = new Byte[fs.Length];
                    fs.Read(fileBytes, 0, fileBytes.Length);

                    sendStream.Write(fileBytes, 0, fileBytes.Length);


                }
                while (true)
                {
                    var readByte =
                        sendStream.ReadByte();
                    if (readByte == 1)
                    {
                        var buffer = new byte[3];
                        sendStream.Read(buffer, 0, 3);
                        var dataLen = sendStream.ReadByte();
                        sendStream.Read(buffer, 0, 3);
                        var dataBuffer = new byte[dataLen];
                        sendStream.Read(dataBuffer, 0, dataLen);
                        var str = Encoding.ASCII.GetString(dataBuffer);
                        return str;

                    }
                    else if (readByte < 0)
                    {
                        return null;
                    }

                }
            }
            return null;
        }
    }

}
