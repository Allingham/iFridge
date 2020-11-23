using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FridgeModels;
using Microsoft.EntityFrameworkCore;

namespace iFridgeAPI.Data
{
    public class ContextWaretype : DbContext
    {


        public ContextWaretype(DbContextOptions<ContextWaretype> options) : base(options) { }

        public DbSet<Waretype> Waretypes { get; set; }




    }
}
