using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Data.Models
{
    public class SearchResult
    {
        public string Address { get; set; }

        public List<string> SummaryText { get; set; }

        public string FullText { get; set; }

        public int Id { get; set; }

    }
}
