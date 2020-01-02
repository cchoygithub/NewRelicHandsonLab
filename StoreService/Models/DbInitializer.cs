using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreService.Models
{
    public static class DbInitializer
    {
        public static void Initialize(ToDoContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Tasks.Any())
            {
                return;   // DB has been seeded
            }

            var r = new Random();
            for (int i = 1; i <= 500; i++)
            {                
                context.Tasks.Add(new ToDoTask { Title = $"task{1}", Description = $"desc {i}", Deadline = new DateTime(2020, 1, 1).AddDays(r.Next(366)) });
            }
            context.SaveChanges();


            for (int i = 1; i <= 500; i++)
            {
                context.Users.Add(new User { Name=$"user{i}"});
            }
            context.SaveChanges();

            for (int i = 1; i <= 500; i++)
            {
                var list = new HashSet<int>();
                for(int j=1; j < r.Next(10)+1; j++)
                {
                    var auid = r.Next(500) + 1;
                    while (list.Contains(auid))
                    {
                        auid = r.Next(500) + 1;
                    }
                    list.Add(auid);
                    context.Assignments.Add(new Assignment { ToDoTaskID=i,UserID= auid });
                }
                
            }
            context.SaveChanges();
        }
    }
}
