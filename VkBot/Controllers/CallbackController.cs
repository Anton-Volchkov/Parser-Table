﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Bot;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        private readonly IConfiguration _configuration;

        private readonly IVkApi _vkApi;

        private readonly CommandExecutor commandExec;

        public CallbackController(IVkApi vkApi, IConfiguration configuration, CommandExecutor cmdExec)
        {
            _vkApi = vkApi;
            _configuration = configuration;
            commandExec = cmdExec;
        }

        [HttpPost]
        public async Task<IActionResult> Callback([FromBody] Updates updates)
        {
            // Проверяем, что находится в поле "type" 
            if (updates.Type == "confirmation")
            {
                // Отправляем строку для подтверждения 
                return Ok(_configuration["Config:Confirmation"]);
            }

            if (updates.Type == "message_new")
            {
                var msg = Message.FromJson(new VkResponse(updates.Object));

                //если сообщение НЕ НАЧИНАЕТСЯ С ЭТОГО, то ничо не делаем
                if (!msg.Text.ToLower().StartsWith("!бот"))
                {
                    return Ok("ok");
                }

                //а если начинается, то вот
                msg.Text = string.Join(' ', msg.Text.Split(' ').Skip(1)); // убираем !бот


                var text = await commandExec.HandleMessage(msg);

                // Отправим в ответ полученный от пользователя текст
                _vkApi.Messages.Send(new MessagesSendParams
                {
                    //TODO: плохой рандом ид
                    RandomId = new DateTime().Millisecond + Guid.NewGuid().ToByteArray().Sum(x => x),
                    PeerId = msg.PeerId.Value,
                    Message = text
                });
            }

            // Возвращаем "ok" серверу Callback API
            return Ok("ok");
        }
    }
}
