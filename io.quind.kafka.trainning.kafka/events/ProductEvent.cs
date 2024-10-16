using Confluent.Kafka;
using io.quind.kafka.trainning.kafka.configuration;
using io.quind.kafka.trainning.model.exceptions;
using io.quind.kafka.trainning.model.model;
using io.quind.kafka.trainning.model.ports.inputs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.kafka.events
{
    public class ProductEvent:BackgroundService
    {
        private readonly KafkaConsumer<Product> consumer;
        private readonly KafkaProducer producer;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ProductEvent(KafkaConsumer<Product> consumer, KafkaProducer producer, IServiceScopeFactory serviceScopeFactory)
        {
            this.consumer = consumer;
            this.producer = producer;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            consumer.ConsumerEvent("sales-topic",json=>JsonSerializer.Deserialize<Product>(json))
                .Subscribe(async product =>
                {
                    try
                    {
                        var productInventory = await OnNextSaleReceived(product);
                        if (await OnNextSaleValidateStock(productInventory))
                        {
                            producer.ProducerEvent(productInventory, p => p.Id.ToString(), "low_stock-topic")
                            .Subscribe(message => Console.WriteLine(message),
                                        error => Console.WriteLine($"Error producer message: {error.Message}"));
                        }
                    }
                    catch (ProductException ex) 
                    {
                        producer.ProducerEvent(product, p => p.Id.ToString(),"insufficient_stock-topic")
                        .Subscribe(message => Console.WriteLine(message),
                                    error => Console.WriteLine($"Error producer message: {error.Message}"));
                    }
                },
                error => OnErrorReceived(error)
                );
            

            return Task.CompletedTask;
        }

        private async Task<Product> OnNextSaleReceived(Product product)
        {

            using (var scope = serviceScopeFactory.CreateScope()) 
            {
                var prodcutService = scope.ServiceProvider.GetRequiredService<IProductService>();
                return await prodcutService.RegisterSale(product);

            }          

        }

        private async Task<bool> OnNextSaleValidateStock(Product product)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var prodcutService = scope.ServiceProvider.GetRequiredService<IProductService>();
                return await prodcutService.IsLowStock(product);

            }
        }

        private void OnErrorReceived(Exception ex)
        {
            // Manejar el error como prefieras (log, etc.)
            Console.WriteLine($"Error al procesar el mensaje: {ex.Message}");
        }

    }
}
