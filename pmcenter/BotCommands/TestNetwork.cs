using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter.Commands
{
    internal class TestNetworkCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "testnetwork";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            Log("Starting network test...", "BOT");
            double latencyToGh =
                Math.Round((await TestLatency("https://github.com").ConfigureAwait(false)).TotalMilliseconds, 2);
            double latencyToTg =
                Math.Round((await TestLatency("https://api.telegram.org/bot").ConfigureAwait(false)).TotalMilliseconds,
                    2);
            double latencyToCi =
                Math.Round((await TestLatency("https://ci.appveyor.com").ConfigureAwait(false)).TotalMilliseconds, 2);
            _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                Vars.CurrentLang.Message_Connectivity
                    .Replace("$1", latencyToGh + "ms")
                    .Replace("$2", latencyToTg + "ms")
                    .Replace("$3", latencyToCi + "ms"),
                parseMode: ParseMode.Markdown,
                linkPreviewOptions: false,
                disableNotification: Vars.CurrentConf.DisableNotifications,
                replyParameters: update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
