using GraphQL.Types;
using myapp.Models.Dtos; // Assuming your RoleDto is here

namespace myapp.Models.GraphQL.Types
{
    public class RoleType : ObjectGraphType<RoleDto>
    {
        public RoleType()
        {
            Field(r => r.id);
            Field(r => r.name);
        }
    }
}