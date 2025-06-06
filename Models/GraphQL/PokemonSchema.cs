using GraphQL;
using GraphQL.Types;
using msusersgraphql.Models.GraphQL.Types;
namespace msusersgraphql.Models.GraphQL;

public class PokemonSchema : Schema
{
    public PokemonSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<PokemonQuery>();
        Description = "Pokémon GraphQL API schema";
    }
}