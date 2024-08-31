using API.DataAccess.Application;
using API.Dto;
using API.RepositoryContract;
using AutoMapper;
using Framework.Repository;

namespace API.Repository
{
    public class ConnectionListRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<ApplicationDbContext, ConnectionList, ConnectionListDto, ulong>(context, mapper), IConnectionListRepository
    {
    }
}
