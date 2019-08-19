using ParserBRU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class StudentInfo : IBotCommand
    {
        public string[] Alliases { get; set; } = {"инфо","информация"};
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private readonly BRUParser _parser;
        public StudentInfo(BRUParser parser)
        {
            _parser = parser;
        }
        public async Task<string> Execute(Message msg)
        {
            var split = msg.Text.Split(' ', 2); // [команда, параметры]

            return await _parser.GetTable(split[1].Trim());
        }
    }
}
