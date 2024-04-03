

using TaskMonitorWebAPI.Data;
using TaskMonitorWebAPI.Interface;
using TaskMonitorWebAPI.Models;

namespace TaskMonitorWebAPI.Repository
{
    public class ReminderRep : IReminder
    {

        private readonly DataContext _context;

        public ReminderRep(DataContext context)
        {
            _context = context;
        }
        public bool Create(Reminder model)
        {
            _context.Reminders.Add(model);
            return Save();
        }

        public bool Delete(Reminder model)
        {
            _context.Reminders.Remove(model);
            return Save();
        }

        public bool DoesItExist(int id)
        {
            return _context.Reminders.Any(r => r.Id == id);
        }

        public bool DoesItExist(string name)
        {
            return _context.Reminders.Any(r=>r.Notification == name);
        }


        public ICollection<Reminder> GetAll()
        {
            return _context.Reminders.ToList();
        }

        public Reminder GetByName(string name)
        {
            return _context.Reminders.FirstOrDefault(r => r.Notification == name);
        }

        public Reminder GetById(int id)
        {
            return _context.Reminders.FirstOrDefault(r => r.Id == id);
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool Update(Reminder model)
        {
            _context.Reminders.Update(model);
            return Save();
        }
    }
}
