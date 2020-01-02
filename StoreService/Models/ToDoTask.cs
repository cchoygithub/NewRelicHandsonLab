using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreService.Models
{
    public class ToDoTask
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool Completed { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
    }
}
