using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace R6DetailedStats
{
    class HTMLDataParser
    {
        HttpClient httpClient;
        string html;
        private HtmlDocument doc;

        public HTMLDataParser()
        {
            httpClient = new HttpClient();
            doc = new HtmlDocument();
        }

        public async Task<bool> UpdateDocAsync(string url)
        {
            try
            {
                html = await httpClient.GetStringAsync(url);
                doc.LoadHtml(html);
                Console.WriteLine("Loaded HTML content from: " + url);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("HTML Parser returned with error code: " + ex);

                return false;
            }
        }

        public int ParseInt(string xPath)
        {
            int parsed;

            if (doc.DocumentNode.SelectSingleNode(xPath) != null)
            {
                parsed = int.Parse(doc.DocumentNode.SelectSingleNode(xPath).InnerText);
            }
            else
            {
                parsed = 0;
                Console.WriteLine("ERROR PARSING DATA");
            }

            return parsed;
        }

        public string ParseString(string xPath)
        {
            string parsed;

            if (doc.DocumentNode.SelectSingleNode(xPath) != null)
            {
                parsed = doc.DocumentNode.SelectSingleNode(xPath).InnerText;

                if (parsed == "J&#196;GER")
                    parsed = "JÄGER";
            }
            else
            {
                parsed = "DATA UNAVAILABLE";
                Console.WriteLine("ERROR PARSING DATA");
            }

            return parsed;
        }

        public string ParseImgUrl(string xPath)
        {
            string parsed;

            if (doc.DocumentNode.SelectSingleNode(xPath) != null)
            {
                parsed = doc.DocumentNode.SelectSingleNode(xPath).Attributes["src"].Value.ToString();
                Console.WriteLine(parsed);
            }
            else
            {
                parsed = "DATA UNAVAILABLE";
                Console.WriteLine("ERROR PARSING DATA");
            }

            return parsed;
        }
    }
}
