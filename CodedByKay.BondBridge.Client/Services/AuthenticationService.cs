using CodedByKay.BondBridge.Client.Exceptions;
using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace CodedByKay.BondBridge.Client.Services
{
    /// <summary>
    /// Service for handling authentication-related operations.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the AuthenticationService class.
        /// </summary>
        /// <param name="httpClientFactory">The factory for creating HttpClient instances.</param>
        /// <param name="applicationSettings">The application settings.</param>
        public AuthenticationService(
            IHttpClientFactory httpClientFactory,
            IOptions<ApplicationSettings> applicationSettings)
        {
            _applicationSettings = applicationSettings.Value;
            _httpClient = httpClientFactory.CreateClient("BondBridgeClient");
        }

        /// <summary>
        /// Adds a new user with the specified email and password.
        /// </summary>
        /// <param name="email">The email of the user to add.</param>
        /// <param name="password">The password of the user to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddUser(string email, string password)
        {
            // Prepare the request content
            var requestData = new
            {
                email = email,
                password = password
            };

            string requestJson = JsonSerializer.Serialize(requestData);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            try
            {
                // Send the POST request
                string requestUri = "/api/usermanager/adduser";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _applicationSettings.AppUserRegistrationToken);

                HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);

                // Ensure success status code
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // Unauthorized access - wrong username or password
                        throw new UnauthorizedAccessException("Oopss! Bond Bridge verkar ha lite störningar med sina tjänster i molnet. Försök igen lite senare tack.");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new BadRequestException("Oopss! Vänligen fyll i en korrekt email adress eller ett korrekt lösnord!");
                    }
                    else
                    {
                        // Other errors
                        throw new Exception($"Sign-in failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                // Consider logging the exception details
                throw;
            }
            catch (JsonException e)
            {
                // Handle JSON deserialization errors
                throw new Exception("Error deserializing the authentication response.", e);
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        /// <summary>
        /// Attempts to sign in the user with the specified email and password.
        /// </summary>
        /// <param name="email">The email of the user to sign in.</param>
        /// <param name="password">The password of the user to sign in.</param>
        /// <returns>An authentication model representing the signed-in user.</returns>
        public async Task<AuthModel> SignInAsync(string email, string password)
        {
            // Prepare the request content
            var requestData = new
            {
                email = email,
                password = password
            };
            string requestJson = JsonSerializer.Serialize(requestData);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            try
            {
                // Send the POST request
                string requestUri = "/api/Authentication/signin";
                HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);

                // Handle non-success status code
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // Unauthorized access - wrong username or password
                        throw new UnauthorizedAccessException("Oopss! Fel email eller lösenord. Försök igen!");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new BadRequestException("Oopss! Vänligen fyll i en korrekt email adress eller ett korrekt lösnord!");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new NotFoundException("Oopss! Användaren hittades tyvärr inte!");
                    }
                    else
                    {
                        // Other errors
                        throw new Exception($"Sign-in failed with status code: {response.StatusCode}");
                    }
                }

                // Read the response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response content to AuthModel
                JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                AuthModel? authModel = JsonSerializer.Deserialize<AuthModel>(responseContent, options);

                if (authModel == null)
                {
                    throw new Exception("Failed to deserialize the authentication response.");
                }

                return authModel;
            }
            catch (HttpRequestException e)
            {
                // Consider logging the exception details
                throw;
            }
            catch (JsonException e)
            {
                // Handle JSON deserialization errors
                throw new Exception("Error deserializing the authentication response.", e);
            }
        }

    }
}
