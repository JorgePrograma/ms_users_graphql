namespace msusersgraphql.Models.Dtos
{
    public class EmployeeDto
    {
        public string? Id { get; set; }
        public string? IdPerson { get; set; }
        public string? IdUser { get; set; }
        public string? BussinesEmail { get; set; }
        public string? BussinesPhone { get; set; }
        public string? IdPosition { get; set; }
        public string? IdBranch { get; set; }
    }

    public class EmployeeListDto
    {
        public List<EmployeeDto> Data { get; set; } = new List<EmployeeDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class EmployeeDataDto
    {
        public List<EmployeeDto> Items { get; set; } = new List<EmployeeDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}