using System;
using System.Collections.Generic;

namespace Ag.Common.Dtos
{
    public class WorkDayForListDto
    {
        public DateTime Date { get; set; }

        public List<UserForListDto> Workers { get; set; }
    }
}
