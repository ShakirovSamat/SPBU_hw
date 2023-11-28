namespace Test_1
{
    class Chat
    {
        public static void Main()
        {
            Client client = new Client();
            Server server;


            Console.Write("Enter IpAddress and port to chat as a client or enter only port to chat as server: ");
            string input = Console.ReadLine();
            string[] data = input.Split(' ');
            if (data.Length == 1)
            {
                server = new Server(8888);
                server.Start();
                server.AcceptConnection();

                server.ListenChat();
                while (true)
                {
                    var massage = Console.ReadLine();
                    server.SendMessage(massage);
                }
            }
            else
            {
                int.TryParse(data[1], out int port);
                client.Connect(data[0], port);

                client.ListenChat();
                while (true)
                {
                    var massage = Console.ReadLine();
                    client.SendMessage(massage);
                }

            }
            
            Console.ReadLine();
        }
    }
}