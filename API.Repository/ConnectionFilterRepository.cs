using API.DataAccess.Application;
using API.Dto;
using API.RepositoryContract;
using AutoMapper;
using Framework.Repository;

namespace API.Repository
{
    public class ConnectionFilterRepository(ApplicationDbContext context, IMapper mapper) : BaseUserRepository<ApplicationDbContext, ConnectionFilter, ConnectionFilterDto, ulong>(context, mapper), IConnectionFilterRepository
    {
    }
}
