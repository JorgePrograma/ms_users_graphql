namespace msusersgraphql.Models.Dtos
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? IdUserIDCS { get; set; }
        public string? AvatarPath { get; set; }
        public string? UserName { get; set; }
        public DateTime CreationDate { get; set; }
        public string? State { get; set; }
        public List<UserRoleDto> Roles { get; set; } = new List<UserRoleDto>();
    }

    public class UserRoleDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }

    public class UserListDto
    {
        public List<UserDto> Data { get; set; } = new List<UserDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}