using GraphQL.Types;
using msusersgraphql.Models.Dtos;
using myapp.Models.Dtos;
using myapp.Models.GraphQL.Types;

namespace msusersgraphql.Models.GraphQL.Types;


public class UserType : ObjectGraphType<UserDto>
{
    public UserType()
    {
        Name = "User";
        Description = "A user creature";

        Field(x => x.id).Description("The ID of the Pokémon");
        Field(x => x.idUserIDCS).Description("The name of the Pokémon");
        Field(x => x.userName).Description("The height of the Pokémon in decimeters");
        Field(x => x.avatarPath).Description("The weight of the Pokémon in hectograms");
        Field(x => x.creationDate).Description("The weight of the Pokémon in hectograms");
        Field(x => x.roles).Description("The weight of the Pokémon in hectograms");
        Field(x => x.state).Description("The weight of the Pokémon in hectograms");
        
        Field<ListGraphType<UserType>>("users")
            .Description("The types of the Pokémon");
            
        Field<EmployeeType>("sprites")
            .Description("The sprites of the Pokémon");
            }
}

public class UserTypeType : ObjectGraphType<PokemonTypeDto>
{
    public UserTypeType()
    {
        Name = "PokemonType";
        Field(x => x.Name).Description("The name of the type");
    }
}

public class EmployeeType : ObjectGraphType<EmployeeDto>
{
    public EmployeeType()
    {
        Name = "PokemonSprites";
        Field(x => x.bussinesEmail).Description("Default front sprite");
        Field(x => x.bussinesPhone).Description("Shiny front sprite");
    }
}

public class UserListType : ObjectGraphType<PokemonListDto>
{
    public UserListType()
    {
        Name = "UserList";
        Description = "A list of Pokémon with pagination info";
        
        Field(x => x.Count).Description("Total count of Pokémon");
        Field(x => x.Next).Description("URL for next page");
        Field(x => x.Previous).Description("URL for previous page");
        
        Field<ListGraphType<PokemonListItemType>>("results")
            .Resolve(context => context.Source.Results)
            .Description("List of Pokémon");
    }
}
