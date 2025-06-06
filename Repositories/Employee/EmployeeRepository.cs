using System.Net.Http.Json;
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.User
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HttpClient _httpClient;

        public EmployeeRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UsersAPI");
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(string id)
        {
            try
            {
                var url = $"employee/get-filter?idUser={id}";
                Console.WriteLine($"Calling Employee by User URL: {_httpClient.BaseAddress}{url}");

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"No employee found for user {id}");
                    return null;
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<EmployeeDataDto>>();
                if (apiResponse?.Data?.Items?.Any() == true)
                {
                    return apiResponse.Data.Items.First();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeeByUserIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<EmployeeListDto> GetEmployeesAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var url = $"employee/get-filter?pageNumber={pageNumber}&pageSize={pageSize}";
                Console.WriteLine($"Calling Employee List URL: {_httpClient.BaseAddress}{url}");

                var response = await _httpClient.GetAsync(url);

                Console.WriteLine($"Employee List Response Status: {response.StatusCode}");
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Employee List Response Content: {content.Substring(0, Math.Min(200, content.Length))}...");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // Deserializar la estructura anidada
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<EmployeeListDto>>();
                if (apiResponse?.Data == null)
                {
                    throw new InvalidOperationException("Failed to deserialize employee response or no employees found.");
                }

                // Convertir a la estructura esperada por GraphQL
                return new EmployeeListDto
                {
                    Data = apiResponse.Data.Data,
                    TotalCount = apiResponse.Data.TotalCount,
                    PageNumber = apiResponse.Data.PageNumber,
                    PageSize = apiResponse.Data.PageSize
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeesAsync: {ex.Message}");
                throw;
            }
        }
    }
}
