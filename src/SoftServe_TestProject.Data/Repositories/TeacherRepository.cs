using Microsoft.EntityFrameworkCore;
using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Repositories;

namespace SoftServe_TestProject.Data.Repositories
{
    internal class TeacherRepository : ITeacherRepository
    {
        private readonly ProjectContext _context;

        public TeacherRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Teacher teacher)
        {
            await _context.Set<Teacher>().AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Teacher teacher)
        {
            _context.Remove(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context
                .Set<Teacher>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            return await _context
                .Set<Teacher>()
                .FindAsync(id);
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            _context.Set<Teacher>().Update(teacher);
            await _context.SaveChangesAsync();
        }
    }
}
