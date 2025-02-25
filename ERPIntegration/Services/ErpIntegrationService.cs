using ERPIntegration.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERPIntegration.Services
{
    public class ErpIntegrationService : IErpIntegrationService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        private readonly string tokenUrl;
        private readonly string logoutUrl;
        private readonly string customersUrl;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string username;
        private readonly string password;
        private readonly string grantType;
        private readonly string scope;

        public ErpIntegrationService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // ✅ Fix: Use null-coalescing operator (??) to provide default values
            tokenUrl = _configuration["Authentication:TokenUrl"] ?? throw new InvalidOperationException("Token URL is missing in configuration.");
            logoutUrl = _configuration["Authentication:LogoutUrl"] ?? throw new InvalidOperationException("Logout URL is missing in configuration.");
            customersUrl = _configuration["Authentication:CustomersUrl"] ?? throw new InvalidOperationException("Customers URL is missing in configuration.");
            clientId = _configuration["Authentication:ClientId"] ?? throw new InvalidOperationException("ClientId is missing in configuration.");
            clientSecret = _configuration["Authentication:ClientSecret"] ?? throw new InvalidOperationException("ClientSecret is missing in configuration.");
            username = _configuration["Authentication:Username"] ?? throw new InvalidOperationException("Username is missing in configuration.");
            password = _configuration["Authentication:Password"] ?? throw new InvalidOperationException("Password is missing in configuration.");
            grantType = _configuration["Authentication:GrantType"] ?? throw new InvalidOperationException("GrantType is missing in configuration.");
            scope = _configuration["Authentication:Scope"] ?? throw new InvalidOperationException("Scope is missing in configuration.");
        }

        public async Task<string> GetTokenAsync()
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new("client_id", clientId),
                new("client_secret", clientSecret),
                new("username", username),
                new("password", password),
                new("grant_type", grantType),
                new("scope", scope)
            };

            var content = new FormUrlEncodedContent(parameters);
            var response = await _client.PostAsync(tokenUrl, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Token request failed: " + response.ReasonPhrase);

            var result = await response.Content.ReadAsStringAsync();
            TokenResponse? tokenResponse = JsonSerializer.Deserialize<TokenResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.access_token))
                throw new Exception("Access token not found in the response.");

            return tokenResponse.access_token;
        }

        public async Task<string> GetCustomersJsonAsync(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Cookie", "Locale=Culture=en-US&TimeZone=GMTM0500G; UserBranch=1");

            var response = await _client.GetAsync(customersUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Customer request failed: " + response.ReasonPhrase);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task LogoutAsync(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync(logoutUrl, null);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Logout request failed: " + response.ReasonPhrase);
        }
    }
}
