using GraphQL;
using GraphQL.Types;
using msusersgraphql.Services;
namespace msusersgraphql.Models.GraphQL.Types;

public class PokemonQuery : ObjectGraphType
{
    public PokemonQuery(IPokemonService pokemonService)
    {
        Name = "Query";
        
        Field<PokemonType>(
            "pokemon",
            arguments: new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id", Description = "ID of the Pokémon" },
                new QueryArgument<StringGraphType> { Name = "name", Description = "Name of the Pokémon" }
            ),
            resolve: context =>
            {
                var id = context.GetArgument<int?>("id");
                var name = context.GetArgument<string>("name");
                
                if (id.HasValue)
                {
                    return pokemonService.GetPokemonByIdAsync(id.Value);
                }
                
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return pokemonService.GetPokemonByNameAsync(name);
                }
                
                context.Errors.Add(new ExecutionError("You must provide either an id or name"));
                return null;
            },
            description: "Get a Pokémon by ID or name"
        );
        
        Field<PokemonListType>(
            "pokemons",
            arguments: new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "limit", DefaultValue = 20, Description = "Number of items per page" },
                new QueryArgument<IntGraphType> { Name = "offset", DefaultValue = 0, Description = "Pagination offset" }
            ),
            resolve: context =>
            {
                var limit = context.GetArgument<int>("limit");
                var offset = context.GetArgument<int>("offset");
                
                return pokemonService.GetPokemonsAsync(limit, offset);
            },
            description: "Get a list of Pokémon with pagination"
        );
    }
}