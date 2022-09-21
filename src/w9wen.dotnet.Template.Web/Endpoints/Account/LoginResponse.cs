using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace w9wen.dotnet.Template.Web.Endpoints.Account
{
  public class LoginResponse
  {
    public string? UserName { get; set; }

    public string? Token { get; set; }

    public string? KnownAs { get; set; }

    public string? Gender { get; set; }
  }
}