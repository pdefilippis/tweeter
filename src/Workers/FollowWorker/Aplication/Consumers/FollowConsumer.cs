using FollowWorker.Aplication.Abstractions;
using FollowWorker.Domains;
using FollowWorker.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TweeterModel;

namespace FollowWorker.Aplication.Consumers
{
    public class FollowConsumer : BackgroundService, IDisposable
    {
        private const string _queueName = "Follows";
        private readonly IChannel _channel;
        private readonly IConnection _connection;
        private readonly IFollowRepository _followRepository;

        public FollowConsumer(IConfiguration configuration, IFollowRepository followRepository)
        {
            _followRepository = followRepository;
            var mqConfig = GetConfigurationMQ(configuration);
            CreateConnexionMQ(mqConfig, ref _connection, ref _channel);
            CreateQueue();
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            if (_channel == null)
                throw new Exception("No se encuentra creado el canal");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, msj) =>
            {
                try
                {
                    await ProcessMessage(msj);
                    await _channel.BasicAckAsync(msj.DeliveryTag, false);
                }
                catch
                {
                    await _channel.BasicNackAsync(msj.DeliveryTag, false, false);
                }
            };
            await _channel.BasicConsumeAsync(_queueName, false, consumer, stoppingToken);
        }

        private async Task ProcessMessage(BasicDeliverEventArgs msj)
        {
            var follow = GetMessage<FollowModel>(msj.Body.ToArray());
            await _followRepository.SaveFollow(new FollowEntity { UserId = follow.UserId, FollowTo = follow.FollowTo });
        }

        private T GetMessage<T>(byte[]? body)
        {
            var message = Encoding.UTF8.GetString(body);
            return JsonConvert.DeserializeObject<T>(message);
        }

        private RabbitConfigurationEntity GetConfigurationMQ(IConfiguration configuration)
        {
            var mqConfig = configuration.GetRequiredSection("RabbitMQ").Get<RabbitConfigurationEntity>();
            if (mqConfig == null)
                throw new ArgumentException("No se encuentra configurada la seccion: RabbitMQ");

            return mqConfig;
        }

        private void CreateConnexionMQ(RabbitConfigurationEntity config, ref IConnection connection, ref IChannel channel)
        {
            var factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password,
                Port = config.Port
            };

            connection = factory.CreateConnectionAsync().Result;
            channel = _connection.CreateChannelAsync().Result;
        }

        //TODO: Si fuera mas de una queue se pasaria por parametro el nombre y propiedades de la misma.
        private void CreateQueue()
        {
            var queue = _channel.QueueDeclareAsync(_queueName, true, false, false, null).Result;
        }
    }
}
