using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC.Web.Model;
using System.Net.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace CC.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly WebSettings _settings;

        private ILogger _logger;

        private const string CATEGORIES_KEY = "All_Categories";

        //TODO: Super duplication that could be fixed in this class
        public CatalogService(IDistributedCache cache, ILogger<CatalogService> logger, IOptions<WebSettings> options)
        {
            _httpClient = new HttpClient();
            _cache = cache;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categoriesString = await _cache.GetStringAsync(CATEGORIES_KEY);

            if (!string.IsNullOrEmpty(categoriesString))
            {
                return JsonConvert.DeserializeObject<IEnumerable<Category>>(categoriesString);
            }

            var requestUrl = $"http://{_settings.CatalogBaseUrl}/categories";
            _logger.LogDebug($"Requesting Category information from {requestUrl}");
            var result = await _httpClient.GetAsync(requestUrl);

            if (!result.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Unable to find Catalog Service at {requestUrl}, recieved {result.StatusCode}");
            }

            var categoryContent = await result.Content.ReadAsStringAsync();
            _logger.LogDebug($"CategoryData: {categoryContent}");

            //TODO: Think about how long this stuff is cached for...
            await _cache.SetStringAsync(CATEGORIES_KEY, categoryContent);

            return JsonConvert.DeserializeObject<IEnumerable<Category>>(categoryContent);
        }

        public async Task<Product> GetProduct(int productId)
        {
            var key = $"Product_{productId}";
            var productString = await _cache.GetStringAsync(key);

            if (!string.IsNullOrEmpty(productString))
            {
                return JsonConvert.DeserializeObject<Product>(productString);
            }

            var requestUrl = $"http://{_settings.CatalogBaseUrl}/products/{productId}";
            var result = await _httpClient.GetAsync(requestUrl);

            if (!result.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Unable to find Catalog Service at {requestUrl}, recieved {result.StatusCode}");
            }

            var resultString = await result.Content.ReadAsStringAsync();
            await _cache.SetStringAsync(key,resultString);

            return JsonConvert.DeserializeObject<Product>(resultString);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            var key = $"CategoryProducts_{categoryId}";
            var cachedString = await _cache.GetStringAsync(key);

            if (!string.IsNullOrEmpty(cachedString))
            {
                return JsonConvert.DeserializeObject<IEnumerable<Product>>(cachedString);
            }

            var requestUrl = $"http://{_settings.CatalogBaseUrl}/products?categoryId={categoryId}";
            var result = await _httpClient.GetAsync(requestUrl);

            if (!result.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Unable to find Catalog Service at {requestUrl}, recieved {result.StatusCode}");
            }

            var resultString = await result.Content.ReadAsStringAsync();
            await _cache.SetStringAsync(key, resultString);

            return JsonConvert.DeserializeObject<IEnumerable<Product>>(resultString);
        }
    }
}
