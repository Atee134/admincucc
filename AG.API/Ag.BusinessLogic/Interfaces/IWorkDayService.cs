﻿using Ag.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IWorkDayService
    {
        List<DateTime> GetDatesOfCurrentPeriod();
        void AddWorkDay(WorkDayForAddDto workdayDto, int userId);
    }
}