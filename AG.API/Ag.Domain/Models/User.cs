using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public virtual User Colleague { get; set; }

        [Required]
        public string Sites { get; set; } //TODO like category

        [InverseProperty("Operator")]
        public virtual ICollection<WorkDay> OperatorWorkDays { get; set; }

        [InverseProperty("Performer")]
        public virtual ICollection<WorkDay> PerformerWorkDays { get; set; }

        [InverseProperty("Operator")]
        public virtual ICollection<IncomeEntry> OperatorIncomeEntries { get; set; }

        [InverseProperty("Performer")]
        public virtual ICollection<IncomeEntry> PerformerIncomeEntries { get; set; }
    }
}