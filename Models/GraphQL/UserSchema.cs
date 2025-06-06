using GraphQL;
using GraphQL.Types;
using msusersgraphql.Models.GraphQL.Types;
namespace msusersgraphql.Models.GraphQL;

public class UserSchema : Schema
{
    public UserSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<UserQuery>();
        Description = "User GraphQL API schema";
    }
}