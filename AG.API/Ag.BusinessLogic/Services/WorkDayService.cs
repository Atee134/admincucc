using Ag.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Services
{
    public class WorkDayService : IWorkDayService
    {
        private const int periodSeparatorDay = 15; // TODO add this to config

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
    }
}
