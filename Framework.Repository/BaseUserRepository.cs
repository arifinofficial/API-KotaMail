using AutoMapper;
using Framework.Dto;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Framework.Repository
{
    public abstract class BaseUserRepository<TContext, TEntity, TDto, TDtoType>(TContext context, IMapper mapper)
        : BaseRepository<TContext, TEntity, TDto, TDtoType>(context, mapper), IBaseUserRepository<TDto>
        where TContext : DbContext, new()
        where TEntity : class, new()
        where TDto : BaseDto<TDtoType>, new()
    {
        public virtual async Task<TDto> UserReadAsync(string userId, object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = await dbSet.Where($"UserId = \"{userId}\" AND Id = {primaryKey}").FirstOrDefaultAsync();
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);
            return dto;
        }

        public virtual async Task<TDto> UserDeleteAsync(string userId, object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = await dbSet.Where($"UserId = \"{userId}\" AND Id = {primaryKey}").FirstOrDefaultAsync();
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);

            dbSet.Remove(entity);
            await this.Context.SaveChangesAsync();

            return dto;
        }
    }
}
