﻿using Ag.Common.Enums;

namespace Ag.Common.Dtos.Request
{
    public class UserForRegisterDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}