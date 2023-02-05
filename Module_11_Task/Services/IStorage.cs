using Module_11_Task.Models;

namespace Module_11_Task.Services
{
    public interface IStorage
    {
        
        Session GetSession(long chatId);
    }
}
