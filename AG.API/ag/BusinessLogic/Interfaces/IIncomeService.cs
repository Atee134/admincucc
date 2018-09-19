using BusinessLogic.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IIncomeService
    {
        Task<List<IncomeEntryForReturnDto>> GetIncomeEntries();
    }
}