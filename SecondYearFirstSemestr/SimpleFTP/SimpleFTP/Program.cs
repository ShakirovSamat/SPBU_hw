using System.Net.Sockets;

namespace SimpleFTP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Server server;
            Client client;
            Console.WriteLine(Directory.GetCurrentDirectory());
            while (true)
            {
                Console.Write("Введите локальный адресс сервера: ");
                String serverAddress = Console.ReadLine();
                Console.Write("Введите порт сервера: ");
                int.TryParse(Console.ReadLine(), out int serverPort);

                try
                {
                    server = new Server(serverAddress, serverPort);
                    server.Start();
                    client = new Client(serverAddress, serverPort);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Ошибка подключения");
                    continue;
                }
                break;
            }
            
            Console.WriteLine("Подключение прошло успешно");

            while (true)
            {
                Console.WriteLine("Введите запрос:");
                string request = Console.ReadLine();
                Task<string> task = client.Request(request);
                if (task == null)
                {
                    Console.WriteLine("Неверный запрос");
                    continue;
                }
                Console.WriteLine(task.Result);
            }
        }
    }
}
