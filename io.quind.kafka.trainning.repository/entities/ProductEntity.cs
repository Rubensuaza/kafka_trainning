using io.quind.kafka.trainning.model.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.repository.entities
{
    [Table(name: "tbl_product")]
    public class ProductEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("order_point")]
        public int OrderPoint { get; set; }

        public Product ToProduct() 
        {
            return new Product
            {
                Id=this.Id,
                Description=this.Description,
                Quantity=this.Quantity,
                OrderPoint=this.OrderPoint
            };
        }

        public static ProductEntity From(Product product)
        {
            return new ProductEntity
            {
                Id=product.Id,
                Description=product.Description,
                Quantity=product.Quantity,
                OrderPoint=product.OrderPoint
            };
        }
    }
}
