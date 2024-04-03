

namespace TaskMonitorWebAPI.Dto
{
    public class TasksDto
    {
        public String TaskName { get; set; }
        public String TaskDescription { get; set; }
        public Boolean Completed { get; set; }
        public DateTime DeadLine { get; set; }

    
    }
}
