using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TweeterModel;
using TweetWorker.Aplication.Abstractions;
using TweetWorker.Entities;

namespace TweetWorker.Aplication.Consumers
{
    public class TweetConsumer : BackgroundService, IDisposable
    {
        private const string _queueName = "Tweets";
        private readonly IChannel _channel;
        private readonly IConnection _connection;
        private readonly ITweetRepository _tweetRepository;
        
        public TweetConsumer(IConfiguration configuration, ITweetRepository tweetRepository)
        {
            _tweetRepository = tweetRepository;
            var mqConfig = GetConfigurationMQ(configuration);
            CreateConectionMQ(mqConfig, ref _connection, ref _channel);
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
                catch(Exception ex)
                {
                    await _channel.BasicNackAsync(msj.DeliveryTag, false, false);
                }
            };
            await _channel.BasicConsumeAsync(_queueName, false, consumer, stoppingToken);
        }

        private async Task ProcessMessage(BasicDeliverEventArgs msj)
        {
            var tweet = GetMessage<TweetModel>(msj.Body.ToArray());
            await _tweetRepository.SaveTweet(new TweetEntity { Message = tweet.Message, UserId = tweet.UserId });
        }

        private T GetMessage<T>(byte[]? body)
        {
            var message = Encoding.UTF8.GetString(body);
            return JsonConvert.DeserializeObject<T>(message);
        }

        private RabbitConfiguration GetConfigurationMQ(IConfiguration configuration)
        {
            var mqConfig = configuration.GetRequiredSection("RabbitMQ").Get<RabbitConfiguration>();

            if (mqConfig == null)
                throw new ArgumentException("No se encuentra configurada la seccion: RabbitMQ");

            return mqConfig;
        }

        private void CreateConectionMQ(RabbitConfiguration config, ref IConnection connection, ref IChannel channel)
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
