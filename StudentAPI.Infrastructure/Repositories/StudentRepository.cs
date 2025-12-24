using Microsoft.EntityFrameworkCore;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Interfaces;
using StudentAPI.Infrastructure.Persistence;

namespace StudentAPI.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentAPIDbContext _context;
        public StudentRepository(StudentAPIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetPassedStudentsAsync()
        {
            return await _context.Students
                .Where(s => s.Grade >= 50)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<double?> GetAverageGrade()
        {
            bool anyStudents = await _context.Students.AnyAsync();

            if (!anyStudents)
            {
                return null;
            }

            return await _context.Students
                .AverageAsync(s => s.Grade);
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Student student)
        {
            _context.Add(student);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Update(student);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Student student)
        {
            _context.Remove(student);

            await _context.SaveChangesAsync();
        }
    }
}
