using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CC.Catalog.Models;
using CC.Catalog.Data;

namespace CC.Catalog.Controllers
{
    public class ProductController : ControllerBase
    {
        private CatalogContext _db;

        public ProductController(CatalogContext db)
        {
            _db = db;
        }

        [HttpGet("/products/{productId}")]
        public Product GetProduct(int productId)
        {
            return _db.Products.Single(x=>x.Id == productId);
        }

        [HttpGet("/products")]
        public IEnumerable<Product> GetProductsByCategoryId([FromQuery]int categoryId)
        {
            return _db.Products.Where(x=>x.Category.Id == categoryId);
        }
    }
}
