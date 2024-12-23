﻿using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class UptimeCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "uptime";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            string uptimeString =
                Vars.CurrentLang.Message_UptimeInfo
                    .Replace("$1", new TimeSpan(0, 0, 0, 0, Environment.TickCount).ToString())
                    .Replace("$2", Vars.StartSW.Elapsed.ToString());
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                uptimeString,
                parseMode: ParseMode.Markdown,
                linkPreviewOptions: false,
                disableNotification: Vars.CurrentConf.DisableNotifications,
                replyParameters: update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
