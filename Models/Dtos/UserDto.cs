using System;
using System.Collections.Generic;

namespace myapp.Models.Dtos
{
    public class UserDto
    {
        public string id { get; set; }
        public string idUserIDCS { get; set; }
        public string avatarPath { get; set; }
        public string userName { get; set; }
        public DateTime creationDate { get; set; }
        public string state { get; set; }
        public List<RoleDto> roles { get; set; }
    }
}