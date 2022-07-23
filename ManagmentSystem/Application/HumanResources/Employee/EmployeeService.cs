using AutoMapper;
using Domain.Common.Out;
using Domain.HumanResources;
using Infrastructure.Extension;
using Infrastructure.Helper;
using Infrastructure.ORM;
using Infrastructure.ORM.Dapper;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;

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

    public async Task<FilterResultOut<EmployeeDataOut>> GetData(EmployeeFilter filter)
    {
        IQueryable<EmployeeDataOut> query = 
            from emp in _context.Employee.AsNoTracking()
            where (string.IsNullOrWhiteSpace(filter.Name) || emp.Name.ToLower().Contains(filter.Name.ToLower()))
               && (string.IsNullOrWhiteSpace(filter.Gender) || emp.Gender.ToLower().Equals(filter.Gender.ToLower()))
               && (string.IsNullOrWhiteSpace(filter.Phone) || emp.Phone.ToLower().Contains(filter.Phone.ToLower()))
               && (filter.FromBirthday == null || emp.Birthday == null || emp.Birthday >= filter.FromBirthday)
               && (filter.ToBirthday == null || emp.Birthday == null || emp.Birthday <= filter.ToBirthday)
               && (string.IsNullOrWhiteSpace(filter.JobTitle) || emp.JobTitle.ToLower().Contains(filter.JobTitle.ToLower()))
               && (filter.FromJobRank == null || emp.JobRank >= filter.FromJobRank)
               && (filter.ToJobRank == null || emp.JobRank <= filter.ToJobRank)
               && (filter.FromCreateDate == null || emp.CreateDate >= filter.FromCreateDate)
               && (filter.ToCreateDate == null || emp.CreateDate <= filter.ToCreateDate)
               && (filter.FromUpdateDate == null || emp.UpdateDate == null || emp.UpdateDate >= filter.FromUpdateDate)
               && (filter.ToUpdateDate == null || emp.UpdateDate == null || emp.UpdateDate <= filter.ToUpdateDate)
               && (filter.FromRemoveDate == null || emp.RemoveDate == null || emp.RemoveDate >= filter.FromRemoveDate)
               && (filter.ToRemoveDate == null || emp.RemoveDate == null || emp.RemoveDate <= filter.ToRemoveDate)
               && (filter.IsRemoved == emp.IsRemoved)
            select _mapper.Map<EmployeeDataOut>(emp);

        return new FilterResultOut<EmployeeDataOut>(filter.PageSize, await query.CountAsync(), 
            await query.GetPage(filter.PageNo, filter.PageSize, filter.OrderBy, filter.IsDesc).ToListAsync());        
    }
}
