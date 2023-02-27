using TelegramGame.Bot.Core;

if (Environment.GetEnvironmentVariable("token") is not { } token)
    return;
var botClient = new StreetFightBot(token);
botClient.Run();
Console.ReadLine();
