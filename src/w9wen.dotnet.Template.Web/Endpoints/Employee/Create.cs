using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.w9wen.dotnet.Template.Web.Endpoints.Employee;
using Swashbuckle.AspNetCore.Annotations;
using w9wen.dotnet.Template.Core.Constants;
using w9wen.dotnet.Template.Core.Entities;
using w9wen.dotnet.Template.Infrastructure.Data;

namespace w9wen.dotnet.Template.Web.Endpoints.Employee
{
  public class Create : EndpointBaseAsync
    .WithRequest<CreateEmployeeRequest>
    .WithActionResult<EmployeeDto>
  {
    private readonly AppUserManager _appUserManager;
    private readonly IMapper _mapper;

    public Create(AppUserManager appUserManager,
                  IMapper mapper)
    {
      _appUserManager = appUserManager;
      _mapper = mapper;
    }

    [Authorize(Policy = "Modify")]
    [HttpPost(EmployeeListRequest.Route)]
    [SwaggerOperation(
            Summary = "Create a new employee",
            Description = "Create a new employee",
            OperationId = "Employee.Create",
            Tags = new[] { "EmployeeEndpoints" })
        ]
    public override async Task<ActionResult<EmployeeDto>> HandleAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
      var userName = request.UserName;
      if (string.IsNullOrEmpty(userName))
      {
        return BadRequest("User name could not be null or empty");
      }

      if (await _appUserManager.Users.AnyAsync(x => x.UserName.ToLower().Equals(userName.ToLower())))
      {
        return BadRequest("User name already taken");
      }

      var requestRoles = request.Roles;

      if (requestRoles == null || requestRoles.Count < 1)
      {
        return BadRequest("You should choose a role at least!");
      }
      else if (requestRoles.Except(Enum.GetNames(typeof(AppRoleTypeEnum)).ToList()).Count() > 0)
      {
        return BadRequest("Invalid role");
      }

      var appUserItem = this._mapper.Map<AppUserEntity>(request);

      var createResult = await _appUserManager.CreateAsync(appUserItem, EmployeeConstants.DEFAULT_PASSWORD);

      if (!createResult.Succeeded) return BadRequest(createResult.Errors);

      var addToRoleResult = await _appUserManager.AddToRolesAsync(appUserItem, requestRoles);

      if (!addToRoleResult.Succeeded) return BadRequest(addToRoleResult.Errors);

      var employeeDto = _mapper.Map<EmployeeDto>(appUserItem);

      employeeDto.Roles = requestRoles;

      return employeeDto;

    }
  }
}