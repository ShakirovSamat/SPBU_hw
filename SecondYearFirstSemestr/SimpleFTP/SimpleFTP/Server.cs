using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleFTP
{
	public class Server
	{
		private const int LINE_END = '\n';
		TcpListener tcpListener;
		TcpClient? tcpClient;
		NetworkStream? stream;

		public Server(String address, int port)
		{
			try
			{
				var ip = IPAddress.Parse(address);
				tcpListener = new TcpListener(ip, port);
			}
			catch (ArgumentNullException ex)
			{
				Console.WriteLine("Неверный IP address");
			}
			catch (FormatException ex)
			{
				Console.WriteLine("Неверный IP address");
			}
			catch (ArgumentOutOfRangeException ex)
			{
				Console.WriteLine("Неверный port");
			}
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
				Task.Run(async () => await ParallelRequestsProcessing(tcpClient));
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
			foreach (var directory in directories)
			{
				response.Append("/." + directory + " false ");
			}

			foreach (var file in files)
			{
				response.Append("/." + file + " true ");
			}

			response.Append("\n");
			await stream.WriteAsync(Encoding.UTF8.GetBytes(response.ToString()));
		}

		private async void ResponseToGetRequest(String path)
		{
			if (!File.Exists(path))
			{
				await stream.WriteAsync(Encoding.UTF8.GetBytes("-1\n"));
				return;
			}

			FileInfo fileInfo = new FileInfo(path);
			string size = (fileInfo.Length).ToString() + " \n";
			List<byte> response = new List<byte>();
			response.AddRange(Encoding.UTF8.GetBytes(size));
			response.AddRange(File.ReadAllBytes(path));

			await stream.WriteAsync(response.ToArray());
		}

		private async Task ParallelRequestsProcessing(TcpClient tcpClient)
		{
			CancellationTokenSource cancellationToken = new();
			stream = tcpClient.GetStream();

			var buffer = new List<byte>();
			int byteReaded = LINE_END;

			while (!cancellationToken.IsCancellationRequested)
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
						cancellationToken.Cancel();
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
