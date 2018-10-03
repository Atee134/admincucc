using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Common.Dtos
{
    public class WorkDayForAddDto
    {
        public DateTime Date { get; set; }

        public Shift Shift { get; set; }
    }
}
