using Ag.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IWorkDayService
    {
        List<DateTime> GetDatesOfCurrentPeriod();
        void AddWorkDay(DateTime date, int userId);
    }
}
