using System.Net.Sockets;
using System.Text;

namespace SimpleFTP
{
    public class Client
    {
        TcpClient? client;
        NetworkStream stream;

        public Client(String address, int port)
        {
            try
            {
                client = new TcpClient(address, port);
                stream = client.GetStream();
            }
            catch (SocketException ex)
            {
                throw ex;
            }
        }

        public Task<string> Request(String request)
        {
            switch (request.Split(" ")[0])
            {
                case "List":
                    return ListRequest(request);
                    break;
                case "Get":
                    return GetRequest(request);
                    break;
                default:
                    return null;
            }

        }

        private async Task<string> ListRequest(String inputRequest)
        {
            byte[] request = Encoding.UTF8.GetBytes(inputRequest + '\n');
            await stream.WriteAsync(request);

            var buffer = new List<byte>();
            var byteReaded = 10;
            while ((byteReaded = stream.ReadByte()) != '\n')
            {
                buffer.Add((byte)byteReaded);
            }

            var response = Encoding.UTF8.GetString(buffer.ToArray());
            return response;
        }

        public async Task<string> GetRequest(String inputRequest)
        {
            byte[] request = Encoding.UTF8.GetBytes(inputRequest + '\n');
            await stream.WriteAsync(request);


            byte[] sizeBuffer = new byte[8];
            await stream.ReadAsync(sizeBuffer, 0, sizeBuffer.Length);
            int size = BitConverter.ToInt32(sizeBuffer, 0);
            if (size == -1)
            {
                return "-1";
            }

            byte[] data = new byte[size];
            long bytes = await stream.ReadAsync(data);
            var response = $"{ size} {Encoding.UTF8.GetString(data, 0, (int)bytes)}";
            return response;
        }
    }

}
