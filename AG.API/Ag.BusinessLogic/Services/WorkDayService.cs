using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos;
using Ag.Domain;
using Ag.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ag.BusinessLogic.Services
{
    public class WorkDayService : IWorkDayService
    {
        private const int periodSeparatorDay = 15; // TODO add this to config
        private const int scheduleThresholdInDays = 4;
        private readonly AgDbContext _context;

        public WorkDayService(AgDbContext context)
        {
            _context = context;
        }

        public List<DateTime> GetCurrentWorkDaysOfUser(int userId)
        {
            List<DateTime> relevantDates = GetModifiableWorkDays();
            var workDays = _context.WorkDays.Where(w => (w.Operator.Id == userId || w.Performer.Id == userId) && relevantDates.Contains(w.Date));

            return workDays.Select(w => w.Date).ToList();
        }

        public List<DateTime> GetModifiableWorkDays()
        {
            DateTime now = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();

            if (now.Day < periodSeparatorDay)
            {
                DateTime toAdd = new DateTime(now.Year, now.Month, 1);

                int count = DateTime.DaysInMonth(now.Year, now.Month);

                for (int i = 0; i < count; i++)
                {
                    dates.Add(toAdd);
                    toAdd = toAdd.AddDays(1);
                }
            }
            else
            {
                dates.AddRange(GetDatesOfPeriod(now));

                DateTime nextPeriodStartDate;

                if (now.Month == 12)
                {
                    nextPeriodStartDate = new DateTime(now.Year + 1, 1, 1);
                }
                else
                {
                    nextPeriodStartDate = new DateTime(now.Year, now.Month + 1, 1);
                }

                dates.AddRange(GetDatesOfPeriod(nextPeriodStartDate));
            }

            return dates;
        }

        private List<DateTime> GetDatesOfPeriod(DateTime periodStartDate)
        {
            DateTime toAdd;
            int count;

            List<DateTime> availableDates = new List<DateTime>();

            if (periodStartDate.Day <= periodSeparatorDay)
            {
                toAdd = new DateTime(periodStartDate.Year, periodStartDate.Month, 1);
                count = periodSeparatorDay;
            }
            else
            {
                toAdd = new DateTime(periodStartDate.Year, periodStartDate.Month, periodSeparatorDay + 1);
                count = DateTime.DaysInMonth(periodStartDate.Year, periodStartDate.Month) - periodSeparatorDay;
            }

            for (int i = 0; i < count; i++)
            {
                availableDates.Add(toAdd);
                toAdd = toAdd.AddDays(1);
            }

            return availableDates;
        }

        public void AddWorkDay(DateTime date, int userId)
        {
            if (date < DateTime.Now.AddDays(scheduleThresholdInDays))
            {
                // TODO throw exception
                return;
            }

            var user = _context.Users.Include(u => u.Colleague).SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                // TODO throw exception
                return;
            }

            if (user.Role != Common.Enums.Role.Operator)
            {
                // TODO throw exception
                return;
            }

            if (user.Colleague == null)
            {
                // TODO throw exception
                return;
            }

            if (_context.WorkDays.Include(w => w.Operator).SingleOrDefault(w => w.Date == date && w.Operator.Id == userId) != null)
            {
                return; // no exception, workday already exists, nothing more to do
            }

            var workDay = new WorkDay
            {
                Date = date,
                Shift = user.Shift,
                Performer = user.Colleague,
                Operator = user
            };

            _context.WorkDays.Add(workDay);
            _context.SaveChanges();
        }

        public void RemoveWorkDay(DateTime date, int userId)
        {
            if (date < DateTime.Now.AddDays(scheduleThresholdInDays))
            {
                // TODO throw exception
                return;
            }

            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                // TODO throw exception
                return;
            }

            if (user.Role != Common.Enums.Role.Operator)
            {
                // TODO throw exception
                return;
            }

            var workDay = _context.WorkDays.Include(w => w.Operator).Include(w => w.Performer).SingleOrDefault(w => w.Date == date && w.Operator.Id == userId);

            if (workDay == null)
            {
                return;
            }

            _context.WorkDays.Remove(workDay);
            _context.SaveChanges();
        }
    }
}
