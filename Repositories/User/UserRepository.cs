using System.Net.Http.Json;
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _httpClient;

        public UserRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UsersAPI");
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<UserDto>($"user/{id}");
                if (response == null)
                {
                    throw new InvalidOperationException($"User with id '{id}' was not found.");
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<UserListDto> GetUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var url = $"user/get-by-filter?pageNumber={pageNumber}&pageSize={pageSize}";
                Console.WriteLine($"Calling URL: {_httpClient.BaseAddress}{url}");

                var response = await _httpClient.GetAsync(url);

                Console.WriteLine($"Response Status: {response.StatusCode}");
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {content.Substring(0, Math.Min(200, content.Length))}...");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // Deserializar la respuesta anidada
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<UserDataDto>>();
                if (apiResponse?.Data == null)
                {
                    throw new InvalidOperationException("Failed to deserialize response or no users found.");
                }

                // Convertir a la estructura esperada por GraphQL
                return new UserListDto
                {
                    Data = apiResponse.Data.Items,
                    TotalCount = apiResponse.Data.TotalCount,
                    PageNumber = apiResponse.Data.PageNumber,
                    PageSize = apiResponse.Data.PageSize
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUsersAsync: {ex.Message}");
                throw;
            }
        }
    }
}
