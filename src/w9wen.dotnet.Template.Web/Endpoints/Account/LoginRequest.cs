using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace w9wen.dotnet.Template.Web.Endpoints.Account
{
  public class LoginRequest
  {

    public const string Route = "/Account/Login";

    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }
  }
}