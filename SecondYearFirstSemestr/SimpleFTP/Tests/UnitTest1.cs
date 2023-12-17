using SimpleFTP;
using System.Net.Security;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Tests
{
	public class Tests
	{
		Server server;
		Client client;

		[SetUp]
		public void Setup()
		{
			server = new Server("127.00.00.01", 8888);
			server.Start();
			client = new Client();
			client.ConnectToAsync("127.00.00.01", 8888);
		}

		[Test]
		public void ListRequestTestWithGoodData()
		{
			Directory.CreateDirectory("Directory");
			using (File.Create("Directory/1.txt")) ;
			Directory.CreateDirectory("Directory/InnerDirectory");
			Task<byte[]> response = client.Request($"List Directory");
			var data = Encoding.UTF8.GetString(response.Result);

			Assert.That(data, Is.EqualTo("2 /.Directory\\InnerDirectory false /.Directory\\1.txt true "));

			File.Delete("Directory/1.txt");
			Directory.Delete("Directory/InnerDirectory");
			Directory.Delete("Directory");
		}

		[Test]
		public void GetRequestTestWithTextData()
		{
			string text = "Объективно всё хорошо\nСубъективно я не вывожу";
			using (File.Create("1.txt")) ;
			StreamWriter stream = new StreamWriter("1.txt");
			stream.Write(text);
			stream.Close();

			Task<byte[]> response = client.Request("Get 1.txt");
			var data = Encoding.UTF8.GetString(response.Result);

			Assert.That("84 " + text, Is.EqualTo(data));

			File.Delete("1.txt");
		}

		[Test]
		public void ListRequestWithBadData()
		{
			Task<byte[]> response = client.Request("List NonExistent");
			var data = Encoding.UTF8.GetString(response.Result);
			Assert.That("-1", Is.EqualTo(data));
		}

		[Test]
		public void GetRequestWithBadData()
		{
			Task<byte[]> response = client.Request("Get NonExistent");
			var data = Encoding.UTF8.GetString(response.Result);
			Assert.That("-1", Is.EqualTo(data));
		}

		[Test]
		public void GetRequestWithByteData()
		{
			var data = new byte[] { 5, 124, 55, 13, 5, 11 };
			using (File.Create("1.txt")) ;
			File.WriteAllBytes("1.txt", data);

			Task<byte[]> response = client.Request("Get 1.txt");
			var responseData = Encoding.UTF8.GetString(response.Result);
			var size = Encoding.UTF8.GetBytes(data.Length.ToString() + " ");
			Assert.That(size.Concat(data).ToArray(), Is.EqualTo(response.Result));

			File.Delete("1.txt");
		}
	}
}