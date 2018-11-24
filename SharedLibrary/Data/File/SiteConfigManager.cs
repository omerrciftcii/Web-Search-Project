using System;
using System.Collections.Generic;
using SharedLibrary.Data.Domain;
using System.IO;
using System.Configuration;

namespace SharedLibrary.Data.File
{
    public class SiteConfigManager : Data.SiteConfigManager
    {
        public override IEnumerable<SiteConfig> ReadSiteConfigs() //Dosya Oku
        {
            List<SiteConfig> list = new List<SiteConfig>();
            using (StreamReader read = new StreamReader(ConfigurationManager.AppSettings["FilePath"] + "\\sites.txt", true))
            {
                while (true)
                {
                    var line = read.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }
                    string[] parcala = line.Split(',');

                    list.Add(new SiteConfig
                    {
                        Id = int.Parse(parcala[0]),
                        Address = parcala[1],
                        IsActive = bool.Parse(parcala[2]),
                        PeriodInMinutes = int.Parse(parcala[3]),
                        RefreshMethod = (EnumRefreshMethod)(Enum.Parse(typeof(EnumRefreshMethod), parcala[4])),
                        LastExecutionTime = DateTime.Parse(parcala[5])
                    });
                }

            }

            return list;
        }

        public override void WriteSiteConfig(SiteConfig siteConfig)
        {
            var originalLines = System.IO.File.ReadAllLines(ConfigurationManager.AppSettings["FilePath"] + "\\sites.txt");

            var uplatedLines = new List<string>();
            foreach (var line in originalLines)
            {
                string[] parcala = line.Split(',');
                if(parcala[0]==siteConfig.Id.ToString())
                {
                    parcala[0] = siteConfig.Id.ToString();
                    parcala[1] = siteConfig.Address;
                    parcala[2] = siteConfig.IsActive.ToString() ;
                    parcala[3] = siteConfig.PeriodInMinutes.ToString();
                    parcala[4] = siteConfig.RefreshMethod.ToString();
                    parcala[5] = siteConfig.LastExecutionTime.ToString();
                }
                uplatedLines.Add(string.Join(",", parcala));
            }

            System.IO.File.WriteAllLines(ConfigurationManager.AppSettings["FilePath"] + "sites.txt",uplatedLines);

            //throw new NotImplementedException();
        }
    }
}
