namespace Framework.RepositoryContract
{
    public interface IBaseUserRepository<TDto> : IBaseRepository<TDto>
    {
        Task<TDto> UserReadAsync(string userId, object primaryKey);

        Task<TDto> UserDeleteAsync(string userId, object primaryKey);
    }
}
