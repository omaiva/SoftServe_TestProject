using Microsoft.EntityFrameworkCore;
using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Data.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ProjectContext _context;

        public TeacherRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Teacher teacher)
        {
            await _context.Set<Teacher>().AddAsync(teacher);
        }

        public void Delete(Teacher teacher)
        {
            _context.Set<Teacher>().Remove(teacher);
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context
                .Set<Teacher>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(int id)
        {
            return await _context
                .Set<Teacher>()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public void Update(Teacher teacher)
        {
            _context.Set<Teacher>().Update(teacher);
        }
    }
}
