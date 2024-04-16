using System.Net.Mail;
using System.Net;
using System.Timers;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading;
using TaskMonitorWebAPI.Interface;
using TaskMonitorWebAPI.Data;
using MimeKit;

namespace TaskMonitorWebAPI
{
   
    public class Chatbot
    {
        
        private static System.Timers.Timer aTimer;
        private readonly ITask _taskRep;
        private readonly IConfiguration config;
        private readonly DataContext _context;
        public Chatbot(ITask taskRep ,DataContext context)
        {
            _taskRep = taskRep;
            _context = context; 
        }

        public Chatbot()
        {
            
        }

        private static void main(String[] agrs)
        {
            var set = new Chatbot();
            set.SetTimer();
        }
        private void SetTimer()
        {
            //  Create a timer with a two second interval.
            // aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 


            // Set up timer to run every 24 hours

            //aTimer = new System.Timers.Timer(24 * 60 * 60 * 1000); // Milliseconds in a day
        aTimer = new System.Timers.Timer(2000);
        aTimer.Elapsed += CheckAndSendReminders;
        aTimer.Start();
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

        private void CheckAndSendReminders(object sender, ElapsedEventArgs e)
        {
            ICollection<Tasks> upcomingTasks = _taskRep.GetUpComingTasks();

            foreach (Tasks task in upcomingTasks)
            {
                if (task.ReminderSet && !string.IsNullOrEmpty(task.User.EmailAddress)) // Check reminder set and email
                {


                    // Calculate reminder deadline considering due date and reminder offset
                    var reminderDeadline = task.DueDate.Subtract(task.ReminderTime.Value);

                    if (DateTime.Now.TimeOfDay >= reminderDeadline)
                    {
                        SendTaskReminder(task);
                    }
                }
            }
        }

        public void SendTaskReminder(Tasks task)
        {


           var clientEmail = config.GetSection("Details").GetSection("Email").Value;
           var password = config.GetSection("Details").GetSection("Password").Value;
           var user = _context.User.FirstOrDefault(r=>r.Id == task.UserId1);



        if (string.IsNullOrEmpty(user.EmailAddress))
        {
            Console.WriteLine("Email address not set for task reminder.");
            return;
        }

            // Compose the email content 
            string subject = $"Reminder: {task.Title} due on {task.DueDate}";
            string body = $"This is a reminder for your task: {task.Title}.\nDescription: {task.Description}";
           
            // Send the email using SmtpClient
            SmtpClient client = new SmtpClient(); 
            client.Credentials = new NetworkCredential(clientEmail, password);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.EnableSsl = true;

        
            MailMessage message = new MailMessage(clientEmail,user.EmailAddress);
            message.Subject = subject;
            message.Body = body;
         
 
        try
        {
            client.Send(message);
            Console.WriteLine($"Reminder email sent for task: {task.Title}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending reminder email: {ex.Message}");
        }
         }
}



    


}
