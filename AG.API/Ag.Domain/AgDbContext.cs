using System;
using Ag.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ag.Domain
{
    public class AgDbContext : DbContext
    {
        public AgDbContext(DbContextOptions<AgDbContext> options) : base(options) { }

        public DbSet<IncomeEntry> IncomeEntries { get; set; }
    }
}
