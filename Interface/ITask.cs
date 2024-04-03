using TaskMonitorWebAPI.Entities;

namespace TaskMonitorWebAPI.Interface
{
    public interface ITask
    {
        ICollection<Tasks> GetAll();
        bool Create(Tasks task);
        bool Delete(Tasks task);
        bool Update (Tasks tasks);
        Tasks GetById(int id);
        Tasks GetByName(string name);

        ICollection<Tasks> DayOfTheWeek(DayOfWeek day);
        bool DoesExist(string name);
        bool DoesExist(int id);
        bool Save();
    }
}
