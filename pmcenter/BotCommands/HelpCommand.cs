using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class HelpCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "help";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            _ = await botClient.SendTextMessageAsync(
                Vars.CurrentConf.OwnerUID,
                Vars.CurrentLang.Message_Help,
                parseMode: ParseMode.MarkdownV2,
                            protectContent: false,
                            disableNotification: Vars.CurrentConf.DisableNotifications,
                            messageThreadId: update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
