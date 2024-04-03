
using TaskMonitorWebAPI.Entities;

namespace TaskMonitorWebAPI.Dto
{
    public class ReminderDto
    {
        public String Notification { get; set; }
        public DateTime ReminderDate { get; set; }
        public int TaskID { get; set; }
        public Tasks Task { get; set; }
        //public ICollection<User> Users { get; set; }
    }
}
