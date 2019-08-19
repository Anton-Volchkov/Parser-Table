using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace BRUParserTable
{
   public class BRUParser
   {
        WebClient webClient = new WebClient();

        string Return;

        public async Task<string> GetTable(int number)
        {
            string page = webClient.DownloadString($"http://vuz2.bru.by/rate/{number}/");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            var query = from table in doc.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                        from row in table.SelectNodes("tr").Cast<HtmlNode>()
                        from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
                        select new { Table = table.Id, CellText = cell.InnerText };

            foreach (var cell in query)
            {
                Return += string.Format("{0}: {1}", cell.Table, cell.CellText);
            }

            return Return;
        }
   }
}
