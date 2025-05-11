using AutoMapper;
using UserManagement.DAL.Models;
using UserManagement.PL.ViewModels;

namespace UserManagement.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() { 
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
