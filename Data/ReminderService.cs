//using Microsoft.EntityFrameworkCore;
//using System.Net.Mail;
//using System.Net;
//using TaskMonitorWebAPI.Entities;
//using TaskMonitorWebAPI.Interface;

//namespace TaskMonitorWebAPI.Data
//{
//    public class ReminderService : BackgroundService
//    {
//        private readonly ITask _taskRep;
//        private readonly IConfiguration _config;
//        private readonly ILogger<ReminderService> _logger; // Added for logging
//        private Timer _timer;
//        private DataContext _context;

//        public ReminderService(ITask taskRep, IConfiguration config, ILogger<ReminderService> logger, DataContext context)
//        {
//            _taskRep = taskRep;
//            _config = config;
//            _logger = logger;
//            _context = context;
//        }

//        public Task StartAsync(CancellationToken cancellationToken)
//        {
//            _timer = new Timer(CheckAndSendReminders, null, TimeSpan.Zero, TimeSpan.FromDays(1)); // Run every day
//            _logger.LogInformation("Reminder service started."); // Log service start
//            return Task.CompletedTask;
//        }

//        private void CheckAndSendReminders(object state)
//        {
//            try
//            {
//                var upcomingTasks = _taskRep.GetUpComingTasks();
//                foreach (var task in upcomingTasks)
//                {
//                    if (task.ReminderSet && !string.IsNullOrEmpty(task.User.EmailAddress))
//                    {
//                        var reminderDeadline = task.DueDate.Subtract(task.ReminderTime.Value);
//                        if (DateTime.Now.TimeOfDay >= reminderDeadline)
//                        {
//                            SendTaskReminder(task);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while checking and sending reminders."); // Log errors
//            }
//        }

//        private void SendTaskReminder(Tasks task)
//        {
//            var clientEmail = _config.GetSection("Details").GetSection("Email").Value;
//            var password = _config.GetSection("Details").GetSection("Password").Value;
//            var user = _context.User.FirstOrDefault(r => r.Id == task.UserId1);

//            if (string.IsNullOrEmpty(user.EmailAddress))
//            {
//                _logger.LogWarning($"Email address not set for task {task.Title}"); // Log missing email
//                return;
//            }


//            // Compose the email content 
//            string subject = $"Reminder: {task.Title} due on {task.DueDate}";
//            string body = $"This is a reminder for your task: {task.Title}.\nDescription: {task.Description}";

//            // Send the email using SmtpClient
//            SmtpClient client = new SmtpClient();
//            client.Credentials = new NetworkCredential(clientEmail, password);
//            client.Port = 587;
//            client.Host = "smtp.office365.com";
//            client.EnableSsl = true;


//            MailMessage message = new MailMessage(clientEmail, user.EmailAddress);
//            message.Subject = subject;
//            message.Body = body;


//            try
//            {
//                client.Send(message);
//                Console.WriteLine($"Reminder email sent for task: {task.Title}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error sending reminder email: {ex.Message}");
//            }
//        }

//        public Task StopAsync(CancellationToken cancellationToken)
//        {
//            _timer?.Dispose();
//            _logger.LogInformation("Reminder service stopped."); // Log service stop
//            return Task.CompletedTask;
//        }

//        public void Dispose()
//        {
//            _timer?.Dispose();
//        }

//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
