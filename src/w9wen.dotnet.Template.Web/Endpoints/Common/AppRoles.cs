using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using w9wen.dotnet.Template.Core.Entities;

namespace w9wen.dotnet.Template.Web.Endpoints.Common
{
  public class AppRoles : EndpointBaseAsync
      .WithoutRequest
      .WithActionResult<List<string>>
  {

    [HttpGet("[namespace]/roles")]
    [SwaggerOperation(
        Summary = "Gets a list of all roles",
        Description = "Gets a list of all roles",
        OperationId = "Common",
        Tags = new[] { "CommonEndpoints" })
    ]
    public override async Task<ActionResult<List<string>>> HandleAsync(CancellationToken cancellationToken = default)
    {
      var list = Enum.GetNames(typeof(AppRoleTypeEnum)).ToList();
      return Ok(list);
    }
  }
}