using io.quind.kafka.trainning.model.exceptions;
using io.quind.kafka.trainning.model.model;

namespace io.quind.kafka.trainning.usecase.usecases.validations
{
    public static class ProductValidation
    {
        public static void Validate(Product product)
        {
            ProductOutStock(product);
        }      
        private static void ProductOutStock(Product product) 
        {
            if (product.Quantity >= 0) return;
            throw new ProductException($"Product {product.Description} is out of stock ");
            
        }


    }


}
