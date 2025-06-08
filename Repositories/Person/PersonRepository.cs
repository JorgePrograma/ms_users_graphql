using System.Net.Http.Json;
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories.Person
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
                var url = $"person/get-filter?idFilter={id}";

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
    }
}
