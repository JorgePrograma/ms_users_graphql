using GraphQL.Types;
using myapp.Models.Dtos; // Assuming your EmployeeDto is here

namespace myapp.Models.GraphQL.Types
{
    public class EmployeeType : ObjectGraphType<EmployeeDto>
    {
        public EmployeeType()
        {
            Field(e => e.id);
            Field(e => e.idPerson);
            Field(e => e.idUser);
            Field(e => e.bussinesEmail);
            Field(e => e.bussinesPhone);
            Field(e => e.idPosition);
            Field(e => e.idBranch);
        }
    }
}