using Ag.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ag.Domain.Models
{
    public class IncomeChunk
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Site Site { get; set; }

        [Required]
        public double Sum { get; set; }

        [Required]
        public double IncomeForStudio { get; set; }

        [Required]
        public double IncomeForOperator { get; set; }

        [Required]
        public double IncomeForPerformer { get; set; }

        [Required]
        public IncomeEntry IncomeEntry { get; set; }
    }
}
