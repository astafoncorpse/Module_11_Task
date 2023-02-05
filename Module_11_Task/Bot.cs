using Microsoft.Extensions.Hosting;
using Module_11_Task.Controllers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot.Polling;

namespace Module_11_Task   
{
    internal class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramClient;
        private TextMessageController _textMessageController;
        private InlineKeyboardController _inlineKeyboardController;

        public Bot(ITelegramBotClient telegramClient, InlineKeyboardController inlineKeyboardController, TextMessageController textMessageController)
        {
            _telegramClient = telegramClient;
            _textMessageController = textMessageController;
            _inlineKeyboardController = inlineKeyboardController;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, 
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }

        
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

           
            if (update.Type == UpdateType.Message)
            {
                await _textMessageController.Handle(update.Message, cancellationToken);
                return;
            }

        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
             var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}