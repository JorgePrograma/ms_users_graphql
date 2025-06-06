using myapp.Models.Dtos;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace myapp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HttpClient _httpClient;

        public EmployeeRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmployeeDto> GetEmployeeFromApiByUserId(string userId)
        {
            // Construct the URL with the user ID parameter
            var url = $"https://ib3m6t7bp7sjmglwvvpg3xrmzu.apigateway.sa-bogota-1.oci.customer-oci.com/api/v1/employee/get-filter?idUser={userId}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            // Deserialize the response, handling the outer structure and the list of items
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<EmployeeListResponse>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assuming the employee data is in the 'items' list and you expect only one employee
            return apiResponse.data.items.FirstOrDefault();
        }
    }

    // Helper class to match the structure of the employee list response
    public class EmployeeListResponse
    {
        public List<EmployeeDto> items { get; set; }
        public int totalCount { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}