﻿namespace BlazorCRUD.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}