using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ChatClient
{
    public class ChatClient
    {
        public void MakeCLient()
        {
            TcpClient clientSocket = new TcpClient();
            Console.WriteLine("Client Started");
            clientSocket.Connect("192.168.10.71", 8888);
            Console.WriteLine("Client Connected");
            bool connect = true;
            NetworkStream serverstream = clientSocket.GetStream();
            byte[] bytesTo = new byte[1000];
            byte[] inStream = new byte[1000];
            Console.WriteLine("Type help for details");
            Console.WriteLine("Enter Your Name");
            string Name = Console.ReadLine();
            bytesTo = System.Text.Encoding.ASCII.GetBytes(Name);
            serverstream.Write(bytesTo, 0, bytesTo.Length);
            Thread ct1 = new Thread(Listen);
            Thread ct2 = new Thread(Speak);
            ct1.Start();
            ct2.Start();
            void Speak()
            {
                while (true)
                {

                    Console.WriteLine("Enter A Message");
                    string Response = Console.ReadLine();
                    if (Response.ToLower() == "help")
                    {
                        string Response1 = "Type all to see the list of people connected to the room|| Any message sent as such will be broadcast|| For private messaging send the name with / after,Example to talk to Akshay you would key in Akshay/Your Mesaage|| If you key in an already used name you will be assigned a generic name";
                        Console.WriteLine(Response1);
                        Console.WriteLine("Enter Message");
                        Response = Console.ReadLine();
                    }
                   else if (Response.ToLower() == "exit")
                    {
                        Console.WriteLine("Press Yes To Confirm");
                        if (Console.ReadLine().ToLower()=="y")
                        {
                            Response ="ByeFromChatRoom";
                            bytesTo = System.Text.Encoding.ASCII.GetBytes(Name + '*' + Response);
                            serverstream.Write(bytesTo, 0, bytesTo.Length);
                 
                        }
                        else
                        {
                            Console.WriteLine("Enter Message");
                            Response = Console.ReadLine();
                        }
                    }
                    bytesTo = System.Text.Encoding.ASCII.GetBytes(Name+'*'+Response);
                    serverstream.Write(bytesTo, 0, bytesTo.Length);
                }
            }
            void Listen()
            {

                while (connect)
                {

                    serverstream.Read(inStream, 0, 1000);
                    string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    Console.WriteLine(returndata);

                }
            }
        }


    }
    class Program
    {
        public static void Main(string[] args)
        {
            ChatClient client = new ChatClient();
            client.MakeCLient();
        }
    }
}
