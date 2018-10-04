using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ag.Domain.Models
{
    public class IncomeEntry
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string SiteName { get; set; } // TODO should this be another entity?

        [Required]
        public double IncomeInDollars { get; set; }

        [Required]
        public virtual WorkDay WorkDay { get; set; }
    }
}