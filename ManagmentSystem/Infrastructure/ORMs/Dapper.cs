using Dapper;
using Infrastructure.Service;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Infrastructure.ORM.Dapper;

public interface IDapper
{
    void RunScript(string Query);
    Task RunScriptAsync(string Query);
    Task RunSpAsync(string spName, object? pars = null);
    Task<T> GetAsync<T>(string spName, object? pars = null);
    List<T> GetList<T>(string spName, object? pars = null);
    Task<List<T>> GetListAsync<T>(string spName, object? pars = null);
    Task<int> GetSequenceAsync(string sequence);
}

public class Dapper : IDapper, ISingleton
{
    private static readonly DbConnection Connection = new SqlConnection(DBConn.ConnectionString);

    public Dapper()
    {
        try
        {
            Connection.Open();
        }
        catch (Exception exception)
        {
            string logFormat = "^[{0}]\t[{1}]\t[{2}]\t[{3}]$\n";

            string logRecord = string.Format(logFormat, DateTime.UtcNow.AddHours(3), "Dapper.Connection",
                 exception?.Message, exception?.InnerException?.Message);

            File.AppendAllText(@"Files\Logger.txt", logRecord);

            throw;
        }
    }

    public void RunScript(string Query)
    {
        try
        {
            Connection.Query(Query, commandTimeout: 15, commandType: CommandType.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task RunScriptAsync(string Query)
    {
        try
        {
            await Connection.QueryAsync(Query, commandTimeout: 15, commandType: CommandType.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task RunSpAsync(string spName, object? pars = null)
    {
        try
        {
            await Connection.QueryAsync(spName, pars, commandTimeout: 15, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<T> GetAsync<T>(string spName, object? pars = null)
    {
        try
        {
            return await Connection.QueryFirstOrDefaultAsync<T>(spName, pars, commandTimeout: 15, 
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<T> GetList<T>(string spName, object? pars = null)
    {
        try
        {
            IEnumerable<T> data = Connection.Query<T>(spName, pars,
                commandTimeout: 15, commandType: CommandType.StoredProcedure);

            return data is null ? new List<T>() : data.ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<T>> GetListAsync<T>(string spName, object? pars = null)
    {
        try
        {
            IEnumerable<T> data = await Connection.QueryAsync<T>(spName, pars,
                commandTimeout: 15, commandType: CommandType.StoredProcedure);

            return data is null ? new List<T>() : data.ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }

    // Special Functions

    public async Task<int> GetSequenceAsync(string sequence)
    {
        try
        {
            return await Connection.QueryFirstAsync<int>($"SELECT NEXT VALUE FOR {sequence}",
                commandTimeout: 15, commandType: CommandType.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
