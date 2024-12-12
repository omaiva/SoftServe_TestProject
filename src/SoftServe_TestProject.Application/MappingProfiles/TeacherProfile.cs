using AutoMapper;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Application.MappingProfiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherRequest>().ReverseMap();
        }
    }
}
