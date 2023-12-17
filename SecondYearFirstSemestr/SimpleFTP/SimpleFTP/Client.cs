using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

namespace SimpleFTP
{
	public class Client
	{
		private const int LINE_END = (int)'\n';
		TcpClient? client;
		NetworkStream stream;

		public Client()
		{
			client = new TcpClient();
		}


		public async void ConnectToAsync(string address, int port)
		{
			try
			{
				await client.ConnectAsync(address, port);
			}
			catch (SocketException ex)
			{
				Console.WriteLine("Неверный ip адресс или порт");
			}

			stream = client.GetStream();
		}
		public Task<byte[]> Request(String request)
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

		private async Task<byte[]> ListRequest(String inputRequest)
		{
			byte[] request = Encoding.UTF8.GetBytes(inputRequest + '\n');
			await stream.WriteAsync(request);

			var buffer = new List<byte>();
			var byteReaded = LINE_END;
			while ((byteReaded = stream.ReadByte()) != '\n')
			{
				buffer.Add((byte)byteReaded);
			}

			return buffer.ToArray();
		}

		public async Task<byte[]> GetRequest(String inputRequest)
		{
			byte[] request = Encoding.UTF8.GetBytes(inputRequest + '\n');
			await stream.WriteAsync(request);

			var buffer = new List<byte>();
			var byteReaded = LINE_END;
			while ((byteReaded = stream.ReadByte()) != '\n')
			{
				buffer.Add((byte)byteReaded);
			}

			if (Encoding.UTF8.GetString(buffer.ToArray()) == "-1")
			{
				return buffer.ToArray();
			}

			byte[] size = buffer.ToArray();

			byte[] data = new byte[long.Parse(Encoding.UTF8.GetString(size))];
			long bytes = await stream.ReadAsync(data);

			return size.Concat(data).ToArray();
		}
	}

}
