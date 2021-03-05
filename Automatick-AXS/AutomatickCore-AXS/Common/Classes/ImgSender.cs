using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;


namespace Automatick.Core
{
    public class ImgSender
    {
        public String Host { get; set; }
        public Int32 Port { get; set; }
        public ImgSender(String host, Int32 port)
        {
            Host = host;
            Port = port;
        }


        public String SendBytes(byte[] bytes, int timeout = 10000)
        {
            try
            {
                var tcpClient = new TcpClient();
                tcpClient.Connect(Host, Port);
                if (tcpClient.Connected)
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
                return null;
            }
            catch
            {
                return null;
            }
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
