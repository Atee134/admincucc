using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Common.Dtos
{
    public class WorkDayForListDto
    {
        public DateTime Date { get; set; }

        public List<UserForListDto> Workers { get; set; }
    }
}
