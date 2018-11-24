using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Data.Models
{
    public class SearchModel
    {
        public string SearchText { get; set; }

        public bool IsCaseSensitive { get; set; }

        public bool SearchWholeWord { get; set; }

        public int ItemsCountPerPage { get; set; }

        public int LastResultId { get; set; }

        public bool IsArchive { get; set; }

        public int SiteConfigId { get; set; }


    }
}
