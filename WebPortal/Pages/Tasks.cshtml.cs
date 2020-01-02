using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebPortal.Pages
{
    public class TasksModel : PageModel
    {
        private readonly HttpClient client = new HttpClient();
        public ToDoTaskModel[] Tasks { get; set; }

        public async Task OnGet()
        {
            var url = Environment.GetEnvironmentVariable("STORE_SVC_URL") + "/task";
            var res = await client.GetAsync(url);
            Tasks = await JsonSerializer.DeserializeAsync<ToDoTaskModel[]>(await res.Content.ReadAsStreamAsync());
        }
    }

    public class ToDoTaskModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool Completed { get; set; }

        public string[] AssignedUsers { get; set; }
    }
}
