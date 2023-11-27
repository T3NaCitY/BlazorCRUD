using BlazorCRUD.Shared;

namespace BlazorCRUD.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        string message { get; set;}
        int currentPage { get; set; }
        int pageCount { get; set; }
        string LastSearchText { get; set; }
        Task GetProducts(string? categoryURL = null);

        Task<ServiceResponse<Product>> GetProduct(int productId);
        Task SearchProducts(string? searchText, int page);
        Task<List<string>> ProductSearchSuggestions(string searchText);
    }
}
