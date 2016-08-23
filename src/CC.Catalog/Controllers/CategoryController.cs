using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CC.Catalog.Models;
using CC.Catalog.Data;

namespace CC.Catalog.Controllers
{
    public class CategoryController : ControllerBase
    {
        private CatalogContext _db;

        public CategoryController(CatalogContext db)
        {
            _db = db;
        }

        [HttpGet("/categories")]
        public IEnumerable<Category> Get()
        {
            return _db.Categories.ToList();
        }
    }
}
