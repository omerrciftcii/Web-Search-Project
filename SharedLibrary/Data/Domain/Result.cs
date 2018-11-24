using System;

namespace SharedLibrary.Data.Domain
{
    public class Result
    {
        public DateTime DateCreated { get; set; }

        public string Text { get; set; }

        public int HashCode { get; set; }

        public int SiteConfigId { get; set; }

        public int Id { get; set; }

        public bool IsArchive { get; set; }
    }
}
