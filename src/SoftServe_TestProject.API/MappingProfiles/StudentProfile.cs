using AutoMapper;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Responses;
using SoftServe_TestProject.Application.Requests;

namespace SoftServe_TestProject.API.MappingProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentDTO, StudentRequest>().ReverseMap();
            CreateMap<StudentRequest, StudentResponse>().ReverseMap();
        }
    }
}
