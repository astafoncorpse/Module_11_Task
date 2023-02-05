using Module_11_Task.Models;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;

namespace Module_11_Task.Services
{
    public class MemoryStorage : IStorage
    {
        
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        public Session GetSession(long chatId)
        {
            // Возвращаем сессию по ключу, если она существует
            if (_sessions.ContainsKey(chatId))
                return _sessions[chatId];

            // Создаем и возвращаем новую, если такой не было
            var newSession = new Session() { ActionCode = "LM" };
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}
