using Ag.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IJoinTableHelperService
    {
        List<User> GetColleagues(int userId);
    }
}
