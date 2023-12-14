using BlazorCRUD.Shared;
using System.Security.Claims;

namespace BlazorCRUD.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _Context;
        private readonly IAuthService _authService;

        public CartService(DataContext context, IAuthService authService)
        {
            _Context = context;
            _authService = authService;
        }

        

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>
            {
                Data = new List<CartProductResponse>()
            };

            foreach (var item in cartItems)
            {
                var product = await _Context.Products.Where(p => p.Id == item.ProductId).FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _Context.ProductVariants.Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId).
                    Include(v => v.ProductType).FirstOrDefaultAsync();

                if (productVariant == null)
                {
                    continue;
                }

                var cartProduct = new CartProductResponse
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    imageURL = product.ImageURL,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = item.Quantity,
                };

                result.Data.Add(cartProduct);
            }
            return result;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.userId = _authService.getUserId());
            _Context.CartItems.AddRange(cartItems);
            await _Context.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await _Context.CartItems.Where(p => p.userId == _authService.getUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts()
        {
            return await GetCartProducts(await _Context.CartItems.Where(ci => ci.userId == _authService.getUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.userId = _authService.getUserId();

            var sameItem = await _Context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId && ci.ProductTypeId == ci.ProductTypeId && _authService.getUserId() == ci.userId);
            if (sameItem == null)
            {
                _Context.CartItems.Add(cartItem);
            }

            if (sameItem != null)
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _Context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var DbcartItem = await _Context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId && cartItem.ProductTypeId == ci.ProductTypeId && _authService.getUserId() == ci.userId);
            if (DbcartItem == null)
            {
                return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist.", Success = false };
            }

            DbcartItem.Quantity = cartItem.Quantity;
            await _Context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var DbcartItem = await _Context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId && productTypeId == ci.ProductTypeId && _authService.getUserId() == ci.userId);
            if (DbcartItem == null)
            {
                return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist.", Success = false };
            }

            _Context.CartItems.Remove(DbcartItem);  
            await _Context.SaveChangesAsync();

            return new ServiceResponse<bool> {  Data = true};
        }
    }
}
