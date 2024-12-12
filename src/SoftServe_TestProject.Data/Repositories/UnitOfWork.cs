using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectContext _context;
        public IStudentRepository Students { get; private set; }

        public ITeacherRepository Teachers { get; private set; }

        public ICourseRepository Courses {  get; private set; }


        public UnitOfWork(ProjectContext context,
                          IStudentRepository studentRepository,
                          ITeacherRepository teacherRepository,
                          ICourseRepository courseRepository)
        {
            _context = context;
            Students = studentRepository;
            Teachers = teacherRepository;
            Courses = courseRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
