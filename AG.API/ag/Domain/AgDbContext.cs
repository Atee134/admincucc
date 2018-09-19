using System;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class AgDbContext : DbContext
    {
        public AgDbContext(DbContextOptions<AgDbContext> options) : base(options) { }

        public DbSet<IncomeEntry> IncomeEntries { get; set; }

        public IncomeEntry MyProperty { get; set; }
    }
}
