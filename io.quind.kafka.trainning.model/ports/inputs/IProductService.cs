using io.quind.kafka.trainning.model.model;

namespace io.quind.kafka.trainning.model.ports.inputs
{
    public interface IProductService
    {
        Task<int> Create(Product product);
        Task<Product> GetProductById(int id);
        Task<List<Product>> GetProducts();
        Task<Product> RegisterSale(Product product);
        Task<bool> IsLowStock(Product product);
    }
}
