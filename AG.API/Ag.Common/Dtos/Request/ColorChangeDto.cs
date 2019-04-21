using System.ComponentModel.DataAnnotations;

namespace Ag.Common.Dtos.Request
{
    public class ColorChangeDto
    {
        [Required]
        public int OperatorId { get; set; }

        [Required]
        public int PerformerId { get; set; }

        [Required]
        public string Color { get; set; }
    }
}
