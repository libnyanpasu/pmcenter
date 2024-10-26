using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class RestartCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "restart";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_Restarting,
                parseMode: ParseMode.MarkdownV2,
                            protectContent: false,
                            disableNotification: Vars.CurrentConf.DisableNotifications,
                            messageThreadId: update.Message.MessageId).ConfigureAwait(false);
            Thread.Sleep(5000);
            await Methods.ExitApp(0);
            return true;
        }
    }
}
