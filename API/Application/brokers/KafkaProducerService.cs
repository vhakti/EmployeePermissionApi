namespace EmployeePermissionApi.Application.brokers
{
    using Confluent.Kafka;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.Extensions.Options;

    public class KafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaProducerService> logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _topic = kafkaSettings.Value.Topic;
            _logger = logger;

        }

        public async Task SendMessageAsync(string key, string message)
        {
            
                try
                {
                    await _producer.ProduceAsync(_topic, new Message<string, string> { Key = key, Value = message });

                }
                catch (ProduceException<Null, string> ex)
                {
                    _logger.LogError(ex, "Error sending message {message}, reason: {reason}", message, ex.Error.Reason);
                }
                    
            
        }

    }
}
