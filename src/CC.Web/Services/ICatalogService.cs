using CC.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CC.Web.Services
{
    interface ICatalogService
    {
        Task<Product> GetProduct(int productId);
        Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId);
        Task<IEnumerable<Category>> GetCategories();
        
    }
}
