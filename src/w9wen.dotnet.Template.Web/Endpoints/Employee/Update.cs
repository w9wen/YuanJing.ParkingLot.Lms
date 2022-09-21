using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.w9wen.dotnet.Template.Web.Endpoints.Employee;
using Swashbuckle.AspNetCore.Annotations;
using w9wen.dotnet.Template.Core.Entities;
using w9wen.dotnet.Template.Infrastructure.Data;

namespace w9wen.dotnet.Template.Web.Endpoints.Employee
{
  public class Update : EndpointBaseAsync
    .WithRequest<EmployeeDto>
    .WithoutResult
  {
    #region Fields

    private readonly AppUserManager _appUserManager;
    private readonly IMapper _mapper;

    #endregion Fields

    #region Constructor

    public Update(AppUserManager appUserManager, IMapper mapper)
    {
      _appUserManager = appUserManager;
      _mapper = mapper
      ;
    }

    #endregion Constructor

    [Authorize(Policy = "Modify")]
    [HttpPut("[namespace]")]
    [SwaggerOperation(
            Summary = "Update employee.",
            Description = "Update employee.",
            OperationId = "Employee.Update",
            Tags = new[] { "EmployeeEndpoints" })
        ]
    public override async Task<ActionResult> HandleAsync(EmployeeDto request, CancellationToken cancellationToken = default)
    {
      var item = await this._appUserManager.FindByIdAsync(request.Id.ToString());

      if (item != null)
      {
        var appUserEntity = _mapper.Map(request, item);

        var updateResult = await this._appUserManager.UpdateAsync(appUserEntity);
        if (updateResult.Succeeded)
        {
          var requestRoles = request.Roles;
          if (requestRoles != null && requestRoles.Count > 0)
          {
            /// 角色應在Enum中
            if (requestRoles.Except(Enum.GetNames(typeof(AppRoleTypeEnum)).ToList()).Count() < 1)
            {
              var appRoles = await this._appUserManager.GetRolesAsync(appUserEntity);
              if (appRoles != null && appRoles.Count > 0)
              {
                var addRoles = requestRoles.Except(appRoles);
                var removeRoles = appRoles.Except(requestRoles);

                if (addRoles != null && addRoles.Count() > 0)
                {
                  await this._appUserManager.AddToRolesAsync(appUserEntity, addRoles);
                }
                if (removeRoles != null && removeRoles.Count() > 0)
                {
                  await this._appUserManager.RemoveFromRolesAsync(appUserEntity, removeRoles);
                }
              }
              else
              {
                await this._appUserManager.AddToRolesAsync(appUserEntity, requestRoles);
              }
            }
            else
            {
              return BadRequest("Invalid role");
            }
          }
          else
          {
            await this._appUserManager.RemoveFromRolesAsync(appUserEntity, Enum.GetNames(typeof(AppRoleTypeEnum)));
            // foreach (var role in await this._appUserManager.GetRolesAsync(appUserEntity))
            // {
            //   await this._appUserManager.RemoveFromRoleAsync(appUserEntity, role);
            // }
          }
          return Ok();
        }
        return BadRequest();
      }

      return BadRequest();
    }

  }
}