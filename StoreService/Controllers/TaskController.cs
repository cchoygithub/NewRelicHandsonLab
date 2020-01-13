using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreService.Models;

namespace StoreService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ToDoContext context;

        public TaskController(ToDoContext context) => this.context = context;

        [HttpGet]
        public IEnumerable<ToDoTaskApiModel> List()
        {

            return context.Tasks.ToArray()
                .Select(t => new ToDoTaskApiModel
                {
                    ID = t.ID,
                    Title = t.Title,
                    Description = t.Description,
                    Deadline = t.Deadline,
                    Completed = t.Completed,
                    AssignedUsers = context.Assignments.Where(a => a.ToDoTaskID == t.ID).Select(a => a.User.Name).ToArray()
                })
                .ToArray();
        }
    }

    public class ToDoTaskApiModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool Completed { get; set; }

        public string[] AssignedUsers { get; set; }
    }
}