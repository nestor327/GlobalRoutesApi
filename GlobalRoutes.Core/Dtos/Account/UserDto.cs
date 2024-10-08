namespace GlobalRoutes.Core.Dtos.Account
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime Registered { get; set; }
        public string? UserImageUrl { get; set; }
        public int? CityId { get; set; }
        public int? TimeZoneId { get; set; }
    }
}
