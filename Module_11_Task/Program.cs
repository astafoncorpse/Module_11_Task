using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Module_11_Task.Controllers;
using Module_11_Task.Services;
using Module_11_Task.Configuration;

namespace Module_11_Task
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<IStorage, MemoryStorage>();
            // Подключаем контроллеры сообщений и кнопок

            services.AddTransient<TextMessageController>();
            services.AddSingleton< InlineKeyboardController>();
            services.AddSingleton<IStorage, MemoryStorage>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("5994493788:AAEpKVwDIvIZ5Y1GL4RE78UIbI0ADVrW_qQ"));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "5994493788:AAEpKVwDIvIZ5Y1GL4RE78UIbI0ADVrW_qQ"
            };
        }
    }
}