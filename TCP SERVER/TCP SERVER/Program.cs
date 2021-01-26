using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace TCP_SERVER
{
    class Program
    {
        static void Main(string[] args)
        {
            Server();
        }
        static void Server() {
            try
            {
                string name = Dns.GetHostName();
                IPHostEntry ipHost = Dns.GetHostEntry(name);
                IPAddress ipAddr = ipHost.AddressList[1];
                IPEndPoint endpoint = new IPEndPoint(ipAddr, 10245);
                Socket Listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Listener.Bind(endpoint);
                    Listener.Listen(5);
                    while (true)
                    {
                        Console.WriteLine("Ready for accepting Connection");
                        Socket clientSocket = Listener.Accept();
                        Console.WriteLine("Client Connected");
                        byte[] msg = Encoding.ASCII.GetBytes("You Connected!");
                        clientSocket.Send(msg);
                        Thread t1 = new Thread(() => sendMsg(clientSocket));
                        Thread t2 = new Thread(() => RecvMsg(clientSocket));
                        t1.Start();
                        t2.Start();
                        while (true)
                        {
                            if (!t2.IsAlive || !t1.IsAlive)
                                clientSocket.Close();
                                Listener.Close();
                               
                        }
                        
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

        }
        static void sendMsg(Socket Sender){
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
        static void RecvMsg(Socket clientSocket) {
            string data = "";
            byte[] bytes = new byte[2048];
            try
            {
                while (true)
                {
                    int numByte = clientSocket.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, numByte);
                    if (data != "")
                        Console.WriteLine("Received Text From Client={0}", data);
                    if (data == "-127")
                    {
                        break;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
