using GraphQL.Types;
using myapp.Models.Dtos; // Assuming your UserDto is here
using myapp.Services; // Assuming you have an employee service

namespace myapp.Models.GraphQL.Types
{
    public class UserType : ObjectGraphType<UserDto>
    {
        public UserType(IEmployeeService employeeService) // Inject employee service
        {
            Field(u => u.id);
            Field(u => u.idUserIDCS);
            Field(u => u.avatarPath);
            Field(u => u.userName);
            Field(u => u.creationDate);
            Field(u => u.state);
            Field<ListGraphType<RoleType>>("roles"); // Assuming a UserDto has a list of RoleDto

            // Add a field to resolve employee details for each user
            Field<EmployeeType>("employee",
                resolve: context =>
                {
                    var user = context.Source;
                    return employeeService.GetEmployeeByUserId(user.id); // Call your employee service
                });
        }
    }
}
