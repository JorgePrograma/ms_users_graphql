using myapp.Models.Dtos;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace myapp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _httpClient;

        public UserRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserListDto> GetUsersFromApi(int pageNumber, int pageSize)
        {
            // Construct the URL with pagination parameters
            var url = $"https://ib3m6t7bp7sjmglwvvpg3xrmzu.apigateway.sa-bogota-1.oci.customer-oci.com/api/v1/user/get-by-filter?pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            // Deserialize the response, handling the outer structure
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<UserListDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return apiResponse.data; // Assuming your API response has a 'data' field
        }
    }

    // Helper class to match the outer structure of your API response
    public class ApiResponse<T>
    {
        public T data { get; set; }
        public List<object> errors { get; set; } // Adjust type based on actual error structure
        public int statusCode { get; set; }
    }
}
