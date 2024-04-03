using TaskMonitorWebAPI.Models;

namespace TaskMonitorWebAPI.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime CreatedDate { get; set;}
        public Boolean Completed { get; set; }
        public DateTime DeadLine { get; set; }

        //relationships

       
        public ICollection<Reminder> Reminders { get; set; }
    }
}
