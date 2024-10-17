using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace io.quind.kafka.trainning.kafka.configuration
{
    public class KafkaConsumer<T> 
    {

        private readonly KafkaConfig kafkaConfig;
        private readonly ISubject<T> subject;
        private readonly ILogger<KafkaConsumer<T>> logger;


        public KafkaConsumer(KafkaConfig kafkaConfig, ISubject<T> subject,ILogger<KafkaConsumer<T>> logger)
        {
            this.kafkaConfig = kafkaConfig;
            this.subject = new ReplaySubject<T>(1);
            this.logger=logger;
        }

        public IObservable<T> ConsumerEvent(string topic, Func<string, T> deserializer)
        {
            Task.Run(() => StartConsuming(topic,deserializer));
            return subject.AsObservable();
        }

        private async Task StartConsuming(string topic,Func<string,T> deserializer)
        {
            using var consumer = new ConsumerBuilder<string, string>(kafkaConfig.GetConsumerConfig()).Build();
            consumer.Subscribe(topic);

            try
            {
                while (true)
                {
                    var consumerResult = consumer.Consume();
                    T? data= default;
                    try
                    {
                        data = deserializer(consumerResult.Message.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"error to deserializer JSON: {ex.Message}");
                    }

                    if (data != null)
                    {
                        subject.OnNext(data);
                    }

                    await Task.Delay(100);

                }
            }
            catch (ConsumeException e)
            {
                subject.OnError(new Exception($"error to consume mesage: {e.Error.Reason}"));
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
