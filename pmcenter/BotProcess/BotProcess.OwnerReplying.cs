﻿using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task OwnerReplying(Update update)
        {
            // check anonymous forward (5.5.0 new feature compatibility fix)
            Conf.MessageIDLink link = Methods.GetLinkByOwnerMsgID(update.Message.ReplyToMessage.MessageId);
            User? forwardFrom = null;
            if (link != null && !link.IsFromOwner)
            {
                Log(
                    $"Found corresponding message link for message #{update.Message.ReplyToMessage.MessageId}, which was actually forwarded from {link.TGUser.Id}, patching user information from database...",
                    "BOT");
                forwardFrom = link.TGUser;
            }

            if (forwardFrom == null && update.Message.Text.ToLowerInvariant() != "/retract")
            {
                // The owner is replying to bot messages. (no forwardfrom)
                _ = await Vars.Bot.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_CommandNotReplyingValidMessage,
                    parseMode: ParseMode.Markdown,
                    linkPreviewOptions: false,
                    disableNotification: Vars.CurrentConf.DisableNotifications,
                    replyParameters: update.Message.MessageId).ConfigureAwait(false);
                // The message is forwarded anonymously
                if (!string.IsNullOrEmpty(update.Message.ReplyToMessage.ForwardSenderName) &&
                    !Vars.CurrentConf.DisableMessageLinkTip)
                {
                    _ = await Vars.Bot.SendTextMessageAsync(
                        update.Message.From.Id,
                        Vars.CurrentLang.Message_MsgLinkTip,
                        parseMode: ParseMode.Markdown,
                        linkPreviewOptions: false,
                        disableNotification: Vars.CurrentConf.DisableNotifications,
                        replyParameters: update.Message.MessageId).ConfigureAwait(false);
                    Vars.CurrentConf.DisableMessageLinkTip = true;
                }

                return;
            }

            if (await commandManager.Execute(Vars.Bot, update).ConfigureAwait(false))
            {
                Vars.CurrentConf.Statistics.TotalCommandsReceived += 1;
                return;
            }

            // Is replying, replying to forwarded message AND not command.
            MessageId? replyToMsgId = null;
            if (Vars.CurrentConf.EnableOwnerReplyCopyMessage)
            {
                replyToMsgId = await Vars.Bot.CopyMessageAsync(
                    update.Message.ReplyToMessage.ForwardFrom.Id,
                    update.Message.Chat.Id,
                    update.Message.MessageId,
                    disableNotification: Vars.CurrentConf.DisableNotifications).ConfigureAwait(false);
            }
            else
            {
                replyToMsgId = (await Vars.Bot.ForwardMessageAsync(
                    update.Message.ReplyToMessage.ForwardFrom.Id,
                    update.Message.Chat.Id,
                    update.Message.MessageId,
                    disableNotification: Vars.CurrentConf.DisableNotifications).ConfigureAwait(false)).MessageId;
            }

            // Cc owner's reply.
            if (Vars.CurrentConf.EnableCc)
            {
                await RunCc(update);
            }


            if (Vars.CurrentConf.EnableMsgLink)
            {
                Log(
                    $"Recording message link: user {replyToMsgId} <-> owner {update.Message.MessageId}, user: {update.Message.ReplyToMessage.ForwardFrom.Id}",
                    "BOT");
                Vars.CurrentConf.MessageLinks.Add(
                    new Conf.MessageIDLink
                    {
                        OwnerSessionMessageID = update.Message.MessageId, UserSessionMessageID = replyToMsgId,
                        TGUser = update.Message.ReplyToMessage.ForwardFrom, IsFromOwner = true
                    }
                );
                // Conf.SaveConf(false, true);
            }

            Vars.CurrentConf.Statistics.TotalForwardedFromOwner += 1;
            // Process locale.
            if (Vars.CurrentConf.EnableRepliedConfirmation)
            {
                string replyToMessage = Vars.CurrentLang.Message_ReplySuccessful;
                replyToMessage = replyToMessage.Replace("$1",
                    $"[{Methods.GetComposedUsername(update.Message.ReplyToMessage.ForwardFrom)}](tg://user?id={update.Message.ReplyToMessage.ForwardFrom.Id})");
                _ = await Vars.Bot.SendTextMessageAsync(update.Message.From.Id, replyToMessage,
                    parseMode: ParseMode.Markdown,
                    linkPreviewOptions: false,
                    disableNotification: Vars.CurrentConf.DisableNotifications,
                    replyParameters: update.Message.MessageId).ConfigureAwait(false);
            }

            Log(
                $"Successfully passed owner's reply to {Methods.GetComposedUsername(update.Message.ReplyToMessage.ForwardFrom, true, true)}",
                "BOT");
        }
    }
}
