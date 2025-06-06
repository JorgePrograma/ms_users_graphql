using System.Net.Http.Json;
using System.Text;
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.User
{
    public class ContactRepository : IContactRepository
    {
        private readonly HttpClient _httpClient;

        public ContactRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UsersAPI");
        }

        public async Task<ContactDto> GetContactByIdPersonAsync(string id)
        {
            try
            {
                var url = $"contact-details/get-by-filter?idPerson={id}";
                Console.WriteLine($"Request URL: {url}"); // Log URL

                var response = await _httpClient.GetAsync(url);
                Console.WriteLine($"HTTP Status Code: {response.StatusCode}"); // Log status code

                // Leer el contenido como string para debuggear
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw API Response: {responseContent}"); // Log raw response

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: HTTP request failed with status {response.StatusCode}");
                    return null;
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDtoList<ContactDto>>();
                
                if (apiResponse?.Data != null && apiResponse.Data.Any())
                {
                    Console.WriteLine($"Deserialized Data: {System.Text.Json.JsonSerializer.Serialize(apiResponse.Data)}"); // Log deserialized data
                    return apiResponse.Data.FirstOrDefault();
                }
                else
                {
                    Console.WriteLine("Warning: API response data is null or empty");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetContactByIdPersonAsync: {ex.ToString()}"); // Log full exception
                return null;
            }
        }
    }
}