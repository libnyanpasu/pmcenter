﻿using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;
using static pmcenter.Methods.UpdateHelper;
using Update = Telegram.Bot.Types.Update;

namespace pmcenter.Commands
{
    internal class UpdateCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "update";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                Update2 latest = await CheckForUpdatesAsync().ConfigureAwait(false);
                int currentLocalizedIndex = GetUpdateInfoIndexByLocale(latest, Vars.CurrentLang.LangCode);
                if (IsNewerVersionAvailable(latest))
                {
                    string updateString = Vars.CurrentLang.Message_UpdateAvailable
                        .Replace("$1", latest.Latest)
                        .Replace("$2", latest.UpdateCollection[currentLocalizedIndex].Details)
                        .Replace("$3", GetUpdateLevel(latest.UpdateLevel));
                    // \ update available! /
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        updateString,
                        parseMode: ParseMode.Markdown,
                        linkPreviewOptions: false,
                        disableNotification: Vars.CurrentConf.DisableNotifications,
                        replyParameters: update.Message.MessageId).ConfigureAwait(false);
                    // \ updating! /
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_UpdateProcessing,
                        parseMode: ParseMode.Markdown,
                        linkPreviewOptions: false,
                        disableNotification: Vars.CurrentConf.DisableNotifications,
                        replyParameters: update.Message.MessageId).ConfigureAwait(false);
                    // download compiled package
                    await DownloadUpdatesAsync(latest, currentLocalizedIndex).ConfigureAwait(false);
                    // update languages
                    if (Vars.CurrentConf.AutoLangUpdate)
                    {
                        await DownloadLangAsync().ConfigureAwait(false);
                    }

                    // \ see you! /
                    _ = await botClient.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_UpdateFinalizing,
                        parseMode: ParseMode.Markdown,
                        linkPreviewOptions: false,
                        disableNotification: Vars.CurrentConf.DisableNotifications,
                        replyParameters: update.Message.MessageId).ConfigureAwait(false);
                    Log("Exiting program... (Let the daemon do the restart job)", "BOT");
                    await ExitApp(0);
                    return true;
                    // end of difference
                }

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
                return true;
            }
            catch (Exception ex)
            {
                string errorString = Vars.CurrentLang.Message_UpdateCheckFailed.Replace("$1", ex.ToString());
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
