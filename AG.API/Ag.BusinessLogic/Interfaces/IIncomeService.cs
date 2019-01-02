using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using System.Collections.Generic;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IIncomeService
    {
        List<IncomeEntryForReturnDto> GetIncomeEntries(int? userId = null);
        IncomeEntryForReturnDto AddIncomEntry(int userId, IncomeEntryAddDto incomeEntryDto);
    }
}
