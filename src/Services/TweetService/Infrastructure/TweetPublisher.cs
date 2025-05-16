using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using TweetService.Aplication.Abstractions;
using TweetService.Entities;
using Microsoft.Extensions.Configuration;

namespace TweetService.Infrastructure
{
    public class TweetPublisher : IMessagePublisher, IDisposable
    {
        private const string queueName = "Tweets";
        private readonly IChannel _channel;
        private readonly IConnection _connection;

        public TweetPublisher(IConfiguration configuration)
        {
            var conf = GetConfigurationServerMQ(configuration);
            CreateConnection(conf, ref _connection, ref _channel);
            DeclareQueue(_channel);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            var body = CreateBodyForMessage(message);
            await _channel.BasicPublishAsync(string.Empty, queueName, body);
        }

        private byte[] CreateBodyForMessage(object message)
        {
            var msj = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(msj);
        }

        private RabbitMqOptions GetConfigurationServerMQ(IConfiguration configuration)
        {
            var conf = configuration.GetRequiredSection("RabbitMQ").Get<RabbitMqOptions>();

            if (conf == null)
                throw new Exception("No existe el nodo RabbitMQ");

            return conf;
        }

        private void CreateConnection(RabbitMqOptions options, ref IConnection connection, ref IChannel channel)
        {
            var factory = new ConnectionFactory
            {
                HostName = options.HostName,
                UserName = options.UserName,
                Password = options.Password,
                Port = options.Port,
            };

            connection = factory.CreateConnectionAsync().Result;
            channel = _connection.CreateChannelAsync().Result;
        }

        private void DeclareQueue(IChannel channel)
        {
            var queue = channel.QueueDeclareAsync(queueName, true, false, false, null).Result;
        }
    }
}
