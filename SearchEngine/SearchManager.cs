using SharedLibrary.Data.Domain;
using SharedLibrary.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    public abstract class SearchManager
    {
        public abstract List<SearchResult> Search(SearchModel model);

        public SearchModel Parse(string searchTerm)
        {
            string[] parts = searchTerm.Split(',');

            return new SearchModel
            {
                ItemsCountPerPage = 10,
                SearchText = parts[0],
                IsCaseSensitive = Convert.ToBoolean(Convert.ToInt32(GetPart(parts, 1) ?? "0")),
                SearchWholeWord = Convert.ToBoolean(Convert.ToInt32(GetPart(parts, 2) ?? "0")),
                LastResultId = Convert.ToInt32(GetPart(parts, 3) ?? "0"),
                IsArchive = Convert.ToBoolean(Convert.ToInt32(GetPart(parts, 4) ?? "0")),
                SiteConfigId = Convert.ToInt32(GetPart(parts, 5) ?? "0" )
            };
        }

        private string GetPart(string [] parts, int index)
        {
            if (parts.Length <= index)
                return null;

            return parts[index];
        }

        public bool HasWord(string sentence, SearchModel model)
        {
            if (model.SearchWholeWord)
            {
                if (model.IsCaseSensitive)
                    return sentence.Contains(' ' + model.SearchText + ' ');
                else
                    return sentence.ToLower().Contains(' ' + model.SearchText.ToLower() + ' ');
            }
            if (model.IsCaseSensitive)
                return sentence.Contains(model.SearchText);

            return sentence.ToLower().Contains(model.SearchText.ToLower());
        }

        public List<String> GetSentences(string text, SearchModel model)
        {
            var sentences = text.Split(new[] { ". " }, StringSplitOptions.RemoveEmptyEntries);

            var matches = from sentence in sentences
                          where
                            HasWord(sentence, model)
                          select sentence;
            return matches.ToList();
        }

        public static SearchManager Create()
        {
            var dataStoreType = ConfigurationManager.AppSettings["dataStoreType"];

            if (dataStoreType == "File")
                return new File.SearchManager();
            else if (dataStoreType == "Db")
                return new Db.SearchManager();

            return null;
        }
    }
}
