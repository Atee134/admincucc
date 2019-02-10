using Ag.Common.Enums;

namespace Ag.Common.Dtos.Response
{
    public class IncomeChunkForReturnDto
    {
        public long Id { get; set; }

        public Site Site { get; set; }

        public double Sum { get; set; }

        public double IncomeForStudio { get; set; }

        public double IncomeForOperator { get; set; }

        public double IncomeForPerformer { get; set; }
    }
}
