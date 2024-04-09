using CodedByKay.BondBridge.Client.Exceptions;
using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Models;
using CodedByKay.BondBridge.Client.Models.Request;
using CodedByKay.BondBridge.Client.Models.Response;
using Microsoft.Extensions.Options;
using System.Text;
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

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Oopss! Bond Bridge verkar ha lite störningar med sina tjänster i molnet. Försök igen lite senare tack.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException("Oopss! Ett fel har inträffat. Vänligen starta om Bond Bridge.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception($"Ett oväntat fel inträffade när konversationer skulle hämtas.");
                }

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

        public async Task<CreateGroupResponse> CreateGroup(CreateGroupRequest requestData, string uri)
        {
            var userSecrets = await _userSecureStorageService.GetAsync<UserSecrets>(_applicationSettings.UserSecureStorageKey);
            if (userSecrets == null)
            {
                throw new InvalidOperationException("UserSecrets cannot be null.");
            }

            try
            {
                JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
                string json = JsonSerializer.Serialize(requestData, options);

                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = content
                };

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userSecrets.AccessToken);

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                // Handle specific status codes if necessary
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Handle unauthorized
                    throw new UnauthorizedAccessException("Unauthorized access.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Handle bad request
                    throw new BadRequestException("Bad request made to the server.");
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CreateGroupResponse>(responseContent, options);

                if (result == null)
                {
                    throw new Exception("Error group could not be created!");
                }

                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error sending data: {e.Message}");
                throw;
            }
        }

        public async Task<bool> Delete(string uri, Guid id)
        {
            var userSecrets = await _userSecureStorageService.GetAsync<UserSecrets>(_applicationSettings.UserSecureStorageKey);
            if (userSecrets == null)
            {
                throw new InvalidOperationException("UserSecrets cannot be null.");
            }

            try
            {
                // Construct the full URI including the groupId
                string fullUri = $"{uri}/{id}";

                using var request = new HttpRequestMessage(HttpMethod.Delete, fullUri);

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userSecrets.AccessToken);

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                // Handle specific status codes if necessary
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Handle unauthorized
                    throw new UnauthorizedAccessException("Unauthorized access.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Handle not found
                    throw new KeyNotFoundException("The specified group was not found.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Handle bad request
                    throw new BadRequestException("Bad request made to the server.");
                }

                return true; // Indicate success
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error deleting group: {e.Message}");
                throw;
            }
        }

    }
}
