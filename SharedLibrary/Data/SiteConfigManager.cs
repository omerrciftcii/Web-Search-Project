using System.Collections.Generic;
using System.Configuration;
using SharedLibrary.Data.Domain;

namespace SharedLibrary.Data
{
    public abstract class SiteConfigManager
    {
        public abstract IEnumerable<SiteConfig> ReadSiteConfigs();

        public abstract void WriteSiteConfig(SiteConfig siteConfig);

        public virtual void WriteSiteConfigs(IEnumerable<SiteConfig> siteConfigs)
        {
            foreach (var siteConfig in siteConfigs)
            {
                WriteSiteConfig(siteConfig);
            }
        }

        public static SiteConfigManager Create()
        {
            var dataStoreType = ConfigurationManager.AppSettings["dataStoreType"];

            if (dataStoreType == "File")
                return new File.SiteConfigManager();
            else if (dataStoreType == "Db")
                return new Db.SiteConfigManager();

            return null;
        }
    }
}
