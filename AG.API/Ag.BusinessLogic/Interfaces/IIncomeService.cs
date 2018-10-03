using Ag.Common.Dtos;
using System.Collections.Generic;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IIncomeService
    {
        List<IncomeEntryForReturnDto> GetIncomeEntries();
    }
}
