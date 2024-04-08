using AdminInterface;
using Grpc.Net.Client;
using static AdminInterface.AdministrationHost;

namespace AdministrationClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7072");
            var client = new AdministrationHostClient(channel);
            while (true)
            {
                Task.Delay(1000).Wait();
                Console.WriteLine("enter the dimensions");
                var line = Console.ReadLine();
                Console.WriteLine("is debug mode? default is yes");
                var dbgLine = "";// Console.ReadLine();
                var debugMode = true;
                if (dbgLine == "0")
                {
                    debugMode = false;
                }
                var dimensions = line.Split(" ").Select(int.Parse).ToArray();
                var setRequest = new SettingsRequest();
                setRequest.Dimensions.AddRange(dimensions);
                setRequest.DebugMode = debugMode;
                client.SetSettings(setRequest);

                Console.WriteLine("press enter to start");
                Console.ReadLine();
                client.StartGame(new EmptyRequest());
                Console.WriteLine("press enter to stop");
                Console.ReadLine();
                client.StopGame(new EmptyRequest());
                Console.WriteLine("Thanks for playing");
            }
        }
    }
}