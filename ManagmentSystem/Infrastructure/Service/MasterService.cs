using AutoMapper;
using Infrastructure.Helper;
using Infrastructure.ORM;
using Infrastructure.ORM.Dapper;

namespace Infrastructure.Service;

public class MasterService
{
    public readonly IMapper _mapper;

    public readonly IHelper _helper;

    public readonly IDapper _dapper;

    public readonly DBContext _context;

    public MasterService(IMapper mapper, IHelper helper, IDapper dapper, DBContext context)
    {
        _mapper = mapper; _helper = helper; _dapper = dapper; _context = context;
    }
}