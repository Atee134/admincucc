using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos;
using Ag.Domain;
using Ag.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ag.BusinessLogic.Services
{
    public class WorkDayService : IWorkDayService
    {
        private const int periodSeparatorDay = 15; // TODO add this to config
        private readonly AgDbContext _context;

        public WorkDayService(AgDbContext context)
        {
            _context = context;
        }

        public List<DateTime> GetDatesOfCurrentPeriod()
        {
            DateTime now = DateTime.Now;
            DateTime toAdd;
            int count;

            List<DateTime> availableDates = new List<DateTime>();

            if (now.Day <= periodSeparatorDay)
            {
                toAdd = new DateTime(now.Year, now.Month, 1);
                count = periodSeparatorDay;
            }
            else
            {
                toAdd = new DateTime(now.Year, now.Month, periodSeparatorDay + 1);
                count = DateTime.DaysInMonth(now.Year, now.Month) - periodSeparatorDay;
            }

            for (int i = 0; i < count; i++)
            {
                availableDates.Add(toAdd);
                toAdd = toAdd.AddDays(1);
            }

            return availableDates;
        }

        public void AddWorkDay(WorkDayForAddDto workdayDto, int userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                // TODO throw exception
                return;
            }
               
            if (user.Colleague == null)
            {
                // TODO throw exception
                return;
            }

            var performer = user.Role == Common.Enums.Role.Performer ? user : user.Colleague;
            var op = user.Role == Common.Enums.Role.Operator ? user : user.Colleague;

            var workDay = new WorkDay
            {
                Date = workdayDto.Date,
                Shift = workdayDto.Shift,
                Performer = performer,
                Operator = op
            };

            _context.Add(workDay);
        }
    }
}
