using AutoMapper;
using UserManagement.DAL.Models;
using UserManagement.PL.ViewModels;

namespace UserManagement.PL.MappingProfiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
