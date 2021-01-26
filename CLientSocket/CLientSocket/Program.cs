using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CLientSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            client();

        }
        static void client() {
            try
            {
                string name = Dns.GetHostName();
                IPHostEntry ipHost = Dns.GetHostEntry(name);
                IPAddress ipAddr = ipHost.AddressList[1];
                IPEndPoint endpoint = new IPEndPoint(ipAddr, 10245);
                Socket Sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try {
                    Sender.Connect(endpoint);
                    Console.WriteLine("Connected----{0}",Sender.RemoteEndPoint);
                    byte[] msg = new byte[1024];
                    int byteRecv = Sender.Receive(msg);
                    Console.WriteLine("Received Text From Server={0}",Encoding.ASCII.GetString(msg, 0, byteRecv));
                    Thread sendThread = new Thread(()=>SendMsg(Sender));
                    sendThread.Start();
                    Thread recvThread = new Thread(() => RecvMsg(Sender));
                    recvThread.Start();                    
                    while(true){
                    if(!sendThread.IsAlive|| !recvThread.IsAlive)
                    Sender.Close();
 

                    
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

     static void SendMsg(Socket Sender) {
         try
         {
             while (true)
             {
                 string str = Console.ReadLine();
                 byte[] bytes = Encoding.ASCII.GetBytes(str);
                 int byteSent = Sender.Send(bytes);
                 if (str == "-127")
                 {
                     break;
                 }

             }
         }
         catch (Exception ex) {
             Console.WriteLine(ex.Message);
         }
        }
      static void RecvMsg(Socket Sender) {
          string data="";
          byte[] msg = new byte[1024];
          while (true)
          {
              int byteRecv = Sender.Receive(msg);
              data=Encoding.ASCII.GetString(msg, 0, byteRecv);
              if (data != "")
                  Console.WriteLine("Received Text From Server={0}", data);
              if (data == "-127")
                  break;  
          }
          }
    }
}
