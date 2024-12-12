using Microsoft.EntityFrameworkCore;
using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ProjectContext _context;

        public CourseRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Course course)
        {
            await _context.Set<Course>().AddAsync(course);
        }

        public void Delete(Course course)
        {
            _context.Set<Course>().Remove(course);
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context
                .Set<Course>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context
                .Set<Course>()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Update(Course course)
        {
            _context.Set<Course>().Update(course);
        }
    }
}
