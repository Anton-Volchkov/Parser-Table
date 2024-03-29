﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot
{
    public class CommandExecutor
    {

        private const string ErrorMessage = "Я не знаю такой команды =(";
        private readonly IBotCommand[] Commands;
        public CommandExecutor(IEnumerable<IBotCommand> commands)
        {
            Commands = commands.ToArray();
        }

        // тут вся логика обработки команд
        public async Task<string> HandleMessage(Message msg)
        {
            var result = "";
            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var cmd = split[0].ToLower();

            foreach (var command in Commands)
            {
                if (!command.Alliases.Contains(cmd))
                {
                    continue;
                }

                result = await command.Execute(msg);
                break;
            }

            if (string.IsNullOrEmpty(result)) // если никакая из команд не выполнилась, посылаем сообщение об ошибке
            {
                result = ErrorMessage;
            }

            return result;
        }
    }
}
