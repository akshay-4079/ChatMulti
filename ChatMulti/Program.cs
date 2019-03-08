using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ChatMulti
{
  


    class Program
    {
     public static Dictionary<string,TcpClient> Clients = new Dictionary<string,TcpClient>();
        public static Stack<string> GroupMessage = new Stack<string>();
        public static void Main(string[] args)
        {

            IPAddress ServerAddress = IPAddress.Parse("192.168.10.71");
            TcpListener serversocket = new TcpListener(ServerAddress, 8888);
            int counter = 0;
            TcpClient clientSocket = default(TcpClient);
            serversocket.Start();
            Console.WriteLine("Chat Room ");
            while (true) {
                counter += 1;
                clientSocket = serversocket.AcceptTcpClient();
                HandleClient Client = new HandleClient();
                Client.StartClient(clientSocket, counter);
            }
        
        }


    }
    class HandleClient:Program
    {
        TcpClient Client;
        public string ClientName;
        public Stack<string> UserMessages = new Stack<string>();
        public void StartClient(TcpClient inClient,int count)
        {
            this.Client = inClient;
            Console.WriteLine($"Client {count}  ");
            AssignName();
            Clients.Add(ClientName,inClient);
            Thread thread1 = new Thread(Chat);
            thread1.Start();

        }

        private void AssignName()
        {
            string returndata;
            byte[] bytesFrom = new byte[10025];
            NetworkStream networkStream = Client.GetStream();
            int reply = networkStream.Read(bytesFrom, 0, 10025);
            if (reply > 0)
            {

                returndata = System.Text.Encoding.ASCII.GetString(bytesFrom);
                ClientName = returndata;
                Console.WriteLine(returndata);
                networkStream.Flush();
            }
        }
        void Speak()
        {
            try
            {
                foreach (string entry in GroupMessage)
                {
                    GroupMessage.Pop();
                    foreach (KeyValuePair<string, TcpClient> entry1 in Clients)
                    {
                        byte[] bytesTo = new byte[10025];
                        NetworkStream stream = entry1.Value.GetStream();
                        bytesTo = System.Text.Encoding.ASCII.GetBytes(entry);
                        stream.Write(bytesTo, 0, bytesTo.Length);
             

                    }
                }
            }
            catch
            {
                Console.WriteLine("Error Occured");
            }
        }
        private void Chat()
        {
            string returndata;
            byte[] bytesFrom = new byte[10025];
            byte[] bytesTo = new byte[10025];
            NetworkStream networkStream = Client.GetStream();
            while (true)
            {
                try
                {


                    int reply = networkStream.Read(bytesFrom, 0, 10025);
                    if (reply > 0)
                    {

                        returndata = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        string[] Message = returndata.Split('/');
                        if (Message.Length < 2)
                        {
                            if (Message[0].ToLower() == "all")
                            {
                                foreach(KeyValuePair<string,TcpClient> entry in Clients)
                                {
                                    bytesTo = System.Text.Encoding.ASCII.GetBytes(entry.Key);
                                    networkStream.Write(bytesTo,0,bytesTo.Length);
                                    networkStream.Flush();
                                }
                            }
                            else
                            {
                                GroupMessage.Push(Message[0]);
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<string, TcpClient> entry in Clients)
                            {
                                if (Message[0] == entry.Key)
                                {
                                    NetworkStream stream = entry.Value.GetStream();
                                    bytesTo = System.Text.Encoding.ASCII.GetBytes(Message[1]);
                                    stream.Write(bytesTo, 0, bytesTo.Length);
                                    stream.Close();
                                }
                            }

                        }
                        Speak();
                    }

                    //bytesTo = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine());
                    //networkStream.Write(bytesTo, 0, bytesTo.Length);
                    //networkStream.Flush();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
