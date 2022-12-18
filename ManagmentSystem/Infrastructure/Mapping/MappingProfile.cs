using AutoMapper;
using Domain.HumanResources;

namespace Infrastructure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EmployeeModel, Employee>();
        CreateMap<Employee, EmployeeOut>();
        CreateMap<Employee, EmployeeDataOut>();
    }
}
