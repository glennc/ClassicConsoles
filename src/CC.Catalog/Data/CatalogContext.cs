using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CC.Catalog.Models;

namespace CC.Catalog.Data
{
    public class CatalogContext : DbContext
    {
        public DbSet<Category> Categories {get;set;}
        public DbSet<Product> Products {get;set;}

        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options)
        {
        }
    }
}
