using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.Models.Enums;
using TodoListBlazor.Api.Repositories;

namespace TodoListBlazor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        //api/tasks?name=
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TaskListSearch taskListSearch)
        {
            var tasks = await _taskRepository.GetTaskList(taskListSearch);
            var taskDto = tasks.Select(x => new TaskDto
            {
                Status = x.Status,
                Name = x.Name,
                AssigneeId = x.AssigneeId,
                CreatedDate = x.CreatedDate,
                Priority = x.Priority,
                Id = x.Id,
                AssigneeName = x.Assignee != null ? x.Assignee.FirstName + ' ' + x.Assignee.LastName : "N/A"
            });

            return Ok(taskDto);

        }
        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = await _taskRepository.Create(new Entities.Task
            {
                Name = request.Name,
                Priority = request.Priority.HasValue ? request.Priority.Value : Priority.Low,
                Status = Status.Open,
                CreatedDate = DateTime.Now,
                Id = request.Id
            });
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, TaskUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var taskFromDB = await _taskRepository.GetById(id);
            if (taskFromDB == null)
                return NotFound($"{id} is not found.");

            taskFromDB.Name = request.Name;
            taskFromDB.Priority = request.Priority;

            var taskResult = await _taskRepository.Update(taskFromDB);

            return Ok(new TaskDto
            {
                Name = taskResult.Name,
                Status = taskResult.Status,
                Id = taskResult.Id,
                Priority = taskResult.Priority,
                CreatedDate = taskResult.CreatedDate
            });

        }
        //api/tasks/xxx
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
                return NotFound($"{id} is not found.");
            return Ok(new TaskDto
            {
                Name = task.Name,
                Status = task.Status,
                Id = task.Id,
                Priority = task.Priority,
                CreatedDate = task.CreatedDate
            });
        }
    }
}
