﻿using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class TestService : ITestService
    {
        private readonly DiplomContext _context;
        public TestService(DiplomContext context)
        {
            _context = context;
        }
        public async Task<List<Test>> GetFilteredTestBySearchAsync(string searchText)
        {
            var test = await _context.Tests
                .Where(n => n.Name.Contains(searchText) || n.Subject.Name.Contains(searchText))
                .ToListAsync();
            return test;
        }
        public async Task<List<Test>> GetFilteredTestAsync(string subject, string class_)
        {
            var query = _context.Tests
                .Include(n => n.Subject)
                .Include(n => n.Class)
                .AsQueryable();

            if (!string.IsNullOrEmpty(subject))
                query = query.Where(n => n.Subject.Name == subject);

            if (!string.IsNullOrEmpty(class_))
                query = query.Where(n => n.Class.Name == class_);

            return await query.ToListAsync();
        }
        public async Task<List<Test>> GetAllAsync()
        {
            return await _context.Tests.ToListAsync();
        }

        public async Task<Test?> GetByIdAsync(int id)
        {
            return await _context.Tests.FindAsync(id);
        }

        public async Task AddAsync(Test test)
        {
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Test test)
        {
            _context.Tests.Update(test);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test != null)
            {
                _context.Tests.Remove(test);
                await _context.SaveChangesAsync();
            }
        }
    }
}
