using Ag.Common.Enums;
using System.Collections.Generic;

namespace Ag.Common.Dtos.Response
{
    public class UserForListDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public double LastPercent { get; set; }

        public Shift Shift { get; set; }

        public Role Role { get; set; }

        public string Color { get; set; }

        public List<Site> Sites { get; set; }
    }
}
