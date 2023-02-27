using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.UserInteraction;

namespace TelegramGame.Bot.Core;

public class StreetFightBot : ITelegramBot
{
    private readonly TelegramBotClient _botClient;
    private readonly CancellationTokenSource _cancellationToken;
    private readonly UpdateProcessor _updateProcessor;

    public StreetFightBot(string token)
    {
        _botClient = new(token);
        _cancellationToken = new();
        _updateProcessor = new(this);
    }

    public void Run()
    {
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: _cancellationToken.Token
        );
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is { } message)
        {
            var chatId = message.Chat.Id;
            _updateProcessor.ProcessCommands(chatId, update);
        }
    }

    public async Task<Message> SendMessage(long chatId, Answer answer)
    {
        if (answer.HasPhoto)
        {
            return await _botClient.SendPhotoAsync(
                chatId: chatId,
                photo: new InputMedia(System.IO.File.OpenRead(answer.PhotoPath), "photo"),
                caption: answer.Text,
                replyMarkup: answer.ReplyKeyboardMarkup,
                parseMode: ParseMode.Html);
        }

        return await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: answer.Text,
            replyMarkup: answer.ReplyKeyboardMarkup,
            parseMode: ParseMode.Html);
    }

    public async Task<Message> EditMessage(long chatId, int messageId, Answer answer)
    {
        if (answer.HasPhoto)
        {
            return await _botClient.EditMessageMediaAsync(
                chatId: chatId,
                messageId: messageId,
                replyMarkup: answer.ReplyKeyboardMarkup,
                media: new InputMediaPhoto(new InputMedia(System.IO.File.OpenRead(answer.PhotoPath), "photo")));
        }
        
        return await _botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: answer.Text,
            replyMarkup: answer.ReplyKeyboardMarkup,
            parseMode: ParseMode.Html);
    }

    public void DeleteMessage(long chatId, int messageId)
    {
        _botClient.DeleteMessageAsync(chatId: chatId, messageId: messageId);
    }
    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}