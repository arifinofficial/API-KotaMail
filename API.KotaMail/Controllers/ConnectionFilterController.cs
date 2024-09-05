using API.Dto;
using API.KotaMail.Models.ConnectionFilter;
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
    public class ConnectionFilterController(IConnectionService connectionService, IConnectionFilterService connectionFilterService) : ApiBaseController
    {
        private readonly IConnectionService _connectionService = connectionService;
        private readonly IConnectionFilterService _connectionFilterService = connectionFilterService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorJsonFromModelState();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var connectionResponse = await _connectionService.UserReadAsync(new GenericUserRequest<ulong> { 
                UserId = userId,
                Data = model.ConnectionId
            });

            if (connectionResponse.IsError())
                return GetErrorJson(connectionResponse);

            var connectionFilterDto = new ConnectionFilterDto
            {
                UserId = userId,
                ConnectionId = connectionResponse.Data.Id,
                Key = model.Key,
                Value = model.Value
            };
            PopulateAuditFieldsOnCreate(connectionFilterDto);

            var response = await _connectionFilterService.InsertAsync(new GenericRequest<ConnectionFilterDto>
            {
                Data = connectionFilterDto
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

            var readConnectionFilter = await _connectionFilterService.UserReadAsync(new GenericUserRequest<ulong>
            {
                UserId = userId,
                Data = id
            });

            var connectionFilterDto = readConnectionFilter.Data;
            connectionFilterDto.Key = model.Key;
            connectionFilterDto.Value = model.Value;
            PopulateAuditFieldsOnUpdate(connectionFilterDto);

            var response = await _connectionFilterService.UpdateAsync(new GenericRequest<ConnectionFilterDto> 
            { 
                Data = connectionFilterDto 
            });

            if(response.IsError()) 
                return GetErrorJson(response);

            return GetSuccessJson(response, response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(ulong id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _connectionFilterService.UserDeleteAsync(new GenericUserRequest<ulong> { UserId = userId, Data = id });

            if(response.IsError())
                return GetErrorJson(response);

            return GetSuccessJson(response, response.Data);
        }
    }
}
