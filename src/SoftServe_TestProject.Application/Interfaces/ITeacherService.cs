using SoftServe_TestProject.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftServe_TestProject.Application.Interfaces
{
    public interface ITeacherService
    {
        Task<TeacherRequest?> GetTeacherByIdAsync(int id);
        Task<IEnumerable<TeacherRequest>> GetAllTeachersAsync();
        Task CreateTeacherAsync(TeacherRequest teacherRequest);
        Task UpdateTeacherAsync(TeacherRequest teacherRequest);
        Task DeleteTeacherAsync(int id);
    }
}
