using Ag.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IWorkDayService
    {
        List<DateTime> GetCurrentWorkDaysOfUser(int userId);
        List<DateTime> GetModifiableWorkDays();
        void AddWorkDay(DateTime date, int userId);
        void RemoveWorkDay(DateTime date, int userId);
    }
}
