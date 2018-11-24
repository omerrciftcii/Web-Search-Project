using System;
using System.Linq;
using System.Collections.Generic;
using SharedLibrary.Data.Domain;
using System.IO;
using System.Configuration;

namespace SharedLibrary.Data.File
{
    public class ResultManager : Data.ResultManager
    {
        public override IEnumerable<Result> ReadResults()
        {
            string PATH = ConfigurationManager.AppSettings["FilePath"];
            string[] fileNames = Directory.GetFiles(PATH, "*_*.txt");
            List<Result> resultList = new List<Result>();

            foreach (var fileName in fileNames)
            {
                var parts = fileName.Split('_');

                using (var read = new StreamReader(fileName, true))
                {
                    var fileText = read.ReadToEnd();

                    var r = new Result
                    {
                        DateCreated = Convert.ToDateTime(parts[2].Replace("#",":").Split('.')[0]),
                        HashCode = Convert.ToInt32(parts[1]),
                        IsArchive = false,
                        SiteConfigId = Convert.ToInt32(parts[0].Split('\\').Last()),
                        Text = fileText,
                        Id = Convert.ToInt32(parts[1]),
                    };

                    resultList.Add(r);
                }
            }

            resultList.OrderByDescending(o => o.DateCreated).GroupBy(t => t.SiteConfigId).ToList().ForEach(g => g.Skip(1).ToList().ForEach(a => a.IsArchive = true));

            return resultList;
        }

        public override void WriteResult(Result result, SiteConfig siteConfig)
        {
            string PATH = ConfigurationManager.AppSettings["FilePath"];
            var patternToDelete = $"{siteConfig.Id}_{(siteConfig.RefreshMethod == EnumRefreshMethod.Overwrite ? "" : result.Text.GetHashCode().ToString() + "_")}*.txt";
            var patternToCreate = $"{PATH}{siteConfig.Id}_{result.Text.GetHashCode()}_{result.DateCreated.ToString("yyyy-MM-ddTHH#MM#ss")}.txt";

           new DirectoryInfo(PATH).GetFiles(patternToDelete).ToList().ForEach(d => d.Delete());

            using (StreamWriter sw = new StreamWriter(patternToCreate))
                sw.WriteLine(result.Text);
        }
    }
}
