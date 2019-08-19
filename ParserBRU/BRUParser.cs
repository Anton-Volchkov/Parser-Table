using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace ParserBRU
{
    public class BRUParser
    {

        StringBuilder strBuilder = new StringBuilder();

        string sendText = "";
   
        public async Task<string> GetTable(string number)
        {
            var enc1251 = CodePagesEncodingProvider.Instance.GetEncoding(1251);

            WebClient webClient = new WebClient()
            {
                Encoding = enc1251
            };
            string page = webClient.DownloadString($"http://vuz2.bru.by/rate/{number}/");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            var bodyNodeName = doc.DocumentNode.SelectSingleNode("//div[@class='box data']/h1");
            string Name = bodyNodeName.InnerText;

            var SkipLessons = doc.DocumentNode.SelectNodes("//div[@class='box data']/h2");
            string Skip = SkipLessons.Last().InnerText;

            List<List<string>> table = doc.DocumentNode.SelectSingleNode("//table")
                        .Descendants("tr")
                        .Skip(1)
                        .Where(tr => tr.Elements("td").Count() > 1)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                        .ToList();

            List<List<string>> firstRow = doc.DocumentNode.SelectSingleNode("//table")
                      .Descendants("tr")
                      .Where(tr => tr.Elements("th").Count() > 1)
                      .Select(tr => tr.Elements("th").Select(td => td.InnerText.Trim()).ToList())
                      .ToList();

            strBuilder.AppendLine($"{Name}\n{Skip}\n");

            foreach (var row in table)
            {
                string typeOfControl = row[0];
                foreach (var item in firstRow)
                {
                    for (int i = 1; i < item.Count; i++)
                    {
                        if (row[i] != "-" && !string.IsNullOrWhiteSpace(row[i]))
                        {
                            strBuilder.AppendLine($"{typeOfControl}({item[i]}): {row[i]}");
                        }
                      
                    }
                }
            }

            sendText = strBuilder.ToString().Replace("&#8209;", ".").Replace("ЭкзаменДата", "Дата Экз.").Replace("Экзамен", "Экз")
                               .Replace("дифференцированный", "диффер.")
                               .Replace("Иностранный язык", "Ин.яз")
                               .Replace("Информатика", "Информ")
                               .Replace("Языки программирования", "ЯП")
                               .Replace("Компьютерные технологии", "КТ")
                               .Replace("Основы компьютерного моделирования", "ОКМ")
                               .Replace("Элективные курсы по физической культуре", "Физра")
                               .Replace("Коррупция и ее общественная опасность", "Коррупция")
                               .Replace("курсовое проектирование", "Курс. Проект.")
                               .Replace("Автомобили и тракторы", "Авто и трак-ры")
                               .Replace("Инженерная графика", "Инж.Граф")
                               .Replace("проектирование", "проект.")
                               .Replace("Итоговый модуль", "Итог.Мод.")
                               .Replace("Математика", "Матем")
                               .Replace("Курсовое проектириеДата", "Дата КП")
                               .Replace("ЗачётДата", "Зач")
                               .Replace("-ый", "")
                               .Replace("-ой", "");
                   
            return sendText;
        }
    }
}
