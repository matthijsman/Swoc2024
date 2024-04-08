
using AdminInterface;
using CommunicationHost.GameEngine;
using CommunicationHost.Model;
using CommunicationHost.Utilities;
using Grpc.Core;

namespace CommunicationHost.Services
{
    public class AdminService : AdministrationHost.AdministrationHostBase
    {
        private readonly ILogger<AdminService> _logger;
        private readonly BattleSnake _gameEngine;

        public AdminService(ILogger<AdminService> logger, BattleSnake gameEngine)
        {
            _logger = logger;
            _gameEngine = gameEngine;
        }

        public override Task<EmptyRequest> SetSettings(SettingsRequest request, ServerCallContext context)
        {
            _gameEngine.Initialise(request.Dimensions.ToArray()).FireAndForget();
            return Task.FromResult(new EmptyRequest());
        }

        public override Task<EmptyRequest> StartGame(EmptyRequest request, ServerCallContext context)
        {
            _gameEngine.StartGame();
            return Task.FromResult(new EmptyRequest());
        }

        public override Task<EmptyRequest> StopGame(EmptyRequest request, ServerCallContext context)
        {
            _gameEngine.EndGame();
            return Task.FromResult(new EmptyRequest());
        }
    }
}