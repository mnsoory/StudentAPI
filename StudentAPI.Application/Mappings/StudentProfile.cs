

using AutoMapper;
using StudentAPI.Application.DTOs;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Domain.Entities;

namespace StudentAPI.Application.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentDto>();
            CreateMap<CreateStudentCommand, Student>();
            CreateMap<UpdateStudentCommand, Student>();
        }
    }
}
