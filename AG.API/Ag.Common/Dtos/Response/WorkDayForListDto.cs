using System;
using System.Collections.Generic;

namespace Ag.Common.Dtos.Response
{
    public class WorkDayForListDto
    {
        public DateTime Date { get; set; }

        public List<UserForListDto> Workers { get; set; }
    }
}
