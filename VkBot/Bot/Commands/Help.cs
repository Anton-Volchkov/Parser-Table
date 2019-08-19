using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Help : IBotCommand
    {
        public string[] Alliases { get; set; } = {"помощь","хелп","команды","помоги"};
        public string Description { get; set; } = "Команда !Бот команды возвращает вам список доступных команд." +
                                                 "\nПример: !Бот команды ";

        public async Task<string> Execute(Message msg)
        {
            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("***КОМАНДЫ БОТА****");
            strBuilder.AppendLine("Перед каждой командой нужно ставить восклицательный знак.\nПример: !Команда");
            strBuilder.AppendLine("_____________").AppendLine();
            strBuilder.AppendLine("!Бот команды");
            strBuilder.AppendLine("!Бот ифно + номер зачётки");
            strBuilder.AppendLine("_____________").AppendLine();

            return strBuilder.ToString();
        }
    }
}
