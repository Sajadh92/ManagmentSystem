using Domain.Common.Out;
using Domain.HumanResources;
using Fingers10.ExcelExport.ActionResults;

namespace Application.HumanResources;

public interface IEmployee
{
    Task<EmployeeOut> Get(int id);
    Task<FilterResultOut<EmployeeDataOut>> GetData(EmployeeFilter filter);
    Task<ExcelResult<EmployeeExcelOut>> ExportExcel(EmployeeFilter filter);
    Task<List<AutoComplete>> GetAutoComplete(string? term);
    Task<EmployeeOut> Create(EmployeeModel model);
    Task Update(EmployeeModel model);
    Task Remove(int id);
    Task PermanentRemove(int id);
    Task Restore(int id);
}
