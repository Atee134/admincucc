using Ag.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ag.Common.Dtos.Request
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be minimum 6 characters long.")]
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
