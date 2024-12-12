using AutoMapper;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Application.MappingProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseRequest>().ReverseMap();
        }
    }
}
