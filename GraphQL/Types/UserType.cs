using GraphQL.Types;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Services.User;

namespace msusersgraphql.GraphQL.Types
{
    public sealed class UserType : ObjectGraphType<UserDto>
    {
        public UserType()
        {
            Name = "User";
            Field(x => x.Id).Description("The ID of the user");
            Field(x => x.IdUserIDCS).Description("The IDCS ID of the user");
            Field(x => x.AvatarPath, nullable: true).Description("The avatar path of the user");
            Field(x => x.UserName).Description("The username of the user");
            Field(x => x.CreationDate).Description("The creation date of the user");
            Field(x => x.State).Description("The state of the user");
            Field<ListGraphType<UserRoleType>>("roles").Description("The roles of the user");

            // Campo relacionado: Empleado
            Field<EmployeeType>("employee")
                .ResolveAsync(async context =>
                {
                    var employeeService = context.RequestServices!.GetRequiredService<IEmployeeService>();
                    var user = context.Source;

                    try
                    {
                        // Buscar empleado por IdUser
                        var employees = await employeeService.GetEmployeeByIdAsync(user.Id);
                        return employees;
                    }
                    catch
                    {
                        return null; // Si no encuentra empleado, devolver null
                    }
                })
                .Description("Employee data for this user");

            // Campo relacionado: Persona (a través del empleado)
            Field<PersonType>("person")
                .ResolveAsync(async context =>
                {
                    var personService = context.RequestServices!.GetRequiredService<IPersonService>();
                    var employeeService = context.RequestServices!.GetRequiredService<IEmployeeService>();
                    var user = context.Source;

                    try
                    {
                        // Primero buscar el empleado para obtener el IdPerson
                        var employees = await employeeService.GetEmployeesAsync(1, 1000);
                        var employee = employees.Data.FirstOrDefault(e => e.IdUser == user.Id);

                        if (employee?.IdPerson != null)
                        {
                            return await personService.GetPersonByIdAsync(employee.IdPerson);
                        }
                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Description("Person data for this user");
        }
    }

    public sealed class UserRoleType : ObjectGraphType<UserRoleDto>
    {
        public UserRoleType()
        {
            Name = "UserRole";
            Field(x => x.Id).Description("The ID of the role");
            Field(x => x.Name).Description("The name of the role");
        }
    }

    public sealed class UserListType : ObjectGraphType<UserListDto>
    {
        public UserListType()
        {
            Name = "UserList";
            Field(x => x.TotalCount).Description("Total count of users");
            Field(x => x.PageNumber).Description("Current page number");
            Field(x => x.PageSize).Description("Page size");
            Field<ListGraphType<UserType>>("data")
                .Description("List of users")
                .Resolve(context => context.Source.Data);

        }
    }
}