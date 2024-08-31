using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;

namespace Framework.ServiceContract
{
    public interface IBaseUserService<TDto, TDtoType> : IBaseService<TDto, TDtoType>
    {
        Task<GenericResponse<TDto>> UserReadAsync(GenericUserRequest<TDtoType> request);

        Task<GenericResponse<TDto>> UserDeleteAsync(GenericUserRequest<TDtoType> request);
    }
}
