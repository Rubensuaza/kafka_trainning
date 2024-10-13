using io.quind.kafka.trainning.model.model;

namespace io.quind.kafka.trainning.model.ports.outputs
{
    public interface IProductRepository
    {
        Task<int> Save(Product product);
        Task<Product> FindById(int id);
        Task<List<Product>> FindAll();
        Task<int> Update(Product product);
    }
}
