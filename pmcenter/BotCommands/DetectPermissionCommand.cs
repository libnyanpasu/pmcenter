using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace pmcenter.Commands
{
    internal class DetectPermissionCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "detectperm";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            bool confWritable = !new FileInfo(Vars.ConfFile).IsReadOnly;
            bool langWritable = !new FileInfo(Vars.LangFile).IsReadOnly;
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_ConfAccess
                    .Replace("$1", confWritable.ToString())
                    .Replace("$2", langWritable.ToString())
                ,
                parseMode: ParseMode.Markdown,
                linkPreviewOptions: false,
                disableNotification: Vars.CurrentConf.DisableNotifications,
                replyParameters: update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
