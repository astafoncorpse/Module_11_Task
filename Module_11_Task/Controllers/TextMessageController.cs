using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Module_11_Task.Services;

namespace Module_11_Task.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            
            if (message.Text == "/start")
            {
                Console.WriteLine("Отправлено меню выбора");

                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                        InlineKeyboardButton.WithCallbackData($" Длинна сообщения" , $"LM"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"Sum")
                });

                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"  Мой бот умеет считать длинну сообщения или сумму цифр. {Environment.NewLine}" +
                    $"{Environment.NewLine}Выбери действие.{Environment.NewLine}", cancellationToken: ct, replyMarkup: new InlineKeyboardMarkup(buttons));
            }
            else 
            {
                string text = string.Empty;
                var actionCode = _memoryStorage.GetSession(message.From.Id).ActionCode;

                
                if (actionCode == "LM")
                {
                    text = $"Длина сообщения равна {message.Text.Length}";
                }

                
                if (actionCode == "Sum")
                {
                    
                    var numbersArray = message.Text.Split(' '); 
                    var sumResult = 0;
                   
                    foreach (string number in numbersArray)
                    {
                        
                        if (int.TryParse(number, out int parseResult))
                        {
                            sumResult += parseResult;
                        }
                    }

                    text = $"сумма чисел равна {sumResult}";
                }


                _memoryStorage.GetSession(message.From.Id).ActionCode = string.Empty;

                text += $"{Environment.NewLine}. Напишите команду /start снова чтобы перейти в главное меню";
                
              
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, text, cancellationToken: ct);
            }

           
        }
    }
}