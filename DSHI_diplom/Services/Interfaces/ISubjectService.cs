﻿using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<List<Subject>> GetAllAsync();
        Task<Subject?> GetByIdAsync(int id);
        Task AddAsync(Subject subject);
        Task UpdateAsync(Subject subject);
        Task DeleteAsync(int id);

    }
}
