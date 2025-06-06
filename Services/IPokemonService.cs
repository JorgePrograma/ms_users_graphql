
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Services;

public interface IPokemonService
{
    Task<PokemonDto> GetPokemonByIdAsync(int id);
    Task<PokemonDto> GetPokemonByNameAsync(string name);
    Task<PokemonListDto> GetPokemonsAsync(int limit = 20, int offset = 0);
}

/**
consulta 
curl -X POST \
  http://localhost:3010/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"{ pokemon(id: 1) { id name } }"}'
**/