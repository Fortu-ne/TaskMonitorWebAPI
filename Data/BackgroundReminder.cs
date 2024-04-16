using System.Net.Mail;
using System.Net;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Interface;

namespace TaskMonitorWebAPI.Data
{

    public class BackgroundReminder : IHostedService, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly ILogger<BackgroundReminder> _logger;
        private readonly ITask _taskRep;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public BackgroundReminder(IConfiguration config, ILogger<BackgroundReminder> logger, ITask taskRep, IServiceScopeFactory serviceScopeFactory)
        {
            _config = config;
            _logger = logger;
            _taskRep = taskRep;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckAndSendReminders, null, TimeSpan.Zero, TimeSpan.FromDays(1)); // Run every day
            _logger.LogInformation("Reminder service started.");
            return Task.CompletedTask;
        }

        private void CheckAndSendReminders(object state)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                    var upcomingTasks = _taskRep.GetUpComingTasks(); // Use context for data access

                    foreach (var task in upcomingTasks)
                    {
                        var reminderDeadline = task.DueDate.Subtract(task.ReminderTime.Value);

                        if(task.ReminderSet && DateTime.Now.TimeOfDay >= reminderDeadline)
                        {
                             SendTaskReminder(task);

                        }
                      
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking and sending reminders.");
            }
        }

        private void SendTaskReminder(Tasks task)
        {
            var clientEmail = _config.GetSection("Details").GetSection("Email").Value;
            var password = _config.GetSection("Details").GetSection("Password").Value;

            using (var scope = _serviceScopeFactory.CreateScope()) // Create new scope for DataContext (optional)
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>(); // Access user email (optional)

                var user = context.User.FirstOrDefault(r => r.Id == task.UserId1); // Consider using dependency injection for DataContext

                if (string.IsNullOrEmpty(user?.EmailAddress))
                {
                    _logger.LogWarning($"Email address not set for task {task.Title}");
                    return;
                }

                // Compose the email content
                string subject = $"Reminder: {task.Title} due on {task.DueDate.Hour}";
                string body = $"This is a reminder for your task: {task.Title}.\nDescription: {task.Description}";

                // Send the email using SmtpClient
                using SmtpClient client = new SmtpClient();
                client.Credentials = new NetworkCredential(clientEmail, password);
                client.Port = 587;
                client.Host = "smtp.office365.com";
                client.EnableSsl = true;

                MailMessage message = new MailMessage(clientEmail, user.EmailAddress);
                message.Subject = subject;
                message.Body = body;

                try
                {
                    client.Send(message);
                    _logger.LogInformation($"Reminder email sent for task: {task.Title}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error sending reminder email: {ex.Message}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            _logger.LogInformation("Reminder service stopped.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
