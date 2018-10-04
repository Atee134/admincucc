using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        void AddPerformer(int operatorId, int performerId);
    }
}
