using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TodoListBlazorWasm.Pages
{
    public partial class Student
    {
        private StudentInfo[] students;
        [Inject] private HttpClient http { set; get; }

        protected override async Task OnInitializedAsync()
        {
            students = await http.GetFromJsonAsync<StudentInfo[]>("sample-data/students.json");
        }

        public class StudentInfo
        {
            public string FirstName { set; get; }
            public string LastName { set; get; }
            public string FullName => $"{FirstName} {LastName}";
            public int Age { set; get; }
            public string Sex { set; get; }
            public DateTime Birthday { set; get; }
        }
    }
}
