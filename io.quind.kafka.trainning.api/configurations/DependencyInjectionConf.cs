using io.quind.kafka.trainning.kafka.configuration;
using io.quind.kafka.trainning.kafka.events;
using io.quind.kafka.trainning.model.model;
using io.quind.kafka.trainning.model.ports.inputs;
using io.quind.kafka.trainning.model.ports.outputs;
using io.quind.kafka.trainning.repository.repositories;
using io.quind.kafka.trainning.usecase.services;
using System.Reactive.Subjects;

namespace io.quind.kafka.trainning.api.configurations
{
    public class DependencyInjectionConf
    {
        public static void DependencyInjectionConfServices(IServiceCollection services)
        {
            services.AddTransient<IProductRepository,ProductRepository>();            
            services.AddTransient<IProductService, ProductService>();

            services.AddSingleton<ISubject<Product>, Subject<Product>>();
            services.AddSingleton<KafkaConfig>();
            services.AddSingleton<KafkaConsumer<Product>>();
            services.AddSingleton<KafkaProducer>();


            services.AddHostedService<ProductEvent>();
        }
    }
}
