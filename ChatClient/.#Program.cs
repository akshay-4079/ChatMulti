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
    class Message
    {
        public string Name;
        public string Message;
    }



    class Program
    {
     public static Dictionary<string,TcpClient> Clients = new Dictionary<string,TcpClient>();
        public static Stack<Message> Messages = new Stack<Message>();
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

            }
        }
        private void Speak()
        {
            foreach (string entry in GroupMessage){
                GroupMessage.Pop();
                foreach(KeyValuePair<string,TcpClient> entry1 in Clients)
                {
                    NetworkStream network = entry1.Value.GetStream();
                    string CurrMessage = "Group:" + entry;
                    bytesTo = System.Text.Encoding.ASCII.GetBytes(CurrMessage);
                    network.Write(bytesTo, 0, bytesTo.Length);
                    network.Flush;

                }
            }
        }

        private void Listen()
        {
            string returndata;
            byte[] bytesFrom = new byte[10025];
            byte[] bytesTo = new byte[10025];
            while (true)
            {
                try
                {
                    NetworkStream networkStream = Client.GetStream();

                    int reply = networkStream.Read(bytesFrom, 0, 10025);
                    if (reply > 0)
                    {

                        returndata = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        string[] Reply = returndata.Split('/');
                        if (Reply.Length <2)
                        {
                            if (returndata.ToLower() == "all")
                            {
                                Console.WriteLine("Current Room Status");
                                foreach (KeyValuePair<string, TcpClient> entry in Clients)
                                {
                                    Console.WriteLine(entry.Key);
                                }
                            }
                            GroupMessage.Push(returndata);
                        }
                        else
                        {
                            Message M = new Message;
                            M.Name = Reply[0];
                            M.Message = Reply[1];
                            foreach(KeyValuePair<string,TcpClient> entry in Clients)
                            {
                                if (entry.Key = M.Name)
                                {
                                    NetworkStream network = entry.Value.GetStream();
                                    string CurrMessage=ClientName+':'+M.Message
                                    bytesTo = System.Text.Encoding.ASCII.GetBytes(CurrMessage);
                                    networkStream.Write(bytesTo, 0, bytesTo.Length);
                                    networkStream.Flush;
                                }   
                            }
                        }
                       


                    }
                    Speak();
                    //bytesTo = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine());
                    //networkStream.Write(bytesTo, 0, bytesTo.Length);
                    //networkStream.Flush();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
               
            }
            Clients.Remove(ClientName);
        } 
    }
}
