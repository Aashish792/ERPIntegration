using ERPIntegration.Models;
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

        // Static full URL for the customer list (do not modify at runtime).
        private static readonly string customersUrl = "https://backend.barcodefactory.com/Acumatica_DB_TEST/entity/Default/23.200.001/Customer?$select=CustomerClass,CustomerID,CustomerName&$top=5";

        // API endpoint URL for token retrieval.
        private readonly string tokenUrl = "https://backend.barcodefactory.com/Acumatica_DB_TEST/identity/connect/token";

        // Credentials (store these securely in production)
        private readonly string clientId = "69C45D2E-D739-4A55-54CB-E46A439FAE87@Company";
        private readonly string clientSecret = "zIwzuGP2Pb4O3-KEhSb69w";
        private readonly string username = "admin";
        private readonly string password = "PARprint141__";
        private readonly string grantType = "password";
        private readonly string scope = "api";

        public ErpIntegrationService(HttpClient client)
        {
            _client = client;
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

            TokenResponse? tokenResponse = JsonSerializer.Deserialize<TokenResponse>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.access_token))
                throw new Exception("Access token not found in the response.");

            return tokenResponse.access_token;
        }

        public async Task<string> GetCustomersJsonAsync(string token)
        {
            // Set authorization header using the token.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Add cookie header as seen in Postman tests.
            _client.DefaultRequestHeaders.Add("Cookie", "Locale=Culture=en-US&TimeZone=GMTM0500G; UserBranch=1");

            var response = await _client.GetAsync(customersUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Customer request failed: " + response.ReasonPhrase);

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw customer response: " + result);

            // Return the raw JSON string directly.
            return result;
        }
    }
}
