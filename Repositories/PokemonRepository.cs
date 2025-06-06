using System.Net.Http.Json;
using msusersgraphql.Models.Dtos;
namespace msusersgraphql.Repositories;

public class PokemonRepository : IPokemonRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PokemonRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PokemonDto> GetPokemonByIdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("PokeAPI");
        var response = await client.GetFromJsonAsync<PokemonDto>($"pokemon/{id}");
        return response;
    }

    public async Task<PokemonDto> GetPokemonByNameAsync(string name)
    {
        var client = _httpClientFactory.CreateClient("PokeAPI");
        var response = await client.GetFromJsonAsync<PokemonDto>($"pokemon/{name.ToLower()}");
        return response;
    }

    public async Task<PokemonListDto> GetPokemonsAsync(int limit = 20, int offset = 0)
    {
        var client = _httpClientFactory.CreateClient("PokeAPI");
        var response = await client.GetFromJsonAsync<PokemonListDto>($"pokemon?limit={limit}&offset={offset}");
        return response;
    }
}