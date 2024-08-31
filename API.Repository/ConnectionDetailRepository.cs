using API.DataAccess.Application;
using API.Dto;
using API.RepositoryContract;
using AutoMapper;
using Framework.Repository;

namespace API.Repository
{
    public class ConnectionDetailRepository(ApplicationDbContext context, IMapper mapper) : BaseUserRepository<ApplicationDbContext, ConnectionDetail, ConnectionDetailDto, ulong>(context, mapper), IConnectionDetailRepository
    {
    }
}
