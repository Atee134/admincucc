using Ag.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ag.Common.Dtos.Request
{
    public class UserForEditDto
    {
        [Required]
        public int? Id { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required]
        public List<Site> Sites { get; set; }

        [Required]
        [Range(0,1, ErrorMessage = "Minimum percent must be between 0 and 1.")]
        public double? MinPercent { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Maximum percent must be between 0 and 1.")]
        public double? MaxPercent { get; set; }
    }
}
