namespace SoftServe_TestProject.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<Course> Courses { get; set; } = [];
        public List<Teacher> Teachers { get; set; } = [];
    }
}
