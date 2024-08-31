using API.Dto;
using API.KotaMail.Models.Connection;
using API.ServiceContract;
using Framework.Application.Controllers;
using Framework.Application.Helpers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.KotaMail.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController(
        IConnectionService connectionService, 
        IConfiguration configuration, 
        IConnectionListService connectionListService) : ApiBaseController
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IConnectionService _connectionService = connectionService;
        private readonly IConnectionListService _connectionListService = connectionListService;

        [HttpGet]
        public async Task<IActionResult> PagedSearch([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " AND ") + $"UserId = \"{userId}\"";

            var response = await _connectionService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = model.OrderByFieldName,
                SortOrder = model.SortOrder,
                Keyword = model.Keyword,
                Filters = filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
                rowJsonData.Add(PopulateResponse(dto));

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorJsonFromModelState();

            var connectionListResponse = await _connectionListService.ReadAsync(new GenericRequest<ulong>
            {
                Data = model.ConnectionListId
            });

            if(connectionListResponse.IsError())
                return GetErrorJson(connectionListResponse);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var base64key = _configuration["Application:Key"];
            var base64Iv = _configuration["Application:IV"];

            var connectionDto = new ConnectionDto
            {
                UserId = userId,
                Name = model.Name,
                IsPublic = model.IsPublic,
                Path = StringHelper.GenerateRandomString(10)
            };

            PopulateAuditFieldsOnCreate(connectionDto);

            var connectionDetailsDto = new List<ConnectionDetailDto>();
            var connectionDetailDto = new ConnectionDetailDto
            {
                UserId = userId,
                ConnectionListId = connectionListResponse.Data.Id,
                Email = model.Email,
                Password = Cryptography.Encrypt(model.Password, base64key, base64Iv),
                ConnectionType = model.ConnectionType,
                Server = connectionListResponse.Data.Server,
                Port = connectionListResponse.Data.Port
            };

            PopulateAuditFieldsOnCreate(connectionDetailDto);
            connectionDetailsDto.Add(connectionDetailDto);

            connectionDto.ConnectionDetails = connectionDetailsDto;

            var response = await _connectionService.InsertAsync(new GenericRequest<ConnectionDto>
            {
                Data = connectionDto
            });

            if (response.IsError())
                return GetErrorJson(response);

            return GetSuccessJson(response, PopulateResponse(response.Data));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ulong id, UpdateModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorJsonFromModelState();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var readConnection = await _connectionService.UserReadAsync(new GenericUserRequest<ulong>
            {
                UserId = userId,
                Data = id
            });

            if (readConnection.IsError())
                return GetErrorJson(readConnection);

            var dto = readConnection.Data;
            dto.Name = model.Name;
            dto.IsPublic = model.IsPublic;
            PopulateAuditFieldsOnUpdate(dto);

            if (dto.ConnectionDetails.Count < 1)
                return GetErrorJson("Connection detail data not found.");

            var connectionDetailDto = dto.ConnectionDetails.First();
            connectionDetailDto.Email = model.Email;
            connectionDetailDto.Password = model.Password;
            PopulateAuditFieldsOnUpdate(connectionDetailDto);

            dto.ConnectionDetails = new List<ConnectionDetailDto>
            {
                connectionDetailDto
            };

            var response = await _connectionService.UpdateAsync(new GenericRequest<ConnectionDto> { Data = dto });

            if(response.IsError())
                return GetErrorJson(response);

            return GetSuccessJson(response, PopulateResponse(response.Data));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(ulong id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _connectionService.UserDeleteAsync(new GenericUserRequest<ulong> { UserId = userId, Data = id });

            if(response.IsError())
                return GetErrorJson(response);

            return GetSuccessJson(response, response.Data);
        }

        private static object PopulateResponse(ConnectionDto dto)
        {
            return new
            {
                dto.Id,
                dto.Name,
                dto.IsPublic,
                dto.Path,
                ConnectionDetail = (dto.ConnectionDetails.Count > 0)
                    ? dto.ConnectionDetails.Select(x => new
                    {
                        x.Id,
                        x.Email,
                        x.Port,
                        x.Server
                    })
                    : null
            };
        }
    }
}
