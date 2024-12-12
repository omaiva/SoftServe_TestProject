﻿using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Domain.Interfaces
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher?> GetByIdAsync(int id);
        Task AddAsync(Teacher student);
        void Update(Teacher student);
        void Delete(Teacher student);
    }
}