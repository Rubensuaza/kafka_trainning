using Confluent.Kafka;
using io.quind.kafka.trainning.model.model;
using io.quind.kafka.trainning.model.ports.outputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.kafka.configuration
{
    public class KafkaConsumer<T> 
    {

        private readonly KafkaConfig kafkaConfig;
        private readonly ISubject<T> subject;
        

        public KafkaConsumer(KafkaConfig kafkaConfig, ISubject<T> subject)
        {
            this.kafkaConfig = kafkaConfig;
            this.subject = new ReplaySubject<T>(1);
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
                        Console.WriteLine($"error to deserializer JSON: {ex.Message}");
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
