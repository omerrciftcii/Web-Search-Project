using System.Threading;
using WebCrawler.Library;
using SharedLibrary.Data;
using System;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var siteConfigManager = SiteConfigManager.Create();

            var siteConfigs = siteConfigManager.ReadSiteConfigs();

            var crawler = new Crawler();

            while (true)
            {
                Console.WriteLine("Döngü Başladı");

                foreach (var siteConfig in siteConfigs)
                {
                    Console.WriteLine("Site:" + siteConfig.Id);

                    if (crawler.Execute(siteConfig))
                    {
                        siteConfigManager.WriteSiteConfig(siteConfig);
                    }

                }
                Console.WriteLine("Döngü Bitti");

                Thread.Sleep(/*60 */ 1000);
            }
        }
    }
}
