using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos;
using Ag.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ag.BusinessLogic.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly AgDbContext _context;

        public IncomeService(AgDbContext context)
        {
            _context = context;
        }

        public List<IncomeEntryForReturnDto> GetIncomeEntries()
        {
            //return _context.IncomeEntries.Select(ie => new IncomeEntryForReturnDto
            //{
            //    IncomeInDollars = ie.IncomeForOwner,
            //    SiteName = ie.SiteName,
            //    WorkDay = ie.Date
            //}).ToList(); // TODO replace this with automapper

            return null;
        }
    }
}
