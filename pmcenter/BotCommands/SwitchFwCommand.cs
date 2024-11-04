using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class SwitchFwCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "switchfw";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            bool isPausedNow = Conf.SwitchPaused();
            _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
            _ = await botClient.SendTextMessageAsync(update.Message.From.Id,
                isPausedNow ? Vars.CurrentLang.Message_ServicePaused : Vars.CurrentLang.Message_ServiceResumed,
                parseMode: ParseMode.Markdown,
                linkPreviewOptions: false,
                disableNotification: Vars.CurrentConf.DisableNotifications,
                replyParameters: update.Message.MessageId).ConfigureAwait(false);

            return true;
        }
    }
}
