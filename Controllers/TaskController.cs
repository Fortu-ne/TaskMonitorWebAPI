using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskMonitorWebAPI.Data;
using System;
using System.Timers;
using TaskMonitorWebAPI.Dto;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Interface;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using Azure.Core;
using static Org.BouncyCastle.Math.EC.ECCurve;




namespace TaskMonitorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TaskController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ITask _taskRep;
        private readonly DataContext _context;


        // Reminder
        private System.Timers.Timer aTimer;

        //private readonly ITask _taskRep;
        private readonly IConfiguration config;
       // private readonly DataContext _context;



        public TaskController(ITask taskRep, IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _taskRep = taskRep;
            _context = context;
           
        }


        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Index()
        {
            var results = _mapper.Map<List<Tasks>>(_taskRep.GetAll());

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            return Ok(results);
        }

        [HttpGet("ReminderSet")]
        [ProducesResponseType(200)]
        public IActionResult IndexByReminder()
        {
            SetTimer();
            var results = _mapper.Map<List<Tasks>>(_taskRep.GetUpComingTasks());

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            return Ok(results);
        }


        [HttpGet("Priorities/{pri:int}")]
        [ProducesResponseType(200)]
        public IActionResult IndexOfPriorities(Priorities pri)
        {
            var results = _mapper.Map<List<Tasks>>(_taskRep.GetAllByPriorities(pri));

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            return Ok(results);
        }


        [HttpGet("ByDay")]
        [ProducesResponseType(200)]
        public IActionResult indexbyday([FromQuery] string day)
        {

            DateOnly model = new DateOnly();

            if(day != null)
            {
               //model = DateOnly.ParseExact(day, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                model = DateOnly.ParseExact(day, "yyyy-MM-dd");
            }
            var results = _mapper.Map<List<Tasks>>(_taskRep.GetUpComingTasks(model));

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            return Ok(results);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create([FromBody] TasksDto request)
        {

            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var model = _taskRep.GetAll().Where(r => r.Title.Trim().ToLower() == request.Title.Trim().ToLower()).FirstOrDefault();

            if (model != null)
            {
                ModelState.AddModelError(" ", "task already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            DateTime reminderTime = new DateTime();
            if (request.ReminderSet)
            {
                reminderTime = DateTime.ParseExact(request.ReminderTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
            var dueDate = DateTime.ParseExact(request.DueDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);


            var taskMapper = _mapper.Map<Tasks>(new Tasks
            {
                Title = request.Title,
                Description = request.Description,
                CreatedDate = DateTime.Now,
                Completed = false,
                DueDate = dueDate,
                ReminderSet = request.ReminderSet,
                ReminderTime = reminderTime,
                Priorities = request.Priorities,
                UserId1 = request.UserId

            });

            if (!_taskRep.Create(taskMapper))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return NoContent();
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]

        public IActionResult Update(int id, [FromBody] TasksDto request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }

            var verify = _taskRep.DoesExist(id);

            if (!verify)
            {
                ModelState.AddModelError(" ", "task not found");
                return StatusCode(404, ModelState);
            }


            var model = _taskRep.GetById(id);
            var getUser = _context.User.FirstOrDefault(x => x.Id == model.UserId1);
            var dueDate = DateTime.Parse(request.DueDate);
            var reminderTime = DateTime.Parse(request.ReminderTime);

            var updateTask = _mapper.Map<Tasks>(new Tasks
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                DueDate = dueDate,
                ReminderTime = reminderTime,
                ReminderSet = request.ReminderSet,
                Completed = request.Completed,
                Priorities = request.Priorities,
                UserId1 = model.UserId1,
                User = getUser
            });

            if (updateTask != null)
            {
                _taskRep.Update(updateTask);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok("Succesffuly Updated");
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]

        public IActionResult GetTask(int id)
        {
            var model = _taskRep.DoesExist(id);

            if (!model)
            {
                ModelState.AddModelError(" ", "task not found");
                return StatusCode(404, ModelState);
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = _mapper.Map<TasksDto>(_taskRep.GetById(id));



            return Ok(task);
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]

        public IActionResult Delelte(int id)
        {
            var model = _taskRep.GetById(id);

            if (model == null)
            {
                ModelState.AddModelError(" ", "task not found");
                return StatusCode(404, ModelState);
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (model != null)
            {
                _taskRep.Delete(model);
            }


            return NoContent();
        }




        private void SetTimer()
        {
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
                var user = _context.User.FirstOrDefault(r => r.Id == task.UserId1);
                if (task.ReminderSet && !string.IsNullOrEmpty(user.EmailAddress)) // Check reminder set and email
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


        [HttpGet("Reminder")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        private void SendTaskReminder(Tasks task)
        {


            var clientEmail = config.GetSection("Details").GetSection("Email").Value;
            var password = config.GetSection("Details").GetSection("Password").Value;
            var user = _context.User.FirstOrDefault(r => r.Id == task.UserId1);



            if (string.IsNullOrEmpty(user.EmailAddress))
            {
                Console.WriteLine("Email address not set for task reminder.");
              
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


            MailMessage message = new MailMessage(clientEmail, user.EmailAddress);
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

