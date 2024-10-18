using io.quind.kafka.trainning.repository.entities;
using Microsoft.EntityFrameworkCore;

namespace io.quind.kafka.trainning.repository.config
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<ProductEntity> Products { get; set; }
    }
}
