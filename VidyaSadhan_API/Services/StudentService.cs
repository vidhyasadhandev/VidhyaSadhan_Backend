using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidyaSadhan_API.Entities;
using VidyaSadhan_API.Extensions;
using VidyaSadhan_API.Models;

namespace VidyaSadhan_API.Services
{
    public class StudentService
    {
        private VSDbContext _dbContext;
        IMapper _map;

        public StudentService(VSDbContext dbContext, IMapper map)
        {
            _dbContext = dbContext;
            _map = map;
        }

        public async Task<IEnumerable<StudentViewModel>> GetAllStudents()
        {
            var students = await _dbContext.Students.Include(x => x.Account).ToListAsync();
            return _map.Map<IEnumerable<StudentViewModel>>(students);
        }

        public async Task<StudentViewModel> GetStudentById(string UserId)
        {
            var instructor = await _dbContext.Students.FirstOrDefaultAsync(i => i.UserId == UserId);
            _dbContext.Entry(instructor).Reference(y => y.Account).Load();
            return _map.Map<StudentViewModel>(instructor);
        }


        public async Task<int> SaveStudent(StudentViewModel instructor)
        {
            _dbContext.Students.Add(_map.Map<Student>(instructor));
            return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateStudent(StudentViewModel instructor)
        {
            _dbContext.Students.Update(_map.Map<Student>(instructor));
            return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
