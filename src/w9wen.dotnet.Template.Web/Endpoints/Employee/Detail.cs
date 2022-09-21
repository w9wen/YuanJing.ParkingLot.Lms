using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using w9wen.dotnet.Template.Infrastructure.Data;

namespace src.w9wen.dotnet.Template.Web.Endpoints.Employee
{
  public class Detail : EndpointBaseAsync
      .WithRequest<GetDetailByUserNameRequest>
      .WithActionResult<EmployeeDto>
  {
    #region Fields

    private readonly AppUserManager _appUserManager;
    private readonly IMapper _mapper;

    #endregion Fields

    #region Constructor

    public Detail(AppUserManager appUserManager, IMapper mapper)
    {
      _appUserManager = appUserManager;
      _mapper = mapper;
    }

    #endregion Constructor

    [HttpGet(GetDetailByUserNameRequest.Route)]
    [SwaggerOperation(
        Summary = "Get employee",
        Description = "Get employee",
        OperationId = "Employee.Detail",
        Tags = new[] { "EmployeeEndpoints" })
    ]
    public override async Task<ActionResult<EmployeeDto>> HandleAsync([FromRoute] GetDetailByUserNameRequest request,
                                                                                CancellationToken cancellationToken = default)
    {
      string? userName = null;

      if (request.UserName is null)
      {
        return BadRequest();
      }

      userName = request.UserName;

      var appUserItem = await this._appUserManager.FindByNameAsync(userName);

      if (appUserItem != null)
      {
        var employeeDto = this._mapper.Map<EmployeeDto>(appUserItem);

        //// 取得Employee Roles
        var appUserRoles = await this._appUserManager.GetRolesAsync(appUserItem);
        if (appUserRoles != null && appUserRoles.Count() > 0)
        {
          employeeDto.Roles = appUserRoles.ToList();
        }
        return employeeDto;
      }

      return BadRequest();
    }
  }
}