using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories;

namespace msusersgraphql.Services;

public class PokemonService : IPokemonService
{
    private readonly IPokemonRepository _pokemonRepository;

    public PokemonService(IPokemonRepository pokemonRepository)
    {
        _pokemonRepository = pokemonRepository;
    }

    public async Task<PokemonDto> GetPokemonByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid Pokemon ID");
        }

        return await _pokemonRepository.GetPokemonByIdAsync(id);
    }

    public async Task<PokemonDto> GetPokemonByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Pokemon name cannot be empty");
        }

        return await _pokemonRepository.GetPokemonByNameAsync(name);
    }

    public async Task<PokemonListDto> GetPokemonsAsync(int limit = 20, int offset = 0)
    {
        if (limit <= 0 || offset < 0)
        {
            throw new ArgumentException("Invalid pagination parameters");
        }

        return await _pokemonRepository.GetPokemonsAsync(limit, offset);
    }
}