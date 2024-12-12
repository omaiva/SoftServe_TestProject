using Microsoft.EntityFrameworkCore;
using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ProjectContext _context;

        public StudentRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student student)
        {
            await _context.Set<Student>().AddAsync(student);
        }

        public async Task DeleteAsync(Student student)
        {
            _context.Set<Student>().Remove(student);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context
                .Set<Student>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context
                .Set<Student>()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Set<Student>().Update(student);
            await Task.CompletedTask;
        }
    }
}
