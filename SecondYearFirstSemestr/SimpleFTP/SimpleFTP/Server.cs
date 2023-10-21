using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleFTP
{
    public class Server
    {
        TcpListener tcpListener;
        TcpClient? tcpClient;
        NetworkStream? stream;

        public Server(String address, int port)
        {
            var ip = IPAddress.Parse(address);
            tcpListener = new TcpListener(ip, port);
        }

        public async void Start()
        {
            try
            {
                tcpListener.Start();
            }
            catch (Exception ex)
            {
                return;
            }

            while (true)
            {
                tcpClient = await tcpListener.AcceptTcpClientAsync();
                Task.Run(async ()=> await ParallelRequestsProcessing(tcpClient));              
            }
        }

        private async void ResponseToListRequest(String path)
        {
            if (!Directory.Exists(path))
            {
                await stream.WriteAsync(Encoding.UTF8.GetBytes("-1\n"));
                return;
            }

            String[] directories = Directory.GetDirectories(path);
            String[] files = Directory.GetFiles(path);
            int count = directories.Length + files.Length;

            var response = new StringBuilder();
            response.Append(count + " ");
            for (int i = 0; i < directories.Length; i++)
            {
                response.Append("/." + directories[i] + " false ");
            }

            for (int i = 0; i < files.Length; ++i)
            {
                response.Append("/." + files[i] + " true ");
            }

            response.Append("\n");
            await stream.WriteAsync(Encoding.UTF8.GetBytes(response.ToString()));
        }

        private async void ResponseToGetRequest(String path)
        {
            if (!File.Exists(path))
            {
                await stream.WriteAsync(BitConverter.GetBytes(-1));
                return;
            }

            FileInfo fileInfo = new FileInfo(path);
            long size = fileInfo.Length;
            await stream.WriteAsync(BitConverter.GetBytes(size));

            var response = new StringBuilder();
            var streamReader = new StreamReader(path);
            response.Append(streamReader.ReadToEnd());
            streamReader.Close();
            await stream.WriteAsync(Encoding.UTF8.GetBytes(response.ToString()));
        }

        private async Task ParallelRequestsProcessing(TcpClient tcpClient)
        {
             stream = tcpClient.GetStream();

                var buffer = new List<byte>();
                int byteReaded = 10;

                bool isProcessing = true;
                while (isProcessing)
                {
                    while ((byteReaded = stream.ReadByte()) != '\n')
                    {
                        buffer.Add((byte)(byteReaded));
                    }

                    var message = Encoding.UTF8.GetString(buffer.ToArray());
                    buffer.Clear();

                    String[] request = message.Split(" ");
                    switch (request[0])
                    {
                        case "List":
                            ResponseToListRequest(request[1]);
                            break;
                        case "Get":
                            ResponseToGetRequest(request[1]);
                            break;
                        case "END":
                            isProcessing = false;
                            break;
                        case "":
                            break;
                        default:
                            throw new Exception();
                    }
                }
        }



    }
}
