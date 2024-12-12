namespace SoftServe_TestProject.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }
        ICourseRepository Courses { get; }
        Task<int> SaveChangesAsync();
    }
}
