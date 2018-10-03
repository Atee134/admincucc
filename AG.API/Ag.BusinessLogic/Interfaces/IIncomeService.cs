using Ag.BusinessLogic.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IIncomeService
    {
        Task<List<IncomeEntryForReturnDto>> GetIncomeEntries();
    }
}
