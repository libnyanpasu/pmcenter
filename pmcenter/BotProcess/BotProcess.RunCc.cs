using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task RunCc(Update update)
        {
            Log("Cc enabled, forwarding...", "BOT");
            foreach (long id in Vars.CurrentConf.Cc)
            {
                if (update.Message.From.Id != id)
                {
                    Log($"Forwarding message to cc: {id}", "BOT");
                    try
                    {
                        Message forwardedMessageCc = await Vars.Bot.ForwardMessageAsync(id,
                            update.Message.From.Id,
                            update.Message.MessageId,
                            disableNotification: Vars.CurrentConf.DisableNotifications).ConfigureAwait(false);
                        // check if forwarded from channels
                        if (update.Message.ForwardFrom == null && update.Message.ForwardFromChat != null)
                        {
                            // is forwarded from channel
                            _ = await Vars.Bot.SendTextMessageAsync(id,
                                Vars.CurrentLang.Message_ForwarderNotReal
                                    .Replace("$2", update.Message.From.Id.ToString())
                                    .Replace("$1",
                                        "[" + update.Message.From.FirstName + " " + update.Message.From.LastName +
                                        "](tg://user?id=" + update.Message.From.Id + ")"),
                                parseMode: ParseMode.Markdown,
                                linkPreviewOptions: false,
                                disableNotification: Vars.CurrentConf.DisableNotifications,
                                replyParameters: update.Message.MessageId).ConfigureAwait(false);
                        }

                        if (update.Message.ForwardFrom != null && update.Message.ForwardFromChat == null)
                        {
                            // is forwarded from chats
                            // check real message sender
                            if (update.Message.ForwardFrom.Id != update.Message.From.Id)
                            {
                                _ = await Vars.Bot.SendTextMessageAsync(id,
                                    Vars.CurrentLang.Message_ForwarderNotReal
                                        .Replace("$2", update.Message.From.Id.ToString())
                                        .Replace("$1",
                                            "[" + update.Message.From.FirstName + " " + update.Message.From.LastName +
                                            "](tg://user?id=" + update.Message.From.Id + ")"),
                                    parseMode: ParseMode.Markdown,
                                    disableNotification: Vars.CurrentConf.DisableNotifications,
                                    replyParameters:
                                    forwardedMessageCc.MessageId).ConfigureAwait(false);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Unable to forward message to cc: {id}, reason: {ex.Message}", "BOT", LogLevel.Error);
                    }
                }
                else
                {
                    Log($"Current message is sent by cc user: {id}, skipped.");
                }
            }
        }
    }
}
