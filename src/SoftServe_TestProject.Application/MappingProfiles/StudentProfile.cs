using AutoMapper;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Application.MappingProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentRequest>().ReverseMap();
        }
    }
}
