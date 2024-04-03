using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskMonitorWebAPI.Data;
using TaskMonitorWebAPI.Dto;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Interface;


namespace TaskMonitorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReminderController : ControllerBase
    {
      
       private readonly IMapper _mapper;
       private readonly ITask _taskRep;
       private readonly DataContext _context;

      public ReminderController(ITask taskRep, IMapper mapper, DataContext context)
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


        //[HttpGet("{day:DayOfWeek}")]
        //[ProducesResponseType(200)]
        //public IActionResult IndexByDay(DayOfWeek day)
        //{
        //    var results = _mapper.Map<List<Tasks>>(_taskRep.DayOfTheWeek(day));

        //    if (!ModelState.IsValid)
        //    {

        //        return BadRequest(ModelState);
        //    }
        //    return Ok(results);
        //}

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create([FromBody] TasksDto request)
        {

            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var model = _taskRep.GetAll().Where(r => r.Name.Trim().ToLower() == request.TaskName.Trim().ToLower()).FirstOrDefault();

            if (model != null)
            {
                ModelState.AddModelError(" ", "task already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var taskMapper = _mapper.Map<Tasks>(new Tasks
            {
                Name = request.TaskName,
                Description = request.TaskDescription,
                DeadLine = request.DeadLine,
                Completed = false,
                CreatedDate = DateTime.Now,
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

            var updateTask = _mapper.Map<Tasks>(new Tasks
            {
                Id = id,
                Name = request.TaskName,
                Description = request.TaskDescription,
                DeadLine = request.DeadLine,
                Completed = request.Completed,

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


    }
}

