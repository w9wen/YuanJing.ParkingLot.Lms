using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace src.w9wen.dotnet.Template.Web.Endpoints.Employee
{
  public class GetDetailByUserNameRequest
  {
    public const string Route = "[namespace]/{UserName}";

    [Required]
    [FromRoute]
    public string? UserName { get; set; }
  }
}