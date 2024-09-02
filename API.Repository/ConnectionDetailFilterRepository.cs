using API.DataAccess.Application;
using API.Dto;
using API.RepositoryContract;
using AutoMapper;
using Framework.Repository;

namespace API.Repository
{
    public class ConnectionDetailFilterRepository(ApplicationDbContext context, IMapper mapper) : BaseUserRepository<ApplicationDbContext, ConnectionDetailFilter, ConnectionDetailFilterDto, ulong>(context, mapper), IConnectionDetailFilterRepository
    {
    }
}
