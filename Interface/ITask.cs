using TaskMonitorWebAPI.Entities;

namespace TaskMonitorWebAPI.Interface
{
    public interface ITask
    {
        ICollection<Tasks> GetAll();

        ICollection<Tasks> GetAllByPriorities(Priorities request);
        bool Create(Tasks task);
        bool Delete(Tasks task);
        bool Update (Tasks tasks);
        Tasks GetById(int id);
        Tasks GetByName(string name);

        ICollection<Tasks> GetUpComingTasks(DateOnly referenceDate);
        ICollection<Tasks> GetUpComingTasks();
        bool DoesExist(string name);
        bool DoesExist(int id);
        bool Save();
    }
}
