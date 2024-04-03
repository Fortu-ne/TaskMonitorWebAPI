using TaskMonitorWebAPI.Models;

namespace TaskMonitorWebAPI.Interface
{
    public interface IReminder
    {
        ICollection<Reminder> GetAll();
        bool Create(Reminder task);
        bool Delete(Reminder task);
        bool Update(Reminder model);
        Reminder GetById(int id);
        Reminder GetByName(string name);
        bool DoesItExist(string name);
        bool DoesItExist(int id);
        bool Save();
    }
}
