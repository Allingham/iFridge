using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FridgeModels;
using Microsoft.EntityFrameworkCore;

namespace iFridgeAPI.Data
{
    public class V2DataContext :DbContext
    {
        public V2DataContext(DbContextOptions<V2DataContext> options) : base(options) { }

        public DbSet<Category> Category{ get; set; }
        public DbSet<SubCategory> SubCategory{ get; set; }
        public DbSet<Product> Product{ get; set; }
        public DbSet<ProductInstance> ProductInstance{ get; set; }

        
    }
}
