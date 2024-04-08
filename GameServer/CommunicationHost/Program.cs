using CommunicationHost.GameEngine;
using CommunicationHost.Services;

namespace CommunicationHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            builder.Services.AddSingleton<BattleSnake>();

            // Add services to the container.
            builder.Services.AddCors();
            builder.Services.AddGrpc();

            var app = builder.Build();

            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
            app.UseGrpcWeb();
            // Configure the HTTP request pipeline.
            app.MapGrpcService<AdminService>();
            app.MapGrpcService<PlayerService>().EnableGrpcWeb();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.Run();
        }

    }
}