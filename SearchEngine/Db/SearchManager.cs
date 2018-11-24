using System;
using System.Collections.Generic;
using System.Linq;
using SharedLibrary.Data.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using SharedLibrary.Data;

namespace SearchEngine.Db
{
    public class SearchManager : SearchEngine.SearchManager
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        public override List<SearchResult> Search(SearchModel model)
        {
            var siteConfigManager = SiteConfigManager.Create();

            var siteConfigs = siteConfigManager.ReadSiteConfigs();

            SqlCommand cmd = new SqlCommand("WebSearch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@searchtext", SqlDbType.NVarChar).Value = model.SearchText;
            cmd.Parameters.Add("@casesensitive", SqlDbType.Bit).Value = Convert.ToBoolean(model.IsCaseSensitive);
            cmd.Parameters.Add("@wholeword", SqlDbType.Bit).Value = Convert.ToBoolean(model.SearchWholeWord);
            cmd.Parameters.Add("@lastresultid", SqlDbType.Int).Value = model.LastResultId;
            cmd.Parameters.Add("@itemsperpage", SqlDbType.Int).Value = model.ItemsCountPerPage;
            cmd.Parameters.Add("@isArchive", SqlDbType.Bit).Value = model.IsArchive;
            cmd.Parameters.Add("@siteConfigId", SqlDbType.Bit).Value = model.SiteConfigId;
            DataTable dt = new DataTable();
            var da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            List<SearchResult> searcResultList = new List<SearchResult>();

            foreach (DataRow row in dt.Rows)
            {
                var siteConfig = siteConfigs.FirstOrDefault(s => s.Id == row.Field<int>("SiteConfigId"));

                var sr = new SearchResult
                {
                    Address = siteConfig?.Address,
                    Id = row.Field<int>("Id"),
                    SummaryText = GetSentences(row.Field<string>("Text"), model)
                };

                if (sr.SummaryText.Count == 0)
                {
                    throw new Exception("Unexpefted Search Results!");
                }
                searcResultList.Add(sr);
            }
            return searcResultList;
        }
    }
}