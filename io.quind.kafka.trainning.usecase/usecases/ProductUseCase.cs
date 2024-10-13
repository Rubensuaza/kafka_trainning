using io.quind.kafka.trainning.model.exceptions;
using io.quind.kafka.trainning.model.model;
using io.quind.kafka.trainning.model.ports.outputs;
using io.quind.kafka.trainning.usecase.usecases.validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.usecase.usecases
{
    public  class ProductUseCase
    {
        private readonly IProductRepository repository;

        public ProductUseCase(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Product> RegisterSale(Product productSale)
        {
            Product productInventory =await repository.FindById(productSale.Id);
            if (productInventory != null) { 
                productInventory.Quantity -= productSale.Quantity;
                ProductValidation.Validate(productInventory);
                await repository.Update(productInventory);
                return productInventory;
            }
            throw new ProductException($"Product Id {productSale.Id} Not exist");
            
        }
    }
}
