namespace TelegramGame.HarvestedMessages;

public class MenuCommandMessages
{
    public static string GetButtons()
    {
        string menuString = @"В бой:menu battle
Мой персонаж:menu person
Магазин:menu shop";
        return menuString;
    }
}