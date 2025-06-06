using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace msusersgraphql.Models.GraphQL
{
    public class UserSchema : Schema
    {
        public UserSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<UserQuery>();
            Description = "Users Management GraphQL API schema";
        }
    }
}