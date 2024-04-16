
//    using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//    using Microsoft.AspNetCore.Mvc;
//    using TaskMonitorWebAPI.Data;
//    using TaskMonitorWebAPI.Dto;
//    using TaskMonitorWebAPI.Entities;
//    using TaskMonitorWebAPI.Interface;
//using TaskMonitorWebAPI.Models;
//using TaskMonitorWebAPI.Repository;


//namespace TaskMonitorWebAPI.Controllers
//    {
//        [Route("api/[controller]")]
//        [ApiController]
//        [Authorize]
    
    
//        public class ReeminderController : ControllerBase
//        {

//            private readonly IMapper _mapper;
//            private readonly IReminder _reminderRep;
//            private readonly DataContext _context;

//            public ReeminderController(IReminder reminderRep, IMapper mapper, DataContext context)
//            {
//                _mapper = mapper;
//                _reminderRep = reminderRep;
//                _context = context;
//            }


//            [HttpGet]
//            [ProducesResponseType(200)]
//            public IActionResult Index()
//            {
//                var results = _mapper.Map<List<Reminder>>(_reminderRep.GetAll());

//                if (!ModelState.IsValid)
//                {

//                    return BadRequest(ModelState);
//                }
//                return Ok(results);
//            }



//            [HttpPost]
//            [ProducesResponseType(200)]
//            public IActionResult Create([FromBody] ReminderDto request)
//            {

//                if (request == null)
//                {
//                    return BadRequest(ModelState);
//                }


//                var model = _reminderRep.GetAll().Where(r=>r.Notification == request.Notification).FirstOrDefault();

//                if (model != null)
//                {
//                    ModelState.AddModelError(" ", "task already exists");
//                    return StatusCode(422, ModelState);
//                }

//                if (!ModelState.IsValid)
//                    return BadRequest(ModelState);


//                var Mapper = _mapper.Map<Reminder>(new Reminder
//                {
//                   Notification = request.Notification,
//                   ReminderDate = DateTime.Now,
//                   TaskID = request.TaskID,
//                });

//                if (!_reminderRep.Create(Mapper))
//                {
//                    ModelState.AddModelError("", "Something went wrong while saving");
//                    return StatusCode(500, ModelState);

//                }
//                return NoContent();
//            }


//            [HttpPut("{id:int}")]
//            [ProducesResponseType(200)]
//            [ProducesResponseType(500)]

//            public IActionResult Update(int id, [FromBody] ReminderDto request)
//            {
//                if (request == null)
//                {
//                    return BadRequest(ModelState);
//                }

//                var verify = _reminderRep.DoesItExist(id);

//                if (!verify)
//                {
//                    ModelState.AddModelError(" ", "task not found");
//                    return StatusCode(404, ModelState);
//                }

//                var update = _mapper.Map<Reminder>(new Reminder
//                {
//                    Id = id,
//                    Notification = request.Notification,
//                    ReminderDate = request.ReminderDate,
//                    TaskID = request.TaskID,
                   

//                });

//                if (update != null)
//                {
//                    _reminderRep.Update(update);
//                }

//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                return Ok("Succesffuly Updated");
//            }


//            [HttpGet("{id:int}")]
//            [ProducesResponseType(200)]
//            [ProducesResponseType(500)]

//            public IActionResult GetReminder(int id)
//            {
//                var model = _reminderRep.DoesItExist(id);

//                if (!model)
//                {
//                    ModelState.AddModelError(" ", "task not found");
//                    return StatusCode(404, ModelState);
//                }


//                if (!ModelState.IsValid)
//                    return BadRequest(ModelState);

//                var reminder = _mapper.Map<ReminderDto>(_reminderRep.GetById(id));



//                return Ok(reminder);
//            }


//            [HttpDelete("{id:int}")]
//            [ProducesResponseType(200)]
//            [ProducesResponseType(500)]

//            public IActionResult Delelte(int id)
//            {
//                var model = _reminderRep.GetById(id);

//                if (model == null)
//                {
//                    ModelState.AddModelError(" ", "task not found");
//                    return StatusCode(404, ModelState);
//                }


//                if (!ModelState.IsValid)
//                    return BadRequest(ModelState);


//                if (model != null)
//                {
//                    _reminderRep.Delete(model);
//                }


//                return NoContent();
//            }


//        }
//    }



