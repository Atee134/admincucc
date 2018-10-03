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

        // TODO something is wrong with these connections
        [Required]
        public User Operator { get; set; }

        [Required]
        public User Performer { get; set; }

        public IncomeEntry IncomeEntry { get; set; }
    }
}
