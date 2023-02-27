using TelegramGame.Bot.Core;

namespace TelegramGame.User;

public class Display
{
   public long ChatId;
   private int messageId;
   private bool isFirstMessage = true;
   private ITelegramBot bot;
   public Display(long chatId, ITelegramBot bot)
   {
      ChatId = chatId;
      this.bot = bot;
   }
   public void UpdateMainMessage(Answer answer)
   {
      if (isFirstMessage)
      {
         messageId = bot.SendMessage(ChatId, answer).Result.MessageId;
         isFirstMessage = false;
      }
      else
         bot.EditMessage(ChatId, messageId, answer);
   }
}