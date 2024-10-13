using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.kafka.configuration
{
    public class KafkaConfig
    {
        public ConsumerConfig GetConsumerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers="localhost:9092",
                GroupId="trainnning-consumer-group",
                AutoOffsetReset=AutoOffsetReset.Earliest
            };
        }

        public ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All
            };
        }
    }
}
