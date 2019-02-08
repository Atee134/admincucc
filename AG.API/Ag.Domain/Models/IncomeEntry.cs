using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ag.Domain.Models
{
    public class IncomeEntry
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double TotalSum { get; set; }

        [Required]
        public double TotalIncomeForStudio { get; set; }

        [Required]
        public double TotalIncomeForOperator { get; set; }

        [Required]
        public double TotalIncomeForPerformer { get; set; }

        [Required]
        public virtual User Operator { get; set; }

        [Required]
        public virtual User Performer { get; set; }

        public ICollection<IncomeChunk> IncomeChunks { get; set; }
    }
}