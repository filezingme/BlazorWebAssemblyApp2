using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Models;

namespace TodoListBlazor.Api.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Entities.Task>> GetTaskList(TaskListSearch taskListSearch);
        Task<Entities.Task> Create(Entities.Task task);
        Task<Entities.Task> Update(Entities.Task task);
        Task<Entities.Task> Delete(Entities.Task task);
        Task<Entities.Task> GetById(Guid id);
    }
}
