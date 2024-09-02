using API.Dto;
using API.KotaMail.Models.ConnectionDetailFilter;
using API.ServiceContract;
using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.KotaMail.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConnectionDetailFilterController(IConnectionDetailService connectionDetailService, IConnectionDetailFilterService connectionDetailFilterService) : ApiBaseController
    {
        private readonly IConnectionDetailService _connectionDetailService = connectionDetailService;
        private readonly IConnectionDetailFilterService _connectionDetailFilterService = connectionDetailFilterService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorJsonFromModelState();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var connectionDetailResponse = await _connectionDetailService.UserReadAsync(new GenericUserRequest<ulong> { 
                UserId = userId,
                Data = model.ConnectionDetailId
            });

            if (connectionDetailResponse.IsError())
                return GetErrorJson(connectionDetailResponse);

            var connectionDetailFilterDto = new ConnectionDetailFilterDto
            {
                UserId = userId,
                ConnectionDetailId = connectionDetailResponse.Data.Id,
                Key = model.Key,
                Value = model.Value
            };
            PopulateAuditFieldsOnCreate(connectionDetailFilterDto);

            var response = await _connectionDetailFilterService.InsertAsync(new GenericRequest<ConnectionDetailFilterDto>
            {
                Data = connectionDetailFilterDto
            });

            if(response.IsError())
                return GetErrorJson(response);

            return GetSuccessJson(response, response.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ulong id, EditModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return GetErrorJsonFromModelState();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var readConnectionDetailFilter = await _connectionDetailFilterService.UserReadAsync(new GenericUserRequest<ulong>
            {
                UserId = userId,
                Data = id
            });

            var connectionDetailFilterDto = readConnectionDetailFilter.Data;
            connectionDetailFilterDto.Key = model.Key;
            connectionDetailFilterDto.Value = model.Value;
            PopulateAuditFieldsOnUpdate(connectionDetailFilterDto);

            var response = await _connectionDetailFilterService.UpdateAsync(new GenericRequest<ConnectionDetailFilterDto> 
            { 
                Data = connectionDetailFilterDto 
            });

            if(response.IsError()) 
                return GetErrorJson(response);

            return GetSuccessJson(response, response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(ulong id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _connectionDetailFilterService.UserDeleteAsync(new GenericUserRequest<ulong> { UserId = userId, Data = id });

            if(response.IsError())
                return GetErrorJson(response);

            return GetSuccessJson(response, response.Data);
        }
    }
}
