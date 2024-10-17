using io.quind.kafka.trainning.kafka.configuration;
using io.quind.kafka.trainning.model.exceptions;
using io.quind.kafka.trainning.model.model;
using io.quind.kafka.trainning.model.ports.inputs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace io.quind.kafka.trainning.kafka.events
{
    public class SaleHandler:BackgroundService
    {
        private readonly KafkaConsumer<Product> consumer;
        private readonly KafkaProducer producer;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<SaleHandler> logger;

        public SaleHandler(
            KafkaConsumer<Product> consumer,
            KafkaProducer producer,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<SaleHandler> logger)
        {
            this.consumer = consumer;
            this.producer = producer;
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger=logger;
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
                            .Subscribe(message => logger.LogInformation(message),
                                        error => logger.LogError($"Error producer message: {error.Message}"));
                        }
                    }
                    catch (ProductException) 
                    {
                        producer.ProducerEvent(product, p => p.Id.ToString(),"insufficient_stock-topic")
                        .Subscribe(message => logger.LogInformation(message),
                                    error => logger.LogError($"Error producer message: {error.Message}"));
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
            logger.LogError($"Error al procesar el mensaje: {ex.Message}");
        }

    }
}
