using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.UpdateHelper;
using Update = Telegram.Bot.Types.Update;

namespace pmcenter.Commands
{
    internal class CheckUpdateCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "chkupdate";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                Update2 latest = await CheckForUpdatesAsync().ConfigureAwait(false);
                int currentLocalizedIndex = GetUpdateInfoIndexByLocale(latest, Vars.CurrentLang.LangCode);
                if (IsNewerVersionAvailable(latest))
                {
                    Vars.UpdatePending = true;
                    Vars.UpdateVersion = new Version(latest.Latest);
                    Vars.UpdateLevel = latest.UpdateLevel;
                    string updateString = Vars.CurrentLang.Message_UpdateAvailable
                        .Replace("$1", latest.Latest)
                        .Replace("$2", latest.UpdateCollection[currentLocalizedIndex].Details)
                        .Replace("$3", Methods.GetUpdateLevel(latest.UpdateLevel));
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        updateString,
                        parseMode: ParseMode.Markdown,
                        linkPreviewOptions: false,
                        disableNotification: Vars.CurrentConf.DisableNotifications,
                        replyParameters: update.Message.MessageId).ConfigureAwait(false);
                }
                else
                {
                    Vars.UpdatePending = false;
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_AlreadyUpToDate
                            .Replace("$1", latest.Latest)
                            .Replace("$2", Vars.AppVer.ToString())
                            .Replace("$3", latest.UpdateCollection[currentLocalizedIndex].Details),
                        parseMode: ParseMode.Markdown,
                        linkPreviewOptions: false,
                        disableNotification: Vars.CurrentConf.DisableNotifications,
                        replyParameters: update.Message.MessageId).ConfigureAwait(false);
                }

                return true;
            }
            catch (Exception ex)
            {
                string errorString = Vars.CurrentLang.Message_UpdateCheckFailed.Replace("$1", ex.Message);
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    errorString,
                    parseMode: ParseMode.Markdown,
                    linkPreviewOptions: false,
                    disableNotification: Vars.CurrentConf.DisableNotifications,
                    replyParameters: update.Message.MessageId).ConfigureAwait(false);
                return true;
            }
        }
    }
}
