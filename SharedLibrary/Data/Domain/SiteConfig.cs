using System;

namespace SharedLibrary.Data.Domain
{
    public class SiteConfig
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public int PeriodInMinutes { get; set; }

        public EnumRefreshMethod RefreshMethod { get; set; }

        public DateTime LastExecutionTime { get; set; }
    }
}
