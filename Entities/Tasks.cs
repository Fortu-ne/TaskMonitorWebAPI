using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskMonitorWebAPI.Dto;
using TaskMonitorWebAPI.Models;

namespace TaskMonitorWebAPI.Entities
{
    public class Tasks
    {

      

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Title { get; set; } = String.Empty;
        public String Description { get; set; } = String.Empty;
        public DateTime DueDate { get; set;}
        public Boolean Completed { get; set; }
        public DateTime CreatedDate { get; set; }
        public Boolean ReminderSet { get; set; } = false;
        public DateTime? ReminderTime { get; set; } 

        public Priorities Priorities { get; set; }


        [ForeignKey("UserId1")]
        public required int UserId1 { get; set; }
        public virtual User? User { get; set; }

    }
}
