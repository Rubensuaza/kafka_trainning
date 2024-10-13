using Confluent.Kafka;
using io.quind.kafka.trainning.model.model;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;

namespace io.quind.kafka.trainning.kafka.configuration
{
    public class KafkaProducer
    {
        private readonly KafkaConfig config;
        private readonly IProducer<string, string> producer;
        private readonly ISubject<string> subject;

        public KafkaProducer(KafkaConfig config)
        {
            this.config = config;
            this.producer = new ProducerBuilder<string,string>(config.GetProducerConfig()).Build();
            this.subject = new ReplaySubject<string>(1);
        }

        public IObservable<string> ProducerEvent<T>(T data, Func<T, string> keySelector, string topic)
        {
            Task.Run(async () => { await StartProducing(data,keySelector, topic); });
            return subject.AsObservable();
        }

        private async Task StartProducing<T>(T data,Func<T,string> keySelector, string topic)
        {
            try
            {
                var dataJson = JsonSerializer.Serialize(data);
                var message = new Message<string, string>
                {
                    Key = keySelector(data),
                    Value = dataJson.ToString()
                };
                var deliveryResult = await producer.ProduceAsync(topic, message);

                Console.WriteLine($"Message send: {deliveryResult.TopicPartitionOffset}");
                subject.OnNext($"Message send to {topic} with Id {message.Key}");
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Error to send message:{e.Error.Reason}");
                subject.OnError(e);
            }
        }

        public void Close()
        {
            producer.Flush();
            producer.Dispose();
        }
    }
}
