using Framework.Dto;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Application.Controllers
{
    public abstract class ApiBaseController : ControllerBase
    {
        private IEnumerable<string> GetModelStateError()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
        }

        protected JsonResult GetBasicSuccessJson()
        {
            return new JsonResult(new { IsSuccess = true });
        }

        protected JsonResult GetSuccessJson(BasicResponse response, object value)
        {
            return new JsonResult(new
            {
                IsSuccess = true,
                MessageInfoTextArray = response.GetMessageInfoTextArray(),
                Value = value
            });
        }

        protected JsonResult GetErrorJson(string[] messages)
        {
            return new JsonResult(new
            {
                IsSuccess = false,
                MessageErrorTextArray = messages
            });
        }

        protected JsonResult GetErrorJson(string message)
        {
            var messageArray = new[] { message };
            return new JsonResult(new
            {
                IsSuccess = false,
                MessageErrorTextArray = messageArray
            });
        }

        protected JsonResult GetErrorJson(BasicResponse response)
        {
            return new JsonResult(new
            {
                IsSuccess = false,
                MessageErrorTextArray = response.GetMessageErrorTextArray()
            });
        }

        protected JsonResult GetErrorJsonFromModelState()
        {
            return GetErrorJson(GetModelStateError().ToArray());
        }

        protected ActionResult GetPagedSearchGridJson<TDto>(int pageIndex,
            int pageSize,
            List<object> rowJsonData,
            GenericPagedSearchResponse<TDto> response)
        {
            var jsonData = new
            {
                current = pageIndex,
                rowCount = pageSize,
                rows = rowJsonData,
                total = response.TotalCount
            };

            return new JsonResult(jsonData);
        }

        protected void PopulateAuditFieldsOnCreate<T>(BaseDto<T> dto)
        {
            var currentUtcTime = DateTime.UtcNow;

            dto.CreatedBy = User.Identity.Name;
            dto.CreatedDateTime = currentUtcTime;
            dto.LastModifiedBy = User.Identity.Name;
            dto.LastModifiedDateTime = currentUtcTime;
        }

        protected void PopulateAuditFieldsOnUpdate<T>(BaseDto<T> dto)
        {
            var currentUtcTime = DateTime.UtcNow;

            dto.LastModifiedBy = User.Identity.Name;
            dto.LastModifiedDateTime = currentUtcTime;
        }
    }
}
