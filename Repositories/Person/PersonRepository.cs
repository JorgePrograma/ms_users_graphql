using System.Net.Http.Json;
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.User
{
    public class PersonRepository : IPersonRepository
    {
        private readonly HttpClient _httpClient;

        public PersonRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UsersAPI");
        }

        public async Task<PersonDto> GetPersonByIdAsync(string id)
        {
            try
            {
                var url = $"employee/get-filter?idFilter={id}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<PersonDataDto>>();
                if (apiResponse?.Data?.Items?.Any() == true)
                {
                    return apiResponse.Data.Items.First();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPersonByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<PersonListDto> GetPersonsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var url = $"person/get-filter?pageNumber={pageNumber}&pageSize={pageSize}";
                Console.WriteLine($"Calling Person List URL: {_httpClient.BaseAddress}{url}");

                var response = await _httpClient.GetAsync(url);

                Console.WriteLine($"Person List Response Status: {response.StatusCode}");
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Person List Response Content: {content.Substring(0, Math.Min(200, content.Length))}...");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // Deserializar la estructura anidada
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<PersonDataDto>>();
                if (apiResponse?.Data == null)
                {
                    throw new InvalidOperationException("Failed to deserialize person response or no persons found.");
                }

                // Convertir a la estructura esperada por GraphQL
                return new PersonListDto
                {
                    Data = apiResponse.Data.Items,
                    TotalCount = apiResponse.Data.TotalCount,
                    PageNumber = apiResponse.Data.PageNumber,
                    PageSize = apiResponse.Data.PageSize
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPersonsAsync: {ex.Message}");
                throw;
            }
        }
    }
}
