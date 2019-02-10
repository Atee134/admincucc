using Ag.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ag.Common.Dtos.Request
{
    public class IncomeChunkUpdateDto
    {
        public long? Id { get; set; }
        
        [Required]
        public Site? Site { get; set; }

        [Required]
        public double? Income { get; set; }
    }
}
