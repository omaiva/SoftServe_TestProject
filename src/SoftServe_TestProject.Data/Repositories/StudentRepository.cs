using Microsoft.EntityFrameworkCore;
using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Repositories;

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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Student student)
        {
            _context.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context
                .Set<Student>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context
                .Set<Student>()
                .FindAsync(id);
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Set<Student>().Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
