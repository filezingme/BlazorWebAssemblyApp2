using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Models;
using TodoListBlazor.Api.Entities;

namespace TodoListBlazor.Api.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUserList();
    }
}
