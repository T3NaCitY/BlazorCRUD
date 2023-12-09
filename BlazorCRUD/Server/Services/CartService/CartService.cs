namespace BlazorCRUD.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _Context;
        public CartService(DataContext context)
        {
            _Context = context;
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

                if(product == null) {
                    continue;
                }

                var productVariant = await _Context.ProductVariants.Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId).
                    Include(v => v.ProductType).FirstOrDefaultAsync();

                if(productVariant == null)
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

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems, int userId)
        {
            cartItems.ForEach(cartItem => cartItem.userId = userId);
            _Context.CartItems.AddRange(cartItems);
            await _Context.SaveChangesAsync();

            return await GetCartProducts(await _Context.CartItems.Where( p => p.userId == userId).ToListAsync()); 
        }
    }
}
