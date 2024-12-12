using AutoMapper;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Responses;
using SoftServe_TestProject.Application.Requests;

namespace SoftServe_TestProject.API.MappingProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseDTO, CourseRequest>().ReverseMap();
            CreateMap<CourseRequest, CourseResponse>().ReverseMap();
        }
    }
}
