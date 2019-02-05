using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IIncomeService
    {
        IncomeListDataReturnDto GetIncomeEntries(int? userId = null);
        IncomeEntryForReturnDto AddIncomEntry(int userId, IncomeEntryAddDto incomeEntryDto);
    }
}
