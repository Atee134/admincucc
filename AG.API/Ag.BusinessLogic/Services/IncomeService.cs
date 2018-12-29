using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos;
using Ag.Common.Dtos.Response;
using Ag.Domain;
using Ag.Domain.Models;
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

        public List<IncomeEntryForReturnDto> GetIncomeEntries(int userId)
        {
            List<IncomeEntryForReturnDto> incomeEntriesToReturn = new List<IncomeEntryForReturnDto>();

            var userEntries = _context.IncomeEntries
                .Include(i => i.IncomeChunks)
                .Where(i => i.Operator.Id == userId || i.Performer.Id == userId);

            foreach (var entry in userEntries)
            {
                IncomeEntryForReturnDto incomeEntryDto = new IncomeEntryForReturnDto()
                {
                    Id = entry.Id,
                    Date = entry.Date,
                    TotalIncomeForOwner = entry.TotalIncomeForOwner,
                    TotalIncomeForOperator = entry.TotalIncomeForOperator,
                    TotalIncomeForPerformer = entry.TotalIncomeForPerformer,
                    IncomeChunkDtos = GetIncomeChunks(entry)
                };

                incomeEntriesToReturn.Add(incomeEntryDto);
            }

            return incomeEntriesToReturn;
        }

        private List<IncomeChunkForReturnDto> GetIncomeChunks(IncomeEntry entry)
        {
            List<IncomeChunkForReturnDto> incomeChunksForReturn = new List<IncomeChunkForReturnDto>();

            foreach (var chunk in entry.IncomeChunks)
            {
                IncomeChunkForReturnDto incomeChunkDto = new IncomeChunkForReturnDto()
                {
                    Id = chunk.Id,
                    Site = chunk.Site,
                    IncomeForOwner = chunk.IncomeForOwner,
                    IncomeForOperator = chunk.IncomeForOperator,
                    IncomeForPerformer = chunk.IncomeForPerformer
                };

                incomeChunksForReturn.Add(incomeChunkDto);
            }

            return incomeChunksForReturn;
        }
    }
}
