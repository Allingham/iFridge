using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FridgeModels;
using Microsoft.EntityFrameworkCore;

namespace iFridgeAPI.Data
{
    public class ContextWare : DbContext
    {
        public ContextWare(DbContextOptions<ContextWare> options) : base(options) { }

        public DbSet<Ware> Wares { get; set; }




    }
}
