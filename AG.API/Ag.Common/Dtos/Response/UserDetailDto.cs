using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Common.Dtos.Response
{
    public class UserDetailDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public Shift Shift { get; set; }

        public Role Role { get; set; }

        public string Color { get; set; }

        public List<Site> Sites { get; set; }

        public List<UserForListDto> Colleagues { get; set; }

        public double MinPercent { get; set; }

        public double MaxPercent { get; set; }
    }
}
