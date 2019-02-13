using System;

namespace Ag.BusinessLogic.Models
{
    public class IncomeListFilterParams
    {
        public int? UserId { get; set; }

        public string UserName { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public DateTime? Month { get; set; }

        public int? Period { get; set; }

        public bool HideLocked { get; set; }

        public double? MinTotal { get; set; }

        public double? MaxTotal { get; set; }

        public string OrderByColumn { get; set; }

        public bool OrderDescending { get; set; }
    }
}
