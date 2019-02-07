using System;

namespace Ag.BusinessLogic.Exceptions
{
    public class AgUnauthorizedException : ApplicationException
    {
        public AgUnauthorizedException(string message) : base(message) { }
    }
}
