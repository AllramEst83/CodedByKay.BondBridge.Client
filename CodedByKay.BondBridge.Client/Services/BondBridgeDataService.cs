using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CodedByKay.BondBridge.Client.Services
{
    /// <summary>
    /// Service for retrieving data from the Bond Bridge API.
    /// </summary>
    public class BondBridgeDataService : IBondBridgeDataService
    {
        private readonly IUserSecureStorageService _userSecureStorageService;
        private readonly ApplicationSettings _applicationSettings;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the BondBridgeDataService class.
        /// </summary>
        /// <param name="userSecureStorageService">The service for secure user data storage.</param>
        /// <param name="httpClientFactory">The factory for creating HttpClient instances.</param>
        /// <param name="applicationSettings">The application settings.</param>
        public BondBridgeDataService(
            IUserSecureStorageService userSecureStorageService,
            IHttpClientFactory httpClientFactory,
            IOptions<ApplicationSettings> applicationSettings)
        {
            _userSecureStorageService = userSecureStorageService;
            _applicationSettings = applicationSettings.Value;

            _httpClient = httpClientFactory.CreateClient("BondBridgeClient");
        }

        /// <summary>
        /// Retrieves data of the specified type from the specified URI.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <param name="uri">The URI from which to retrieve the data.</param>
        /// <returns>The retrieved data as an object of the specified type.</returns>
        public async Task<T> GetData<T>(string uri) where T : class
        {
            var userSecrets = await _userSecureStorageService.GetAsync<UserSecrets>(_applicationSettings.UserSecureStorageKey);
            if (userSecrets == null)
            {
                throw new InvalidOperationException("UserSecrets cannot be null.");
            }

            try
            {
                // Create a new HttpRequestMessage
                using var request = new HttpRequestMessage(HttpMethod.Get, uri);
                // Add the Authorization header with the Bearer token
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userSecrets.AccessToken);

                // Perform the GET request
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                // Ensure success status code
                response.EnsureSuccessStatusCode();

                // Read the content as a string
                string content = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                T result = JsonSerializer.Deserialize<T>(content, options) ?? throw new InvalidOperationException("Deserialization resulted in null.");

                return result;
            }
            catch (HttpRequestException e)
            {
                // Log the error or handle it as needed
                Console.WriteLine($"Error fetching data: {e.Message}");
                throw;
            }
        }
    }
}
