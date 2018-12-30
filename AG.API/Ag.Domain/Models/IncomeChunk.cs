using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public double IncomeForOwner { get; set; }

        [Required]
        public double IncomeForOperator { get; set; }

        [Required]
        public double IncomeForPerformer { get; set; }

        [Required]
        public IncomeEntry IncomeEntry { get; set; }
    }
}
