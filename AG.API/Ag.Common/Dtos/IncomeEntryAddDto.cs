using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Common.Dtos
{
    class IncomeEntryAddDto
    {
        public DateTime Date { get; set; }

        public string Site { get; set; } // TODO like priority

        public double IncomeInDollars { get; set; }
    }
}
