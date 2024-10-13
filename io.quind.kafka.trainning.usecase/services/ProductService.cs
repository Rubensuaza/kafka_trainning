using io.quind.kafka.trainning.model.exceptions;
using io.quind.kafka.trainning.model.model;
using io.quind.kafka.trainning.model.ports.inputs;
using io.quind.kafka.trainning.model.ports.outputs;
using io.quind.kafka.trainning.usecase.usecases.validations;

namespace io.quind.kafka.trainning.usecase.services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;

        public ProductService(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Create(Product product)
        {
           return await repository.Save(product);
        }

        public async Task<Product> GetProductById(int id)
        {
            return await repository.FindById(id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await repository.FindAll();
        }

        public async Task<bool> IsLowStock(Product product)
        {
            return await Task.Run(() =>  product.Quantity <= product.OrderPoint);
        }

        public async Task<Product> RegisterSale(Product product)
        {
            Product productInventory = await repository.FindById(product.Id);
            if (productInventory != null)
            {
                productInventory.Quantity -= product.Quantity;
                ProductValidation.Validate(productInventory);
                await repository.Update(productInventory);
                return productInventory;
            }
            throw new ProductException($"Product Id {product.Id} Not exist");
        }
    }
}
