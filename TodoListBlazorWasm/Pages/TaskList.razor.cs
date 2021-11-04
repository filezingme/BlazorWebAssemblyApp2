using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.Models.SeedWork;
using TodoListBlazorWasm.Components;
using TodoListBlazorWasm.Pages.Components;
using TodoListBlazorWasm.Services;
using TodoListBlazorWasm.Shared;

namespace TodoListBlazorWasm.Pages
{
    partial class TaskList
    {
        [Inject] private ITaskApiClient TaskApiClient { set; get; }
        [Inject] IToastService ToastService { set; get; }

        protected Confirmation DeleteConfirmation { set; get; }
        protected AssignTask AssignTaskDialog { set; get; }

        private Guid DeleteId { set; get; }
        private List<TaskDto> Tasks;
        public MetaData MetaData { get; set; } = new MetaData();
        private TaskListSearch TaskListSearch = new TaskListSearch();

        [CascadingParameter]
        private Error Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetTasks();
        }

        public async Task SearchTask(TaskListSearch taskListSearch)
        {
            TaskListSearch = taskListSearch;
            await GetTasks();
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
                await GetTasks();
                ToastService.ShowSuccess($"'{DeleteId}' has been deleted successfully.", "Success");
            }
        }

        public void OpenAssignPopup(Guid id)
        {
            AssignTaskDialog.Show(id);
        }

        public async Task AssignTaskSuccess(bool result)
        {
            if (result)
            {
                await GetTasks();
            }
        }

        private async Task GetTasks()
        {
            try
            {
                var pagingResponse = await TaskApiClient.GetTaskList(TaskListSearch);
                Tasks = pagingResponse.Items;
                MetaData = pagingResponse.MetaData;
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }            
        }

        private async Task SelectedPage(int page)
        {
            TaskListSearch.PageNumber = page;
            await GetTasks();
        }
    }
}
