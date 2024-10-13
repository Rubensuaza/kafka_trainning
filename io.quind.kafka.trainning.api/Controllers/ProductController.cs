
using io.quind.kafka.trainning.model.model;
using io.quind.kafka.trainning.model.ports.inputs;
using Microsoft.AspNetCore.Mvc;

namespace io.quind.kafka.trainning.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController:ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var id = await productService.Create(product);
            return await GetProductById(id);

        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(await productService.GetProductById(id));
            
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts() 
        {            
            return Ok(await productService.GetProducts());
        }
        
     }
}
