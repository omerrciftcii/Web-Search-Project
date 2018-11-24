using System;
using System.Collections.Generic;
using SharedLibrary.Data.Domain;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace SharedLibrary.Data.Db
{
    public class SiteConfigManager : Data.SiteConfigManager
    {
        SqlConnection SConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        SqlDataReader reader;
        SqlCommand Command;

        public override IEnumerable<SiteConfig> ReadSiteConfigs()
        {
            List<SiteConfig> list = new List<SiteConfig>();
            SConnection.Open();
            Command = new SqlCommand("Select * From SiteConfig", SConnection);
            //SqlDataAdapter SAdapter = new SqlDataAdapter("SELECT * FROM tbl_SiteConfig ", SConnection);
            //DataTable dt = new DataTable();
            //SAdapter.Fill(dt);
            
            reader = Command.ExecuteReader();
            SiteConfig Sconfig = new SiteConfig();
           
            while (reader.Read())
            {
                list.Add(new SiteConfig
                {
                    Id = reader.GetInt32(0),
                    Address = reader.GetString(1),
                    IsActive = reader.GetBoolean(2),
                    PeriodInMinutes = reader.GetInt32(3),
                    RefreshMethod = (EnumRefreshMethod)(Enum.Parse(typeof(EnumRefreshMethod),Convert.ToString(reader[4]))),
                    LastExecutionTime = reader.GetDateTime(5)
                });
                //Console.WriteLine(reader[0] + " " + reader[1] + " " + reader[2] + " " + reader[3] + " " + reader[4] + " " + reader[5]);
            }
            reader.Close();
            SConnection.Close();
            return list;
           

        }

        public override void WriteSiteConfig(SiteConfig siteConfig)
        {
            SConnection.Open();
            Command = new SqlCommand("update SiteConfig set  Address=@p1,ısActive=@p2,PeriodInMinutes=@p3,RefreshMethod=@p4,LastExecutionTime=@p5 where ID=@p6", SConnection);
           
            Command.Parameters.AddWithValue("@p1", siteConfig.Address);
            Command.Parameters.AddWithValue("@p2", siteConfig.IsActive);
            Command.Parameters.AddWithValue("@p3", siteConfig.PeriodInMinutes);
            Command.Parameters.AddWithValue("@p4", siteConfig.RefreshMethod);
            Command.Parameters.AddWithValue("@p5", siteConfig.LastExecutionTime);
            Command.Parameters.AddWithValue("@p6", siteConfig.Id);

            Command.ExecuteNonQuery();
            SConnection.Close();
          


            // throw new NotImplementedException();
        }
    }
}
