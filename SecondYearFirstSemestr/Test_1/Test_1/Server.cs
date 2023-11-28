using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace Test_1
{
    public class Server
    {
        private TcpListener listener;
        private Stream stream;
        const string address = "127.00.00.01";
        public Server(int port)
        {
            IPAddress ipAddress = IPAddress.Parse(address);
            listener = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            listener.Start();
        }

        public void AcceptConnection()
        {
            var tcpClient = listener.AcceptTcpClient();
            stream = tcpClient.GetStream();
        }

        public async Task SendMessage(string message)
        {
            var data = Encoding.UTF8.GetBytes(message + '\n');
            await stream.WriteAsync(data);
        }

        public void ListenChat()
        {
            Task.Run(() =>
            {
                var data = new List<byte>();
                int bytesRead = 10;
                while (true)
                {
                    while ((bytesRead = stream.ReadByte()) != '\n')
                    {
                        data.Add((byte)bytesRead);
                    }

                    var message = Encoding.UTF8.GetString(data.ToArray());
                    Console.WriteLine(message);
                    data.Clear();
                }
            });
        }
    }
}
