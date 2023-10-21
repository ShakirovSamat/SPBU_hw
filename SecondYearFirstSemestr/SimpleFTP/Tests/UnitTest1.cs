using SimpleFTP;
using System.Net.Security;
using System.Security.Claims;

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
            client = new Client("127.00.00.01", 8888);
        }

        [Test]
        public void ListRequestTestWithGoodData()
        {
            Directory.CreateDirectory("Directory");
            using (File.Create("Directory/1.txt")) ;
            Directory.CreateDirectory("Directory/InnerDirectory");
            Task<string> response = client.Request($"List Directory");

            Assert.That(response.Result, Is.EqualTo("2 /.Directory\\InnerDirectory false /.Directory\\1.txt true "));

            File.Delete("Directory/1.txt");
            Directory.Delete("Directory/InnerDirectory");
            Directory.Delete("Directory");
        }

        [Test]
        public void GetRequestTestWithGoodData()
        {
            string text = "Объективно всё хорошо\nСубъективно я не вывожу";
            using (File.Create("1.txt")) ;
            StreamWriter stream = new StreamWriter("1.txt");
            stream.Write(text);
            stream.Close();

            Task<string> response = client.Request("Get 1.txt");

            Assert.That("84 " + text, Is.EqualTo(response.Result));

            File.Delete("1.txt");
        }

        [Test]
        public void ListRequestWithBadData()
        {
            Task<string> response = client.Request("List NonExistent");
            Assert.That("-1", Is.EqualTo(response.Result));
        }

        [Test]
        public void GetRequestWithBadData()
        {
            Task<string> response = client.Request("Get NonExistent");
            Assert.That("-1", Is.EqualTo(response.Result));
        }
    }
}