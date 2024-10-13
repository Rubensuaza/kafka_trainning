using io.quind.kafka.trainning.model.model;
using io.quind.kafka.trainning.model.ports.outputs;
using io.quind.kafka.trainning.repository.config;
using io.quind.kafka.trainning.repository.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.repository.repositories
{
    public class ProductRepository(DatabaseContext databaseContext) : IProductRepository
    {
        public async Task<List<Product>> FindAll()
        {
           return await databaseContext
                .Products
                .Select(product=>product.ToProduct()).ToListAsync();           
        }
            
        public async Task<Product> FindById(int id)
        {
            try
            {
                var product = await databaseContext.Products.FindAsync(id);
                return product?.ToProduct();
            }
            catch (Exception) { throw; }
        }

        public async Task<int> Save(Product product)
        {
            try
            {
                var productEntity = ProductEntity.From(product);
                await databaseContext.Products.AddAsync(productEntity);
                await databaseContext.SaveChangesAsync();
                return productEntity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Update(Product product)
        {
            try
            {
                var productDb = await databaseContext.Products.FindAsync(product.Id);
                if (productDb != null)
                {
                    productDb.Quantity = product.Quantity;
                    await Task.Run(() => databaseContext.Products.Update(productDb));
                    await databaseContext.SaveChangesAsync();
                }
                return product.Id;
            }
            catch (Exception ) 
            {
                throw;
            }
        }
    }
}
