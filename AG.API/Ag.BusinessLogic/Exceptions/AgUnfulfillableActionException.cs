using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Exceptions
{
    public class AgUnfulfillableActionException : ApplicationException
    {
        public AgUnfulfillableActionException(string message) : base(message) { }
    }
}
