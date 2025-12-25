using AutoMapper;
using StudentAPI.API.Models.Auth;
using StudentAPI.Application.Features.Students.Commands;

namespace StudentAPI.API.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<LoginRequest, AuthenticateUserCommand>();
        }
    }
}
