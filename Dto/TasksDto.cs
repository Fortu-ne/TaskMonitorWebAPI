

using System.ComponentModel.DataAnnotations.Schema;
using TaskMonitorWebAPI.Entities;

namespace TaskMonitorWebAPI.Dto
{
    public class TasksDto
    {
       
        public String Title { get; set; } = String.Empty;
        public String Description { get; set; } = string.Empty;
        public string DueDate { get; set; } = String.Empty;
        public Boolean Completed { get; set; }

        public Priorities Priorities { get; set; }
        public Boolean ReminderSet { get; set; } = false;
        public string ReminderTime { get; set; } = String.Empty;
        public int UserId { get; set; }


    }
}
