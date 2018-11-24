using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class Program
    {
        private static SearchManager _searchManager;
        static void Main(string[] args)
        {
            _searchManager = SearchManager.Create();

            Task.Run(() => CreateTcpListener(9999));

            while (true)
            {
                Console.Write("Arama ifadesini giriniz:" );

                var searchModel = _searchManager.Parse(Console.ReadLine());
                var results = _searchManager.Search(searchModel);

                foreach (var result in results)
                {
                    result.SummaryText.ForEach(i => Console.WriteLine("Summary Texts : {0}\n\n", i));
                }
            }
        }

        private static void CreateTcpListener(int port)
        {
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();

            while (true)
            {
                var client = server.AcceptTcpClient();

                var networkStream = client.GetStream();

                while (client.Connected)
                {
                    byte[] bytes = new byte[1024];
                    networkStream.Read(bytes, 0, bytes.Length);
                    var message = Encoding.UTF8.GetString(bytes).Trim('\0');

                    var searchModel = _searchManager.Parse(message);
                    var results = _searchManager.Search(searchModel);
                    var resultsString = JsonConvert.SerializeObject(results);

                    bytes = Encoding.UTF8.GetBytes(resultsString);
                    networkStream.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}
