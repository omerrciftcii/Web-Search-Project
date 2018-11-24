using SharedLibrary.Data;
using SharedLibrary.Data.Domain;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WebCrawler.Library
{
    public class Crawler
    {
        /*public string ReplaceText(string text)
        {
            text = text.Replace("Ä°", "İ").Replace("Ä±", "ı").Replace("Ã¼", "ü").Replace("ÅŸ", "ş").Replace("Å", "Ş").Replace("Ã§", "ç").Replace("Ã¶", "ö").Replace("ÄŸ", "ğ").Replace("Ã‡", "Ç").Replace("Ã–", "Ö").Replace("Ãœ", "Ü").Replace("â€Š", "-").Replace("â€", "'").Replace("™", "").Replace("Ä", "Ğ");
            string updatedString = Regex.Replace(text, @"^\s+$[\r\n\t]*", string.Empty, RegexOptions.Multiline); //^\s+$: İlk satırdan son satıra kadar tüm boş satıları siliyor(sadece tab ve space boşlukları da dahil). 
            return updatedString;
        }*/
        
        private string ConnectAndGetHtml(string address)
        {
            WebClient web_Client = new WebClient();
            string data = web_Client.DownloadString(address);
            data = data.Replace(">", "> ");
            return Parser(data);
        }

        private string Parser(string data)
        {
            string inner_Text;
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(data);
            var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body");
            inner_Text = htmlBody.InnerText;

            htmlDoc.DocumentNode.Descendants()
                .Where(n => n.Name == "script" || n.Name == "style")
                .ToList()
                .ForEach(n => n.Remove());

            inner_Text = htmlBody.InnerText;

            byte[] bytes = Encoding.Default.GetBytes(inner_Text);
            inner_Text = Encoding.UTF8.GetString(bytes);//2 bytelık özel karakterleri anlamlandırır.

            string resultString = Regex.Replace(inner_Text,@"^\s+$[\r\n]*",string.Empty,RegexOptions.Multiline);
            resultString = Regex.Replace(resultString," {2,}"," ");
            resultString = Regex.Replace(resultString, "\n", " ");
            resultString = Regex.Replace(resultString, "\r", " ");
            resultString = Regex.Replace(resultString, "\t", " ");
            while (resultString.Contains("  "))
                resultString = resultString.Replace("  "," ");
            return resultString;
        }



        public bool Execute(SiteConfig siteConfig)
        {
            // siteConifg.IsActive == false return false;
            // siteConfig.LastExectuionTime -- DateTimeNow -- siteConfig.Period. return 


            //todo: 
            // siteye baglan
            // htmli çek
            // parse et
            // parse edilmiş sonucu data kaynagina yaz. yazmak için resultmanager kullanilabilinir mi??
          
            if (!siteConfig.IsActive || (DateTime.Now - siteConfig.LastExecutionTime).TotalMinutes < siteConfig.PeriodInMinutes)
            {
                return false;
            }

            var resultManager = ResultManager.Create();

            var result = new Result
            {
                DateCreated = DateTime.Now,
                Text = ConnectAndGetHtml(siteConfig.Address)
            };


            
            resultManager.WriteResult(result, siteConfig);
            siteConfig.LastExecutionTime = result.DateCreated;


            //Console.WriteLine("Executing...:" + siteConfig.Id);

            // Console.WriteLine(siteConfig.LastExecutionTime);


            return true;
        }

    }
}
