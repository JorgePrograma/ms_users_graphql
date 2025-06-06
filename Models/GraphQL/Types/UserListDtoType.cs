using GraphQL.Types;
using myapp.Models.Dtos; // Assuming your UserListDto is here

namespace myapp.Models.GraphQL.Types
{
    public class UserListDtoType : ObjectGraphType<UserListDto>
    {
        public UserListDtoType()
        {
            Field<ListGraphType<UserType>>("items"); // Assuming UserListDto has a list of UserDto
            Field(l => l.totalCount);
            Field(l => l.pageNumber);
            Field(l => l.pageSize);
        }
    }
}
