using API.DataAccess.Application;
using API.Dto;
using API.RepositoryContract;
using AutoMapper;
using Framework.Repository;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace API.Repository
{
    public class ConnectionRepository(ApplicationDbContext context, IMapper mapper)
                : BaseUserRepository<ApplicationDbContext, Connection, ConnectionDto, ulong>(context, mapper), IConnectionRepository
    {
        public override async Task<ConnectionDto> UserReadAsync(string userId, object primaryKey)
        {
            var dbSet = Context.Set<Connection>();
            var entity = await dbSet
                .Include(x => x.ConnectionDetails)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == ulong.Parse(primaryKey.ToString()));

            if (entity == null)
                return null;

            var dto = new ConnectionDto();
            this.EntityToDto(entity, dto);

            return dto;
        }

        public override async Task<PagedSearchResult<ConnectionDto>> PagedSearchAsync(PagedSearchParameter parameter)
        {
            var dbSet = Context.Set<Connection>();
            var queryable = string.IsNullOrEmpty(parameter.Filters)
                ? dbSet
                    .Include(x => x.ConnectionDetails)
                    .Include(x => x.ConnectionFilters)
                .AsQueryable()
                : dbSet
                    .Include(x => x.ConnectionDetails)
                    .Include(x => x.ConnectionFilters)
                .Where(parameter.Filters);

            queryable = string.IsNullOrEmpty(parameter.Keyword)
                ? queryable
                : GetKeywordPagedSearchQueryable(queryable, parameter.Keyword);

            return await GetPagedSearchEnumerableAsync(parameter, queryable);
        }
    }
}
