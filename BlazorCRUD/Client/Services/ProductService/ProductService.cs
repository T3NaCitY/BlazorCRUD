﻿using BlazorCRUD.Shared;
using System.Net.Http.Json;

namespace BlazorCRUD.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public List<Product> Products { get ; set ; } = new List<Product>();
        public string message { get; set; } = "Loading products...";
        public int currentPage { get; set; } = 1;
        public int pageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryURL = null)
        {
            var result = categoryURL == null ? await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") 
                : await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryURL}");


            if (result != null && result.Data != null)
            Products = result.Data;

            currentPage = 1;
            pageCount = 0;

            if (Products.Count == 0)
                message = "No products found";

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> ProductSearchSuggestions(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductSearchResultResponse>>($"api/product/search/{searchText}/{page}");
            
            if(result != null && result.Data != null)
            {
                
                Products = result.Data.Products;
                currentPage = result.Data.currentPage;
                pageCount = result.Data.Pages;
            }
               

            if (Products.Count == 0)
                message = "No Products found";

           ProductsChanged?.Invoke();
        }

        
    }
}
