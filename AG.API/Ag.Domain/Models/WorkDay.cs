using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ag.Domain.Models
{
    public class WorkDay
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Shift Shift { get; set; }

        [Required]
        public virtual User Operator { get; set; }

        [Required]
        public virtual User Performer { get; set; }

        public virtual ICollection<IncomeEntry> IncomeEntries { get; set; }
    }
}