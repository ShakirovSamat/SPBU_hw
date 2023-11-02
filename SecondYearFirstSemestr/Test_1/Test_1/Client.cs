using System.Net.Sockets;
using System.Text;

namespace Test_1
{
    public class Client
    {
        private TcpClient client;
        private Stream stream;

        public Client()
        {
            client = new TcpClient();
        }

        public async Task<bool> Connect(string address, int port)
        {
            await client.ConnectAsync(address, port);
            stream = client.GetStream();
            return client.Connected;
        }

        public async Task SendMessage(string message)
        {
            var data = Encoding.UTF8.GetBytes(message + '\n');
            await stream.WriteAsync(data);
        }

        public async void Chat()
        {
            ListenChat();
            while (true)
            {
                var message = Console.ReadLine();
                await SendMessage(message);
            }
        }

        public void ListenChat()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var message = new List<byte>();
                    int bytesRead = 10;
                    while ((bytesRead = stream.ReadByte()) != '\n')
                    {
                        message.Add((byte)bytesRead);
                    }

                    Console.WriteLine(Encoding.UTF8.GetString(message.ToArray()));
                    message.Clear();
                }
            });
        }
    }
}
