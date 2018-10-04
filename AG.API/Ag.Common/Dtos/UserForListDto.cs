using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Common.Dtos
{
    public class UserForListDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Shift { get; set; }

        public string Role { get; set; }
    }
}
