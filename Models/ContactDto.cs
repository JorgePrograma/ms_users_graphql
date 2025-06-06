namespace msusersgraphql.Models.Dtos
{
    public class ContactDto
    {
        public string? Id { get; set; }
        public string? IdPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? CellPhone { get; set; }
        public string? Country { get; set; }
        public string? Department { get; set; }
        public string? Locality { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
    }

    public class ContactListDto
    {
        public List<ContactDto> Data { get; set; } = new List<ContactDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
