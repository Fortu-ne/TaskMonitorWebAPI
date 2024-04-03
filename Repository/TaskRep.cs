using TaskMonitorWebAPI.Data;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Interface;

namespace TaskMonitorWebAPI.Repository
{
    public class TaskRep : ITask
    {
        private readonly DataContext _context;
        public TaskRep(DataContext context)
        {
            _context = context;
        }

        public bool Create(Tasks task)
        {
            _context.Tasks.Add(task);
            return Save();
        }

        public bool Delete(Tasks task)
        {
            _context.Tasks.Remove(task);
            return Save();
        }

        public bool DoesExist(string name)
        {
            return _context.Tasks.Any(x => x.Name == name);
        }

        public bool DoesExist(int id)
        {
           return _context.Tasks.Any(r=>r.Id == id); ;
        }

        public Tasks GetById(int id)
        {
            return _context.Tasks.FirstOrDefault(r => r.Id == id);
        }

        public Tasks GetByName(string name)
        {
           return _context.Tasks.FirstOrDefault(t => t.Name == name);
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public ICollection<Tasks> GetAll()
        {
            return _context.Tasks.ToList();
        }

        public bool Update(Tasks tasks)
        {
            _context.Tasks.Update(tasks);
            return Save();
        }

        public ICollection<Tasks> DayOfTheWeek(DayOfWeek day)
        {
            return _context.Tasks.Where(r=>r.DeadLine.DayOfWeek == day).ToList();
        }
    }
}
