using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoListBlazor.Api.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Entities.Task>> GetTaskList();
        Task<Entities.Task> Create(Entities.Task task);
        Task<Entities.Task> Update(Entities.Task task);
        Task<Entities.Task> Delete(Entities.Task task);
        Task<Entities.Task> GetById(Guid id);
    }
}
