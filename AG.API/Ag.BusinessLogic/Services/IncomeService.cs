using Ag.BusinessLogic.Exceptions;
using Ag.BusinessLogic.Interfaces;
using Ag.BusinessLogic.Models;
using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using Ag.Common.Enums;
using Ag.Domain;
using Ag.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ag.BusinessLogic.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly AgDbContext _context;
        private readonly ILogger<IncomeService> _logger;
        private readonly IJoinTableHelperService _joinTableHelperService;

        public IncomeService(AgDbContext context, ILogger<IncomeService> logger, IJoinTableHelperService joinTableHelperService)
        {
            _context = context;
            _logger = logger;
            _joinTableHelperService = joinTableHelperService;
        }

        public IncomeEntryForReturnDto AddIncomEntry(int userId, IncomeEntryAddDto incomeEntryDto)
        {
            _logger.LogInformation($"Adding income for user with ID: {userId}");

            var op = _context.Users.FirstOrDefault(u => u.Id == userId && u.Role == Role.Operator);

            if (op == null) throw new AgUnfulfillableActionException($"Operator with ID: {userId} does not exist.");

            if (incomeEntryDto.PerformerId == null)
            {
                throw new NotImplementedException("Solo operators are not supported yet!");
            }

            var performer = _joinTableHelperService.GetColleagues(userId).SingleOrDefault(c => c.Id == incomeEntryDto.PerformerId);

            if (performer == null) throw new AgUnfulfillableActionException($"Model with ID:{incomeEntryDto.PerformerId} is not assigned to Operator with ID:{userId}");
            List<IncomeChunk> incomeChunks = new List<IncomeChunk>();

            foreach (var incomeChunkDto in incomeEntryDto.IncomeChunks)
            {
                incomeChunks.Add(CreateIncomeChunkFromDto(incomeChunkDto, op.MinPercent, performer.MinPercent)); // TODO add logic for currentPercent to not use always minimum (above $251 logic)
            }

            var incomeEntry = new IncomeEntry()
            {
                Date = incomeEntryDto.Date,
                Operator = op,
                Performer = performer,
                IncomeChunks = incomeChunks,
                TotalSum = incomeChunks.Sum(i => i.Sum),
                TotalIncomeForStudio = incomeChunks.Sum(i => i.IncomeForStudio),
                TotalIncomeForOperator = incomeChunks.Sum(i => i.IncomeForOperator),
                TotalIncomeForPerformer = incomeChunks.Sum(i => i.IncomeForPerformer)
            };

            _context.IncomeEntries.Add(incomeEntry);
            _context.SaveChanges();

            _logger.LogInformation($"Income successfully added to operator with ID: {userId}, model ID: {performer.Id}, income ID: {incomeEntry.Id}, income chunk IDs: {String.Join(", ",incomeEntry.IncomeChunks.Select(i => i.Id))}");

            return ConvertIncomeEntryForReturnDto(incomeEntry);
        }

        public void ValidateAuthorityToUpdateIncome(int userId, int incomeId)
        {
            if (_context.IncomeEntries.Include(i => i.Operator).SingleOrDefault(i => i.Id == incomeId).Operator.Id != userId) throw new AgUnauthorizedException("Unauthorized");
        }

        public IncomeEntryForReturnDto UpdateIncomeEntry(int incomeId, IncomeEntryUpdateDto incomeEntryDto)
        {
            _logger.LogInformation($"Updating income with ID: {incomeId}");

            var incomeEntryEntity = _context.IncomeEntries
                .Include(i => i.Operator)
                .Include(i => i.Performer)
                .Include(i => i.IncomeChunks)
                .SingleOrDefault(i => i.Id == incomeId);

            if (incomeEntryEntity == null) throw new AgUnfulfillableActionException($"Income entry with ID: {incomeId} does not exist.");

            if (incomeEntryDto.PerformerId != null && incomeEntryEntity.Performer.Id != incomeEntryDto.PerformerId)
            {
                var colleagues = _joinTableHelperService.GetColleagues(incomeEntryEntity.Operator.Id);

                var newPerformer = colleagues.FirstOrDefault(c => c.Id == incomeEntryDto.PerformerId);

                if (newPerformer == null) throw new AgUnfulfillableActionException($"Performer is not assigned to the operator, can not update income entry.");

                _logger.LogInformation($"Performer changed of income entry with ID: {incomeId}. Old performer ID: {incomeEntryEntity.Performer.Id}, new performer ID: {incomeEntryDto.PerformerId}");

                incomeEntryEntity.Performer = newPerformer;
            }

            if (incomeEntryDto.Date != null && incomeEntryEntity.Date != incomeEntryDto.Date.Value)
            {
                _logger.LogInformation($"Date changed of income entry with ID: {incomeEntryEntity.Id}. Old date: {incomeEntryEntity.Date.ToString()}, new date: {incomeEntryDto.Date.ToString()}");

                incomeEntryEntity.Date = incomeEntryDto.Date.Value;
            }

            var incomeChunksToUpdate = incomeEntryDto.IncomeChunks.Where(i => i.Id.HasValue).ToList();

            if (incomeChunksToUpdate.Count > 0)
            {
                UpdateIncomeChunksOfIncomeEntry(incomeEntryEntity, incomeChunksToUpdate);
            }

            var newlyAddedIncomeChunks = incomeEntryDto.IncomeChunks.Where(i => !i.Id.HasValue).ToList();

            if (newlyAddedIncomeChunks.Count > 0)
            {
                AddIncomeChunksToIncomeEntry(incomeEntryEntity, newlyAddedIncomeChunks);
            }

            incomeEntryEntity.TotalSum = incomeEntryEntity.IncomeChunks.Sum(c => c.Sum);
            incomeEntryEntity.TotalIncomeForStudio = incomeEntryEntity.IncomeChunks.Sum(c => c.IncomeForStudio);
            incomeEntryEntity.TotalIncomeForOperator = incomeEntryEntity.IncomeChunks.Sum(c => c.IncomeForOperator);
            incomeEntryEntity.TotalIncomeForPerformer = incomeEntryEntity.IncomeChunks.Sum(c => c.IncomeForPerformer);

            _context.SaveChanges();

            if (newlyAddedIncomeChunks.Count > 0)
            {
                 var newlyAddedIds = incomeEntryEntity.IncomeChunks.Where(i => newlyAddedIncomeChunks.Select(ic => ic.Site).Contains(i.Site)).Select(i => i.Id).ToList();
                _logger.LogInformation($"New income chunks added during updating of income with ID:{incomeId}, income chunk IDs: {String.Join(", ", newlyAddedIds)}");
            }

            _logger.LogInformation($"Updating income entry with ID: {incomeEntryEntity.Id} was successful.");

            return ConvertIncomeEntryForReturnDto(incomeEntryEntity);
        }

        private void UpdateIncomeChunksOfIncomeEntry(IncomeEntry incomeEntry, List<IncomeChunkUpdateDto> incomeChunkDtos)
        {
            _logger.LogInformation($"Updating income chunks of income entry with ID: {incomeEntry.Id}, income chunk IDs: {String.Join(", ", incomeChunkDtos.Select(i => i.Id.Value))}");

            var operatorPercent = incomeEntry.Operator.MinPercent;
            var performerPercent = incomeEntry.Performer.MinPercent;

            foreach (IncomeChunkUpdateDto incomeChunkDto in incomeChunkDtos)
            {
                var incomeChunkEntity = incomeEntry.IncomeChunks.FirstOrDefault(i => i.Id == incomeChunkDto.Id && i.Site == incomeChunkDto.Site);

                if (incomeChunkEntity == null) throw new AgUnfulfillableActionException($"Income chunk with ID: {incomeChunkDto.Id} does not exist");

                var incomes = CalculateIncomes(incomeChunkDto.Income.Value, operatorPercent, performerPercent);

                incomeChunkEntity.Sum = incomeChunkDto.Income.Value;
                incomeChunkEntity.IncomeForStudio = incomes.IncomeForStudio;
                incomeChunkEntity.IncomeForOperator = incomes.IncomeForOperator;
                incomeChunkEntity.IncomeForPerformer = incomes.IncomeForPerformer;
            }
        }

        private void AddIncomeChunksToIncomeEntry(IncomeEntry incomeEntry, List<IncomeChunkUpdateDto> incomeChunkDtos)
        {
            var operatorPercent = incomeEntry.Operator.MinPercent;
            var performerPercent = incomeEntry.Performer.MinPercent;

            foreach (IncomeChunkUpdateDto incomeChunkDto in incomeChunkDtos)
            {
                if (incomeEntry.IncomeChunks.FirstOrDefault(i => i.Site == incomeChunkDto.Site.Value) != null) throw new AgUnfulfillableActionException($"Income entry already contains income for site: {incomeChunkDto.Site.Value.ToString()}");

                IncomeChunkAddDto incomeChunkAddDto = new IncomeChunkAddDto
                {
                    Income = incomeChunkDto.Income.Value,
                    Site = incomeChunkDto.Site.Value
                };

                incomeEntry.IncomeChunks.Add(CreateIncomeChunkFromDto(incomeChunkAddDto, operatorPercent, performerPercent)); // TODO add logic for currentPercent to not use always minimum (above $251 logic)
            }
        }

        public IncomeEntryForReturnDto GetIncomeEntry(int incomeId)
        {
            var incomeEntry = _context.IncomeEntries
                .Include(i => i.Operator)
                .Include(i => i.Performer)
                .Include(i => i.IncomeChunks)
                .SingleOrDefault(i => i.Id == incomeId);

            if (incomeEntry == null) throw new AgUnfulfillableActionException($"Income entry with ID: {incomeId} does not exist.");

            return ConvertIncomeEntryForReturnDto(incomeEntry);
        }

        public IncomeListDataReturnDto GetIncomeEntries(int? userId = null) // TODO later there will be a filter with ID as param passed in
        {
            _logger.LogInformation($"Getting income entries of user with ID: {userId}");

            List<IncomeEntryForReturnDto> incomeEntryDtos = new List<IncomeEntryForReturnDto>();

            IOrderedQueryable<IncomeEntry> incomeEntryEntities; 

            if (userId != null)
            {
                incomeEntryEntities = _context.IncomeEntries
                .Include(i => i.IncomeChunks)
                .Include(i => i.Operator)
                .Include(i => i.Performer)
                .Where(i => i.Operator.Id == userId || i.Performer.Id == userId)
                .OrderByDescending(i => i.Date);
            }
            else
            {
                incomeEntryEntities = _context.IncomeEntries
                 .Include(i => i.IncomeChunks)
                 .Include(i => i.Operator)
                 .Include(i => i.Performer)
                 .OrderByDescending(i => i.Date);
            }

            foreach (var entry in incomeEntryEntities)
            {
                incomeEntryDtos.Add(ConvertIncomeEntryForReturnDto(entry));
            }

            IncomeListDataReturnDto incomeListDataDto = new IncomeListDataReturnDto
            {
                IncomeEntries = incomeEntryDtos,
            };

            CalculateStatisticsForIncomeListDataDto(incomeListDataDto);

            return incomeListDataDto;
        }

        private void CalculateStatisticsForIncomeListDataDto(IncomeListDataReturnDto incomeListDataDto)
        {
            incomeListDataDto.OperatorStatistics = new IncomeStatisticsDto
            {
                Average = incomeListDataDto.IncomeEntries.Average(i => i.TotalIncomeForOperator),
                Total = incomeListDataDto.IncomeEntries.Sum(i => i.TotalIncomeForOperator)
            };
            incomeListDataDto.PerformerStatistics = new IncomeStatisticsDto
            {
                Average = incomeListDataDto.IncomeEntries.Average(i => i.TotalIncomeForPerformer),
                Total = incomeListDataDto.IncomeEntries.Sum(i => i.TotalIncomeForPerformer)
            };
            incomeListDataDto.StudioStatistics = new IncomeStatisticsDto
            {
                Average = incomeListDataDto.IncomeEntries.Average(i => i.TotalIncomeForStudio),
                Total = incomeListDataDto.IncomeEntries.Sum(i => i.TotalIncomeForStudio)
            };
            incomeListDataDto.TotalStatistics = new IncomeStatisticsDto
            {
                Average = incomeListDataDto.IncomeEntries.Average(i => i.TotalSum),
                Total = incomeListDataDto.IncomeEntries.Sum(i => i.TotalSum)
            };

            incomeListDataDto.SiteStatistics = new List<IncomeStatisticsSiteSumDto>();

            List<Site> allSites = Enum.GetValues(typeof(Site)).Cast<Site>().ToList();

            foreach (Site site in allSites)
            {
                var relevantIncomeChunks = incomeListDataDto.IncomeEntries.SelectMany(i => i.IncomeChunks).Where(ic => ic.Site == site).ToList();

                if (relevantIncomeChunks.Count == 0)
                {
                    continue;
                }

                var siteStatisticDto = new IncomeStatisticsSiteSumDto
                {
                    Site = site,
                    Statistics = new IncomeStatisticsDto
                    {
                        Average = relevantIncomeChunks.Average(i => i.Sum),
                        Total = relevantIncomeChunks.Sum(i => i.Sum)
                    }
                };

                incomeListDataDto.SiteStatistics.Add(siteStatisticDto);
            }
        }

        private IncomeChunk CreateIncomeChunkFromDto(IncomeChunkAddDto incomeChunkDto, double operatorPercent, double performerPercent)
        {
            var incomes = CalculateIncomes(incomeChunkDto.Income, operatorPercent, performerPercent);

            return new IncomeChunk()
            {
                Site = incomeChunkDto.Site,
                Sum = incomeChunkDto.Income,
                IncomeForStudio = incomes.IncomeForStudio, // TODO rename incomechunk entity owner to studio
                IncomeForOperator = incomes.IncomeForOperator,
                IncomeForPerformer = incomes.IncomeForPerformer
            };
        }

        private IncomeRatio CalculateIncomes(double totalIncome, double operatorPercent, double performerPercent)
        {
            double incomeForOperator = totalIncome * operatorPercent;
            double incomeForPerformer = totalIncome * performerPercent;
            double incomeForStudio = totalIncome - (incomeForOperator + incomeForPerformer);

            return new IncomeRatio
            {
                IncomeForOperator = incomeForOperator,
                IncomeForPerformer = incomeForPerformer,
                IncomeForStudio = incomeForStudio
            };
        }

        private IncomeEntryForReturnDto ConvertIncomeEntryForReturnDto(IncomeEntry incomeEntry)
        {
            return new IncomeEntryForReturnDto()
            {
                Id = incomeEntry.Id,
                Date = incomeEntry.Date,
                Color = incomeEntry.Performer?.Color ?? incomeEntry.Operator.Color,
                OperatorId = incomeEntry.Operator.Id,
                OperatorName = incomeEntry.Operator.UserName,
                PerformerId = incomeEntry.Performer.Id,
                PerformerName = incomeEntry.Performer.UserName,
                TotalSum = incomeEntry.TotalSum,
                TotalIncomeForStudio = incomeEntry.TotalIncomeForStudio,
                TotalIncomeForOperator = incomeEntry.TotalIncomeForOperator,
                TotalIncomeForPerformer = incomeEntry.TotalIncomeForPerformer,
                IncomeChunks = GetIncomeChunks(incomeEntry)
            };
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
                    Sum = chunk.Sum,
                    IncomeForStudio = chunk.IncomeForStudio,
                    IncomeForOperator = chunk.IncomeForOperator,
                    IncomeForPerformer = chunk.IncomeForPerformer
                };

                incomeChunksForReturn.Add(incomeChunkDto);
            }

            return incomeChunksForReturn;
        }
    }
}
