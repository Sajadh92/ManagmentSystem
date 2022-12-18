using AutoMapper;
using Domain.Common.Out;
using Domain.HumanResources;
using Fingers10.ExcelExport.ActionResults;
using Infrastructure.AppException;
using Infrastructure.Extension;
using Infrastructure.Helper;
using Infrastructure.ORM;
using Infrastructure.ORM.Dapper;
using Infrastructure.Service;
using Infrastructure.Static;
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

    private IQueryable<T> GetQuery<T>(EmployeeFilter filter)
    {
        return from emp in _context.Employee.AsNoTracking()
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
               select _mapper.Map<T>(emp);
    }

    public async Task<FilterResultOut<EmployeeDataOut>> GetData(EmployeeFilter filter)
    {
        IQueryable<EmployeeDataOut> query = GetQuery<EmployeeDataOut>(filter);

        return new FilterResultOut<EmployeeDataOut>(filter.PageSize, await query.CountAsync(), 
            await query.GetPage(filter.PageNo, filter.PageSize, filter.OrderBy, filter.IsDesc).ToListAsync());        
    }

    public async Task<ExcelResult<EmployeeExcelOut>> ExportExcel(EmployeeFilter filter)
    {
        int order = 1; string lang = Language.Arabic;

        return new ExcelResult<EmployeeExcelOut>
        (
            data: await GetQuery<EmployeeExcelOut>(filter).ToListAsync(),
            sheetName: nameof(Employee), 
            fileName: $"{nameof(Employee)}_{DateTime.UtcNow.AddHours(3).ToString().Replace(" ", "_")}",

            (nameof(Employee.Name), Translate(nameof(Employee.Name), lang), order++),
            (nameof(Employee.Gender), Translate(nameof(Employee.Gender), lang), order++),
            (nameof(Employee.Birthday), Translate(nameof(Employee.Birthday), lang), order++),
            (nameof(Employee.Phone), Translate(nameof(Employee.Phone), lang), order++),
            (nameof(Employee.JobTitle), Translate(nameof(Employee.JobTitle), lang), order++),
            (nameof(Employee.JobRank), Translate(nameof(Employee.JobRank), lang), order++),
            (nameof(Employee.CreateDate), Translate(nameof(Employee.CreateDate), lang), order++)
        );
    }

    public async Task<List<AutoComplete>> GetAutoComplete(string? term)
    {
        return await _context.Employee.AsNoTracking()
            .Where(x => string.IsNullOrWhiteSpace(term) || x.Name.ToLower().Contains(term.ToLower()))
            .Select(x => new AutoComplete { Key = x.Id, Value = x.Name })
            .ToListAsync();
    }

    public async Task<EmployeeOut> Create(EmployeeModel model)
    {
        if (await _context.Employee.AsNoTracking().AnyAsync(x => x.Name.ToLower().Equals(model.Name.ToLower())))
        {
            throw new DuplicateException(nameof(model.Name));
        }

        Employee employee = _mapper.Map<Employee>(model);

        employee.IsRemoved = false;
        employee.CreateUserId = 0;
        employee.CreateDate = DateTime.UtcNow.AddHours(3);

        await _context.Employee.AddAsync(employee);

        await _context.SaveChangesAsync();

        return _mapper.Map<EmployeeOut>(employee);
    }

    public async Task Update(EmployeeModel model)
    {
        Employee? employee = await _context.Employee.FindAsync(model.Id);

        if (employee == null)
        {
            throw new KeyNotFoundException(nameof(model.Id));
        }

        _mapper.Map(model, employee);

        employee.UpdateUserId = 0;
        employee.UpdateDate = DateTime.UtcNow.AddHours(3);

        _context.Update(employee);

        await _context.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        Employee? employee = await _context.Employee.FindAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }

        if (employee.IsRemoved)
        {
            await PermanentRemove(id);
        }
        else
        {
            employee.IsRemoved = true;
            employee.RemoveUserId = 0;
            employee.RemoveDate = DateTime.UtcNow.AddHours(3);

            _context.Update(employee);

            await _context.SaveChangesAsync();
        }
    }

    public async Task PermanentRemove(int id)
    {
        Employee? employee = await _context.Employee.FindAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }

        _context.Remove(employee);

        await _context.SaveChangesAsync();
    }

    public async Task Restore(int id)
    {
        Employee? employee = await _context.Employee.FindAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }

        employee.IsRemoved = false;
        employee.CreateUserId = 0;
        employee.CreateDate = DateTime.UtcNow.AddHours(3);

        _context.Update(employee);

        await _context.SaveChangesAsync();
    }

    private static string Translate(string key, string lang)
    {
        return TranslateList.First(x => x.lang == lang && x.key == key).value;
    }

    private static readonly List<(string lang, string key, string value)> TranslateList = new()
    {
        (Language.Arabic, nameof(Employee.Name), "اسم الموظف"),
        (Language.Arabic, nameof(Employee.Gender), "الجنس"),
        (Language.Arabic, nameof(Employee.Phone), "رقم الهاتف"),
        (Language.Arabic, nameof(Employee.Birthday), "المواليد"),
        (Language.Arabic, nameof(Employee.JobTitle), "المسمى الوظيفي"),
        (Language.Arabic, nameof(Employee.JobRank), "الدرجة الوظيفية"),
        (Language.Arabic, nameof(Employee.CreateDate), "تأريخ الأنشاء"),

        (Language.English, nameof(Employee.Name), "Full Name"),
        (Language.English, nameof(Employee.Gender), "Gender"),
        (Language.English, nameof(Employee.Phone), "Phone No"),
        (Language.English, nameof(Employee.Birthday), "Birthday"),
        (Language.English, nameof(Employee.JobTitle), "Job Title"),
        (Language.English, nameof(Employee.JobRank), "Job Rank"),
        (Language.English, nameof(Employee.CreateDate), "Create Date"),
    };
}
