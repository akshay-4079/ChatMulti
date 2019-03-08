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
            byte[] bytesTo = new byte[1000];
            byte[] inStream = new byte[1000];

            Console.WriteLine("Enter Your Name");
            string Name = Console.ReadLine();
            bytesTo = System.Text.Encoding.ASCII.GetBytes(Name);
            serverstream.Write(bytesTo, 0, bytesTo.Length);
            while (connect)
            {
                Console.WriteLine("Message personally by typing the persons Name followed by the message, For a brodcast message dont add anything");
            
                Console.WriteLine("Enter A Message");
                bytesTo = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine());
                serverstream.Write(bytesTo, 0, bytesTo.Length);
                serverstream.Read(inStream, 0, 1000);
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
