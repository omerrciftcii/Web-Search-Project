using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Data;
using SharedLibrary.Data.Models;

namespace SearchEngine.File
{
    public class SearchManager : SearchEngine.SearchManager
    {
        public override List<SearchResult> Search(SearchModel model)
        {
            var resultManager = ResultManager.Create();
            var siteConfigManager = SiteConfigManager.Create();

            var results = resultManager.ReadResults();
            var siteConfigs = siteConfigManager.ReadSiteConfigs();

            List<SearchResult> searcResultList = new List<SearchResult>();

            foreach (var result in results)
            {
                var siteConfig = siteConfigs.FirstOrDefault(s => s.Id == result.SiteConfigId);
                
                if(model.IsArchive == true)
                {
                    if (result.IsArchive == false)
                        continue;
                }
                if(model.SiteConfigId .ToString() != "0")
                {
                    if(model.SiteConfigId != result.Id)
                     continue;
                }

                var sr = new SearchResult
                {
                    Address = siteConfig?.Address,
                    Id = result.Id,
                    SummaryText = GetSentences(result.Text, model)
                };

                if (sr.SummaryText.Count > 0)
                    searcResultList.Add(sr);
            }
            return searcResultList;
        }
    }
}
