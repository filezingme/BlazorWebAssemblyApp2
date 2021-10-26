using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TodoListBlazor.Api.Entities;

namespace TodoListBlazor.Api.Data
{
    public class TodoListDbContext: IdentityDbContext<User, Role, Guid>
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options): base(options)
        {

        }
        public DbSet<Task> Tasks { get; set; }
    }
}
