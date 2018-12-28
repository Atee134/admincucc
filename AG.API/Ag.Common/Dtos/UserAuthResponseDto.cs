using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Common.Dtos
{
    public class UserAuthResponseDto
    {
        public string Token { get; set; }

        public UserForListDto User { get; set; }
    }
}
