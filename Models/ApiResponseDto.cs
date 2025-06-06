namespace msusersgraphql.Models.Dtos
{
    public class ApiResponseDto<T>
    {
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public int StatusCode { get; set; }
    }

    public class UserDataDto
    {
        public List<UserDto> Items { get; set; } = new List<UserDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
