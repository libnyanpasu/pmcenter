using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class ResetConfCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "resetconf";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            if (Vars.IsResetConfAvailable)
            {
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_ConfReset_Started,
                    parseMode: ParseMode.Markdown,
                    linkPreviewOptions: false,
                    disableNotification: Vars.CurrentConf.DisableNotifications,
                    replyParameters: update.Message.MessageId).ConfigureAwait(false);
                long ownerId = Vars.CurrentConf.OwnerUID;
                string apiKey = Vars.CurrentConf.APIKey;
                Vars.CurrentConf = new Conf.ConfObj
                {
                    OwnerUID = ownerId,
                    APIKey = apiKey
                };
                _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
                Vars.CurrentLang = new Lang.Language();
                _ = await Lang.SaveLang().ConfigureAwait(false);
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_ConfReset_Done,
                    parseMode: ParseMode.Markdown,
                    linkPreviewOptions: false,
                    disableNotification: Vars.CurrentConf.DisableNotifications,
                    replyParameters: update.Message.MessageId).ConfigureAwait(false);
                await Methods.ExitApp(0);
                return true;
            }

            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_ConfReset_Inited,
                parseMode: ParseMode.Markdown,
                linkPreviewOptions: false,
                disableNotification: Vars.CurrentConf.DisableNotifications,
                replyParameters: update.Message.MessageId).ConfigureAwait(false);
            Vars.ConfValidator = new Thread(() => Methods.ThrDoResetConfCount());
            Vars.ConfValidator.Start();
            return true;
        }
    }
}
