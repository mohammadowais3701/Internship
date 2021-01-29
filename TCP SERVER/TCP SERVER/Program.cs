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
      static public  Dictionary<String, Socket> connectionList;
      static int count = 0;
      static object obj;
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
                connectionList=new Dictionary<String,Socket>();
                obj = new object();
                Socket Listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Listener.Bind(endpoint);
                    Console.WriteLine("How many clients do you want to connect");
                    int c = Convert.ToInt32(Console.ReadLine());
                    Listener.Listen(c);
                    Console.WriteLine("Ready for accepting Connection");
                    for (int i = 1; i <= c; i++)
                    {
                        Thread t1 = new Thread(() => connectCleints(Listener));
                        t1.Start();
                        lock (obj)
                        {
                            count++;
                        }
                    }
                    Thread sendtext = new Thread(sendMsg);
                    sendtext.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch(Exception ex) {
          
                Console.WriteLine(ex.Message);
            }

        }

        static void connectCleints(Socket Listener) {
            Socket clientSocket = Listener.Accept();
            byte[] bytes = new byte[2048];
            int numByte = clientSocket.Receive(bytes);
            string data = Encoding.ASCII.GetString(bytes, 0, numByte);
            Console.WriteLine("Client Connected--->"+data);
            lock (obj)
            {
                connectionList.Add(data, clientSocket);
            }
            byte[] msg = Encoding.ASCII.GetBytes("You Connected!");
            clientSocket.Send(msg);
        //    Thread t1 = new Thread(() => sendMsg(clientSocket));
            Thread t2 = new Thread(() => RecvMsg(clientSocket));
          //  t1.Start();
            t2.Start();
            try
            {
                while (true)
                {
                    if (!t2.IsAlive )
                    {
                        Console.WriteLine("Client Close");
                        lock (obj)
                        {
                            foreach (var pair in connectionList)
                            {
                                if (pair.Value == clientSocket)
                                {
                                    connectionList.Remove(pair.Key);
                                }
                            }
                            count--;
                            clientSocket.Close();
                        }
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

static void sendMsg(){
            Socket client;
            String name;
            while(true){
            if (connectionList.Count != 0){
            try
            {
                while (true)
                {       
                    Console.WriteLine("Enter Client Name for sending Msg");
                    name = Console.ReadLine();
                    try
                    {
                        client=connectionList[name];
                        break;
                    }
                    catch(Exception ex) {
                        Console.WriteLine("Your target client is not available");
                    }
                }
                    Console.WriteLine("Enter Your Message");
                    string str = Console.ReadLine();
                    byte[] bytes = Encoding.ASCII.GetBytes(str);
                    int byteSent = client.Send(bytes);
                    if (str == "-127")
                    {
                        lock (obj)
                        {
                            connectionList.Remove(name);
                        }
                        lock (obj)
                        {
                        count--;
                        client.Close();
                        }
                    }
            }
            catch (Exception ex) {
                Console.WriteLine("In Send Msg ");
                Console.WriteLine(ex.Message);
            }
        }
        }
   }
static void RecvMsg(Socket clientSocket) {
            string data = "",name="";
            byte[] bytes = new byte[2048];
            try
            {
                while (true)
                {
                    int numByte = clientSocket.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, numByte);
                    lock(obj){
                    foreach (var pair in connectionList)
                    {
                        if (pair.Value == clientSocket)
                        {
                            name = pair.Key; 
                        }
                    }
                }
                    if (data != "")
                        Console.WriteLine("Received Text From {0}={1}",  name , data);
                    if (data == "-127")
                    {
                        lock(obj)
                        count--;
                        break;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("In Recv Msg ");
                Console.WriteLine(ex.Message);
            }
        }

    }
}
