using System;
using System.Threading.Tasks;
using pmcenter.CallbackActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static pmcenter.Methods;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class BotProcess
    {
        private static async Task CallbackQueryRoute(Update update)
        {
            if (update.CallbackQuery == null)
            {
                return;
            }

            if (update.CallbackQuery.From.IsBot)
            {
                return;
            }

            try
            {
                Conf.MessageIDLink link = GetLinkByOwnerMsgID(update.CallbackQuery.Message.ReplyToMessage.MessageId);
                if (link == null)
                {
                    throw new NullReferenceException(Vars.CurrentLang.Message_Action_LinkNotFound);
                }

                CallbackActionResult result = await CallbackProcess.DoCallback(update.CallbackQuery.Data, link.TGUser,
                    update.CallbackQuery.Message);
                // prompt result
                await Vars.Bot.AnswerCallbackQueryAsync(update.CallbackQuery.Id, result.Status, result.ShowAsAlert);
                // update existing buttons
                if (result.Succeeded)
                {
                    _ = await Vars.Bot.EditMessageReplyMarkupAsync(
                        update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId,
                        new InlineKeyboardMarkup(CallbackProcess.GetAvailableButtons(update)));
                }
            }
            catch (Exception ex)
            {
                Log($"Unable to process action {update.CallbackQuery.Data}: {ex}", "BOT", LogLevel.Error);
                await Vars.Bot.AnswerCallbackQueryAsync
                (
                    update.CallbackQuery.Id,
                    Vars.CurrentLang.Message_Action_ErrorWithDetails.Replace("$1", ex.Message),
                    true
                );
            }
        }
    }
}
