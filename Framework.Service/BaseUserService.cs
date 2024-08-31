using Framework.Core.Resources;
using Framework.Dto;
using Framework.RepositoryContract;
using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;

namespace Framework.Service
{
    public abstract class BaseUserService<TDto, TDtoType, TRepository>(TRepository repository) : 
        BaseService<TDto, TDtoType, TRepository>(repository),
        IBaseUserService<TDto, TDtoType>
        where TDto : BaseDto<TDtoType>
        where TRepository : IBaseUserRepository<TDto>
    {
        public async Task<GenericResponse<TDto>> UserReadAsync(GenericUserRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = await _repository.UserReadAsync(request.UserId, request.Data);
            if (dto == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
                return response;
            }

            response.Data = dto;

            return response;
        }

        public async Task<GenericResponse<TDto>> UserDeleteAsync(GenericUserRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = await _repository.UserDeleteAsync(request.UserId, request.Data);
            if (dto == null)
            {
                response.AddErrorMessage(GeneralResource.Item_Delete_NotFound);
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage(GeneralResource.Info_Deleted);

            return response;
        }
    }
}
