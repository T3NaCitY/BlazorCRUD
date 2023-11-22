using BlazorCRUD.Shared;

namespace BlazorCRUD.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        Task GetProducts(string? categoryURL = null);

        Task<ServiceResponse<Product>> GetProduct(int productId);
    }
}
