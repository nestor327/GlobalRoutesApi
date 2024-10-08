using System.ComponentModel.DataAnnotations;

namespace GlobalRoutes.Api.Requests.Account
{
    public class PostUserRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }        
        [Required]
        public int CountryId { get; set; }
        public int CityId { get; set; }
        [Required]
        public int TimeZoneId { get; set; }
    }
}
