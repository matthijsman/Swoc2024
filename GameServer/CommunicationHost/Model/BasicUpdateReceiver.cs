using PlayerInterface;
using Grpc.Core;
using CommunicationHost.Utilities;

namespace CommunicationHost.Model
{
    public class BasicUpdateReceiver<T>
    {
        public readonly string Name;
        private readonly CancellationToken _token;
        private IServerStreamWriter<T>? _writeStream;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public string Identifier { get; }

        public BasicUpdateReceiver()
        {
            _semaphore.Wait();
        }

        public BasicUpdateReceiver(string name, string identifier, CancellationToken token) : this()
        {
            Name = name;
            Identifier = identifier;
            _token = token;
        }

        public void AddStream(IServerStreamWriter<T> stream)
        {
            _writeStream = stream;
        }

        public void StopPlayer()
        {
            _semaphore.Release();
        }

        public async Task WaitForMessages()
        {
            await _semaphore.WaitAsync();
        }

        public bool SendUpdate(T message)
        {
            if (_token.IsCancellationRequested)
            {
                return false;
            }
            else if(_writeStream == null)
            {
                return false;
            }
            else
            {
                _writeStream?.WriteAsync(message).FireAndForget();
                return true;
            }
        }
    }
}