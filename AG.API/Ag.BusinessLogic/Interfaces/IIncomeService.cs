using Ag.BusinessLogic.Models;
using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using System;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IIncomeService
    {
        IncomeEntryForReturnDto GetIncomeEntry(long incomeId);
        IncomeListDataReturnDto GetIncomeEntries(IncomeListFilterParams filterParams);
        void ValidateAuthorityToUpdateIncome(int userId, long incomeId);
        IncomeEntryForReturnDto UpdateIncomeEntry(long incomeEntryId, IncomeEntryUpdateDto incomeEntryDto);
        IncomeEntryForReturnDto AddIncomEntry(int userId, IncomeEntryAddDto incomeEntryDto);
        void DeleteIncomeEntry(long incomeId);
        bool UpdateIncomeEntryLockedState(long incomeId, bool newLockState);
        void RecalculateIncomePercentsOfPeriod(DateTime date, int operatorId, int performerId, bool forcefully = false);
    }
}
