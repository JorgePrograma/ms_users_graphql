using System.Collections.Generic;

namespace myapp.Models.Dtos
{
    public class UserListDto
    {
        public List<UserDto> items { get; set; }
        public int totalCount { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}