namespace msusersgraphql.Models.Dtos
{
    public class PersonDto
    {
        public string? Id { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
    }

    public class PersonListDto
    {
        public List<PersonDto> Data { get; set; } = new List<PersonDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

     public class PersonDataDto
    {
        public List<PersonDto> Items { get; set; } = new List<PersonDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}