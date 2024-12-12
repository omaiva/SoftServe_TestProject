namespace SoftServe_TestProject.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<Student> Students { get; set; } = [];
        public List<Course> Courses { get; set; } = [];
    }
}
