﻿namespace TelegramGame.HarvestedMessages;

public class StartCommandMessages
{
    public static string GetMessageText(string? name)
    {
        string text = $@"Привет, {name}!
С возвращением в бойцовский клуб!";
        return text;
    }

    public static string GetMessageButton()
    {
        return "Главное меню:/menu";
    }
}