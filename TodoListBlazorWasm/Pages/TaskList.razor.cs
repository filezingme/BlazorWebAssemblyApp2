using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Models;
using TodoListBlazorWasm.Components;
using TodoListBlazorWasm.Services;

namespace TodoListBlazorWasm.Pages
{
    partial class TaskList
    {
        [Inject] private ITaskApiClient TaskApiClient { set; get; }

        protected Confirmation DeleteConfirmation { set; get; }

        private Guid DeleteId { set; get; }
        private List<TaskDto> Tasks;
        private TaskListSearch TaskListSearch = new TaskListSearch();

        protected override async Task OnInitializedAsync()
        {
            Tasks = await TaskApiClient.GetTaskList(TaskListSearch);
        }

        public async Task SearchTask(TaskListSearch taskListSearch)
        {
            TaskListSearch = taskListSearch;
            Tasks = await TaskApiClient.GetTaskList(TaskListSearch);
        }

        public void OnDeleteTask(Guid deleteId)
        {
            DeleteId = deleteId;
            DeleteConfirmation.Show();
        }

        public async Task OnConfirmDeleteTask(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                await TaskApiClient.DeleteTask(DeleteId);
                Tasks = await TaskApiClient.GetTaskList(TaskListSearch);
            }
        }
    }
}
