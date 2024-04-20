using Microsoft.VisualBasic;
using System.Collections;
using TaskMonitorWebAPI.Data;
using TaskMonitorWebAPI.Dto;
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
            return _context.Tasks.Any(x => x.Title == name);
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
           return _context.Tasks.FirstOrDefault(t => t.Title == name);
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

        public ICollection<Tasks> GetUpComingTasks(DateOnly referenceDate)
        {
            var now = DateTime.Now;
            return _context.Tasks
                .Where(t =>                 
                    (DateOnly.FromDateTime(t.DueDate) == referenceDate /*&& t.DueDate < now.AddHours(24)*/)  ||
                    (t.ReminderSet && DateOnly.FromDateTime(t.ReminderTime.Value) == referenceDate && t.ReminderTime < now.AddHours(24))
                )
                .ToList();
        }

        public ICollection<Tasks> GetUpComingTasks()
        {
            
                var now = DateTime.Now; // Get current time

            /*
             * currnet date : 2024-04-15 10:10
             * reminder date : 2024-04-15 10:20
             * due date : 2024-04-15 10:30
             * 
             * if(
             * 
             * 
             * 
             */

            return _context.Tasks
            .Where(t =>
          // Filter based on reminder and due date
          (t.DueDate >= now && t.DueDate < now.AddHours(2)) ||
          (t.ReminderSet && t.ReminderTime >= now && t.ReminderTime < now.AddHours(2))
            ).ToList();

        }

        public ICollection<Tasks> GetAllByPriorities(Priorities request)
        {
            return _context.Tasks.Where(r=>r.Priorities == request).ToList();
        }

        public ICollection<Tasks> GetByUserId(int id)
        {
            return _context.Tasks.Where(r=>r.UserId1 == id).ToList();
        }

        public ICollection<Tasks> CheckForWeeklyTasks()
        {
            var date =  DateTime.Now;

            if(date.DayOfWeek == DayOfWeek.Sunday)
            {
                var startingDay = date.AddDays(1);

                var endingDay = date.AddDays(7).AddHours(23).AddMinutes(59);

                return _context.Tasks.Where(r=>r.DueDate >= startingDay && r.DueDate < endingDay).ToList();
            }

            return _context.Tasks.ToList();
        }

     
    }
}
