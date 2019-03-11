using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ChatClient
{
    public class ChatClient { 
        public void MakeCLient()
        {
            TcpClient clientSocket = new TcpClient();
            Console.WriteLine("Client Started");
            clientSocket.Connect("192.168.10.71", 8888);
            Console.WriteLine("Client Connected");
            bool connect = true;
            NetworkStream serverstream = clientSocket.GetStream();
            byte[] bytesTo = new byte[500];
            byte[] inStream = new byte[500];
            Console.WriteLine("Enter Help for Tutorial");
            Console.WriteLine("Enter Your Name");
            string Name = Console.ReadLine();
            bytesTo = System.Text.Encoding.ASCII.GetBytes(Name);
            serverstream.Write(bytesTo, 0, bytesTo.Length);
            while (connect)
            {
                Console.WriteLine("Enter Help for Tutorial");
                Console.WriteLine("Enter A Message");
                string Response = Console.ReadLine();
                if (Response == "help")
                {
                   string Response1 = "Type all to see the list of people connected to the room|| Any message sent as such will be broadcast|| For private messaging send the name with / after,Example to talk to Akshay you would key in Akshay/Your Mesaage|| If you key in an already used name you will be assigned a generic name";
                   Console.WriteLine(Response1);
                }
                bytesTo = System.Text.Encoding.ASCII.GetBytes(Response);
                serverstream.Write(bytesTo, 0, bytesTo.Length);
                serverstream.Read(inStream, 0, inStream.Length);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                Console.WriteLine(returndata);
                serverstream.Flush();
              
            }
        }


    }
    class Program
    {
        public static void Main(string[] args)
        {
            ChatClient client = new ChatClient();
            ChatClient client2 = new ChatClient();
            client.MakeCLient();
        }
    }
}
