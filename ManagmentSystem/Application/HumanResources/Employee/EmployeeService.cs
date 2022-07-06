using AutoMapper;
using Domain.HumanResources;
using Infrastructure.Helper;
using Infrastructure.ORM;
using Infrastructure.ORM.Dapper;
using Infrastructure.Service;

namespace Application.HumanResources;

public class EmployeeService : MasterService, IEmployee, IScopped
{
    public EmployeeService(IMapper mapper, IHelper helper, IDapper dapper, DBContext context) 
        : base(mapper, helper, dapper, context)
    {
    }

    public async Task<EmployeeOut> Get(int id)
    {
        Employee? data = await _context.Employee.FindAsync(id);

        if (data == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }

        return _mapper.Map<EmployeeOut>(data);
    }
}
