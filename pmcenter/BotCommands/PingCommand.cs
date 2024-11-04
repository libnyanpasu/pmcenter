using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class PingCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "ping";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                Vars.CurrentLang.Message_PingReply,
                parseMode: ParseMode.Markdown,
                linkPreviewOptions: false,
                disableNotification: Vars.CurrentConf.DisableNotifications,
                replyParameters: update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
