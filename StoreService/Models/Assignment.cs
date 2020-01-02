namespace StoreService.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int ToDoTaskID { get; set; }
        public int UserID { get; set; }

        public ToDoTask Task { get; set; }
        public User User { get; set; }
    }
}