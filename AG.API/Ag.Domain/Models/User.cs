using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ag.Domain.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public double MinPercent { get; set; }

        [Required]
        public double MaxPercent { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public Shift Shift { get; set; }

        public User Colleague { get; set; }

        public string Sites { get; set; } // csv list for now
        // TODO many-many connection with sites

        public ICollection<IncomeEntry> IncomeEntries { get; set; }

        public ICollection<WorkDay> WorkDays { get; set; }

        // TODO workdays collection
    }
}
