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
            var confWritable = !(new FileInfo(Vars.ConfFile)).IsReadOnly;
            var langWritable = !(new FileInfo(Vars.LangFile)).IsReadOnly;
            _ = await botClient.SendTextMessageAsync(
                update.Message.From.Id,
                Vars.CurrentLang.Message_ConfAccess
                    .Replace("$1", confWritable.ToString())
                    .Replace("$2", langWritable.ToString())
                ,
                parseMode: ParseMode.Markdown,
                            protectContent: false,
                            disableNotification: Vars.CurrentConf.DisableNotifications,
                            messageThreadId: update.Message.MessageId).ConfigureAwait(false);
            return true;
        }
    }
}
