using AutoMapper;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Responses;
using SoftServe_TestProject.Application.Requests;

namespace SoftServe_TestProject.API.MappingProfiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<TeacherDTO, TeacherRequest>().ReverseMap();
            CreateMap<TeacherRequest, TeacherResponse>().ReverseMap();
        }
    }
}
