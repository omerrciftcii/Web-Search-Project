using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using SharedLibrary.Data.Domain;

namespace SharedLibrary.Data.Db
{
    public class ResultManager : Data.ResultManager
    {
        SqlConnection SConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        SqlCommand command;
        SqlDataReader reader;

        public override IEnumerable<Result> ReadResults()
        {
            SConnection.Open();
            command = new SqlCommand("Select * From Result", SConnection);
            reader = command.ExecuteReader();

            List<Result> resultList = new List<Result>();

            while (reader.Read())
            {
                resultList.Add(new Result
                {
                    Id = reader.GetInt32(0),
                    DateCreated = reader.GetDateTime(1),
                    HashCode = Convert.ToInt32(2),
                    Text = reader.GetString(3),
                    SiteConfigId = reader.GetInt32(4),
                    IsArchive = reader.GetBoolean(5),

                });
                //Console.WriteLine(reader[0] + " " + reader[1] + " " + reader[2] + " " + reader[3] + " " + reader[4] + " " + reader[5]);
            }
            reader.Close();
            SConnection.Close();
            return resultList;
        }

        public override void WriteResult(Result result, SiteConfig siteConfig)
        {
            SConnection.Open();

            if(siteConfig.RefreshMethod==EnumRefreshMethod.Overwrite)
            {
                command = new SqlCommand("DELETE FROM Result WHERE SiteConfigId=@p1", SConnection);
                command.Parameters.AddWithValue("@p1", siteConfig.Id);
                command.ExecuteNonQuery();
            }
            else
            {
                command = new SqlCommand("DELETE FROM Result WHERE HashCode=@p2 AND SiteConfigId=@p1", SConnection);
                command.Parameters.AddWithValue("@p1", siteConfig.Id);
                command.Parameters.AddWithValue("@p2", result.Text.GetHashCode());
                command.ExecuteNonQuery();
            }

            SqlCommand update = new SqlCommand("Update Result set IsArchive=1 where SiteConfigId=@p5", SConnection);
            update.Parameters.AddWithValue("@p5", siteConfig.Id);
            update.ExecuteNonQuery();
            command = new SqlCommand("insert into Result (DateCreated,HashCode,Text,SiteConfigId,IsArchive,Address) values (@p1,@p2,@p3,@p4,0,@p5)", SConnection);
            command.Parameters.AddWithValue("@p1", result.DateCreated);
            command.Parameters.AddWithValue("@p2", result.Text.GetHashCode());
            command.Parameters.AddWithValue("@p3", result.Text );
            command.Parameters.AddWithValue("@p4", siteConfig.Id);
            command.Parameters.AddWithValue("@p5", siteConfig.Address);

            command.ExecuteNonQuery();
            SConnection.Close();
        }
    }
}
