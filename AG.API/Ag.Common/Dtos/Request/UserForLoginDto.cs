using System.ComponentModel.DataAnnotations;

namespace Ag.Common.Dtos.Request
{
    public class UserForLoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}
