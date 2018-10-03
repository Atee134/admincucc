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
        public DateTime WorkDay { get; set; } // TODO this will be a reference to the workday entity

        public User User { get; set; }

        public int UserId { get; set; }
    }
}
