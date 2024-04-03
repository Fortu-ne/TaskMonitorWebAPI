using TaskMonitorWebAPI.Entities;

namespace TaskMonitorWebAPI.Models
{
    public class Reminder {

        public int Id { get; set; }
        public String Notification { get; set; }
        public DateTime ReminderDate { get; set; }
        public int TaskID { get; set; }
        public Tasks Task { get; set; }

        // Navigation property for users (many-to-many relationship)
        //public ICollection<User> Users { get; set; }
    }

}
