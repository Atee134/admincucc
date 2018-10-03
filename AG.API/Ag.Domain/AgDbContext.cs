using System;
using Ag.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ag.Domain
{
    public class AgDbContext : DbContext
    {
        public AgDbContext(DbContextOptions<AgDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<WorkDay> WorkDays { get; set; }

        public DbSet<IncomeEntry> IncomeEntries { get; set; }
    }
}
