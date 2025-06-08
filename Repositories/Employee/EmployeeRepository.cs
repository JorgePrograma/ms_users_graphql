using System.Net.Http.Json;
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.Employee
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

    }
}
