using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Repositories;
public interface IPokemonRepository
{
    Task<PokemonDto> GetPokemonByIdAsync(int id);
    Task<PokemonDto> GetPokemonByNameAsync(string name);
    Task<PokemonListDto> GetPokemonsAsync(int limit = 20, int offset = 0);
}