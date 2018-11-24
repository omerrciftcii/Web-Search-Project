using System.Collections.Generic;
using System.Configuration;
using SharedLibrary.Data.Domain;

namespace SharedLibrary.Data
{
    public abstract class ResultManager
    {
        public abstract IEnumerable<Result> ReadResults();

        public abstract void WriteResult(Result result, SiteConfig siteConfig);

        public static ResultManager Create()
        {
            var dataStoreType = ConfigurationManager.AppSettings["dataStoreType"];

            if (dataStoreType == "File")
                return new File.ResultManager();
            else if (dataStoreType == "Db")
                return new Db.ResultManager();

            return null;
        }
    }
}
