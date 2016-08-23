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

        [HttpGet("/products/{id}")]
        public Product Get(int id)
        {
            return _db.Products.Single(x=>x.Id == id);
        }

        [HttpGet("/products")]
        public IEnumerable<Product> GetByProductId([FromQuery]int categoryId)
        {
            return _db.Products.Where(x=>x.Category.Id == categoryId);
        }
    }
}
