using Ag.Common.Dtos.Response;
using System.Collections.Generic;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IIncomeService
    {
        List<IncomeEntryForReturnDto> GetIncomeEntries(int userId);
    }
}
