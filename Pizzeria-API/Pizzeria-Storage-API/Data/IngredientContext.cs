using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pizzeria_Storage_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria_Storage_API.Data
{
    public class IngredientContext : DbContext

    {
        IConfiguration Configuration { get; set; }
        public IngredientContext()
        {

        }
        public IngredientContext(DbContextOptions<IngredientContext> options, IConfiguration configuration ) : base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<IngredientItem> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder  optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
