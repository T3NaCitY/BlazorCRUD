using Microsoft.AspNetCore.Components;

namespace BlazorCRUD.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient http;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly NavigationManager navManager;

        public OrderService(HttpClient http, AuthenticationStateProvider authStateProvider, NavigationManager navManager)
        {
            this.http = http;
            this.authStateProvider = authStateProvider;
            this.navManager = navManager;
        }

        public async Task<OrderDetailsResponse> GetOrderDetails(int orderId)
        {
            var result = await http.GetFromJsonAsync<ServiceResponse<OrderDetailsResponse>>($"api/order/{orderId}");
            return result.Data;
        }

        public async Task<List<OrderOverviewResponse>> GetOrders()
        {
            var result = await http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponse>>>("api/order");
            return result.Data;
        }

        public async Task PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                await http.PostAsync("api/order", null);
            }

            else
            {
                navManager.NavigateTo("login");
            }
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
