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

        public async Task DeleteAsync(Course course)
        {
            _context.Set<Course>().Remove(course);
            await Task.CompletedTask;
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

        public async Task UpdateAsync(Course course)
        {
            _context.Set<Course>().Update(course);
            await Task.CompletedTask;
        }
    }
}
