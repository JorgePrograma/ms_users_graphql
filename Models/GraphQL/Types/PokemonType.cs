using GraphQL.Types;
using msusersgraphql.Models.Dtos;

namespace msusersgraphql.Models.GraphQL.Types;


public class PokemonType : ObjectGraphType<PokemonDto>
{
    public PokemonType()
    {
        Name = "Pokemon";
        Description = "A Pokémon creature";

        Field(x => x.Id).Description("The ID of the Pokémon");
        Field(x => x.Name).Description("The name of the Pokémon");
        Field(x => x.Height).Description("The height of the Pokémon in decimeters");
        Field(x => x.Weight).Description("The weight of the Pokémon in hectograms");
        
        Field<ListGraphType<PokemonTypeType>>("types")
            .Description("The types of the Pokémon");
            
        Field<PokemonSpritesType>("sprites")
            .Description("The sprites of the Pokémon");
            
        Field<ListGraphType<PokemonStatType>>("stats")
            .Description("The stats of the Pokémon");
            
        Field<ListGraphType<PokemonAbilityType>>("abilities")
            .Description("The abilities of the Pokémon");
    }
}

public class PokemonTypeType : ObjectGraphType<PokemonTypeDto>
{
    public PokemonTypeType()
    {
        Name = "PokemonType";
        Field(x => x.Name).Description("The name of the type");
    }
}

public class PokemonSpritesType : ObjectGraphType<PokemonSpritesDto>
{
    public PokemonSpritesType()
    {
        Name = "PokemonSprites";
        Field(x => x.FrontDefault).Description("Default front sprite");
        Field(x => x.FrontShiny).Description("Shiny front sprite");
    }
}

public class PokemonStatType : ObjectGraphType<PokemonStatDto>
{
    public PokemonStatType()
    {
        Name = "PokemonStat";
        Field(x => x.Name).Description("The name of the stat");
        Field(x => x.BaseStat).Description("The base value of the stat");
    }
}

public class PokemonAbilityType : ObjectGraphType<PokemonAbilityDto>
{
    public PokemonAbilityType()
    {
        Name = "PokemonAbility";
        Field(x => x.Name).Description("The name of the ability");
        Field(x => x.IsHidden).Description("Whether the ability is hidden");
    }
}

public class PokemonListType : ObjectGraphType<PokemonListDto>
{
    public PokemonListType()
    {
        Name = "PokemonList";
        Description = "A list of Pokémon with pagination info";
        
        Field(x => x.Count).Description("Total count of Pokémon");
        Field(x => x.Next).Description("URL for next page");
        Field(x => x.Previous).Description("URL for previous page");
        
        Field<ListGraphType<PokemonListItemType>>("results")
            .Resolve(context => context.Source.Results)
            .Description("List of Pokémon");
    }
}

public class PokemonListItemType : ObjectGraphType<PokemonListItemDto>
{
    public PokemonListItemType()
    {
        Name = "PokemonListItem";
        Field(x => x.Name).Description("The name of the Pokémon");
        Field(x => x.Url).Description("The URL for the Pokémon details");
    }
}