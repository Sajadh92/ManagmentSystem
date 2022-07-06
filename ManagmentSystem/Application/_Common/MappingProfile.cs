using Application.HumanResources;
using AutoMapper;
using Domain.HumanResources;

namespace Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeOut>();
    }
}
