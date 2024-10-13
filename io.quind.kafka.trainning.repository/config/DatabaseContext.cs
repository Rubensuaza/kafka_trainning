using io.quind.kafka.trainning.repository.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.repository.config
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<ProductEntity> Products { get; set; }
    }
}
