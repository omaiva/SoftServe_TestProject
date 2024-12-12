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

        public void Delete(Student student)
        {
            _context.Set<Student>().Remove(student);
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
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public void Update(Student student)
        {
            _context.Set<Student>().Update(student);
        }
    }
}
