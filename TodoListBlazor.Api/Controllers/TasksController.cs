using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.Models.Enums;
using TodoList.Models.SeedWork;
using TodoListBlazor.Api.Extensions;
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
            var pagedList = await _taskRepository.GetTaskList(taskListSearch);
            var taskDto = pagedList.Items.Select(x => new TaskDto
            {
                Status = x.Status,
                Name = x.Name,
                AssigneeId = x.AssigneeId,
                CreatedDate = x.CreatedDate,
                Priority = x.Priority,
                Id = x.Id,
                AssigneeName = x.Assignee != null ? x.Assignee.FirstName + ' ' + x.Assignee.LastName : "N/A"
            });

            return Ok(new PagedList<TaskDto>(
                    taskDto.ToList(),
                    pagedList.MetaData.TotalCount,
                    pagedList.MetaData.CurrentPage,
                    pagedList.MetaData.PageSize
                )
            );
        }

        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetByAssigneeId([FromQuery] TaskListSearch taskListSearch)
        {
            var userId = User.GetUserId();
            var pagedList = await _taskRepository.GetTaskListByUserId(Guid.Parse(userId), taskListSearch);
            var taskDto = pagedList.Items.Select(x => new TaskDto
            {
                Status = x.Status,
                Name = x.Name,
                AssigneeId = x.AssigneeId,
                CreatedDate = x.CreatedDate,
                Priority = x.Priority,
                Id = x.Id,
                AssigneeName = x.Assignee != null ? x.Assignee.FirstName + ' ' + x.Assignee.LastName : "N/A"
            });

            return Ok(new PagedList<TaskDto>(
                    taskDto.ToList(),
                    pagedList.MetaData.TotalCount,
                    pagedList.MetaData.CurrentPage,
                    pagedList.MetaData.PageSize
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateRequest request)
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
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var taskFromDB = await _taskRepository.GetById(id);
            if (taskFromDB == null)
                return NotFound($"{id} is not found.");

            taskFromDB.Name = request.Name;
            taskFromDB.Priority = request.Priority.HasValue ? request.Priority.Value : Priority.Low;

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

        [HttpPut]
        [Route("{id}/assign")]
        public async Task<IActionResult> AssignTask(Guid id, [FromBody] AssignTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var taskFromDB = await _taskRepository.GetById(id);
            if (taskFromDB == null)
                return NotFound($"{id} is not found.");

            taskFromDB.AssigneeId = request.UserId.Value == Guid.Empty ? null : request.UserId.Value;

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
                AssigneeId = task.AssigneeId,
                CreatedDate = task.CreatedDate
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
                return NotFound($"{id} is not found.");

            await _taskRepository.Delete(task);

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
