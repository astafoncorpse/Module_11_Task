using Module_11_Task.Services;
using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Module_11_Task.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            string text = string.Empty;

            _memoryStorage.GetSession(callbackQuery.From.Id).ActionCode = callbackQuery?.Data;

            
            if (callbackQuery?.Data == "LM")
            {
                text = $"Выбрано - Длина Сообщения.{Environment.NewLine}{Environment.NewLine}Введите сообщение";
            }

            if (callbackQuery?.Data == "Sum")
            {
                text = $"Выбрано - Сумма чисел.{Environment.NewLine}{Environment.NewLine}Введите числа через пробел";
            }

            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, text, cancellationToken: ct);
        }
    }
}