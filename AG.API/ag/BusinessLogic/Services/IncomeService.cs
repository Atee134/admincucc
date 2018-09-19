using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly AgDbContext _context;

        public IncomeService(AgDbContext context)
        {
            _context = context;
        }

        public Task<List<IncomeEntryForReturnDto>> GetIncomeEntries()
        {
            return _context.IncomeEntries.Select(ie => new IncomeEntryForReturnDto
            {
                IncomeInDollars = ie.IncomeInDollars,
                SiteName = ie.SiteName,
                WorkDay = ie.WorkDay
            }).ToListAsync(); // TODO replace this with automapper
        }
    }
}
