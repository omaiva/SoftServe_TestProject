namespace SoftServe_TestProject.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public List<Student> Students { get; set; } = [];
        public Teacher? Teacher { get; set; }
    }
}
