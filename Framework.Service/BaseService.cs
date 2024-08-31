using Framework.Core.Resources;
using Framework.Dto;
using Framework.RepositoryContract;
using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;

namespace Framework.Service
{
    public abstract class BaseService<TDto, TDtoType, TRepository> : IBaseService<TDto, TDtoType>
        where TDto : BaseDto<TDtoType>
        where TRepository : IBaseRepository<TDto>
    {
        protected readonly TRepository _repository;

        protected BaseService(TRepository repository)
        {
            _repository = repository;
        }

        public virtual async Task<GenericResponse<TDto>> InsertAsync(GenericRequest<TDto> request)
        {
            var response = new GenericResponse<TDto>();

            if (response.IsError()) return response;

            var result = await _repository.InsertAsync(request.Data);

            response.Data = result;
            response.AddInfoMessage(GeneralResource.Info_Saved);

            return response;
        }

        public virtual async Task<GenericResponse<TDto>> ReadAsync(GenericRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = await _repository.ReadAsync(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
                return response;
            }

            response.Data = dto;

            return response;
        }

        public virtual async Task<GenericResponse<TDto>> UpdateAsync(GenericRequest<TDto> request)
        {
            var response = new GenericResponse<TDto>();

            if (response.IsError()) return response;

            var dto = await _repository.UpdateAsync(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage(GeneralResource.Item_Update_NotFound);
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage(GeneralResource.Info_Saved);

            return response;
        }

        public virtual async Task<GenericResponse<TDto>> DeleteAsync(GenericRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = await _repository.DeleteAsync(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage(GeneralResource.Item_Delete_NotFound);
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage(GeneralResource.Info_Deleted);

            return response;
        }

        public async Task<GenericPagedSearchResponse<TDto>> PagedSearchAsync(PagedSearchRequest request)
        {
            return await PagedSearchAsync(_repository, request);
        }

        protected async Task<GenericPagedSearchResponse<TDto>> PagedSearchAsync(IBaseRepository<TDto> repository,
            PagedSearchRequest request)
        {
            return await PagedSearchAsync(repository.PagedSearchAsync, request);
        }

        protected async Task<GenericPagedSearchResponse<TDto>> PagedSearchAsync(
            Func<PagedSearchParameter, Task<PagedSearchResult<TDto>>> pagedSearchFunction,
            PagedSearchRequest request)
        {
            var response = new GenericPagedSearchResponse<TDto>();

            var pagedSearchParameter = GetPagedSearchParameter(request);
            var result = await pagedSearchFunction(pagedSearchParameter);
            response.DtoCollection = result.Result;

            response.TotalCount = result.Count;

            return response;
        }

        protected PagedSearchParameter GetPagedSearchParameter(PagedSearchRequest request)
        {
            return new PagedSearchParameter
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                OrderByFieldName = request.OrderByFieldName,
                SortOrder = request.SortOrder,
                Keyword = request.Keyword,
                Filters = request.Filters,
                FiltersVariable = request.FiltersVariable
            };
        }
    }
}
