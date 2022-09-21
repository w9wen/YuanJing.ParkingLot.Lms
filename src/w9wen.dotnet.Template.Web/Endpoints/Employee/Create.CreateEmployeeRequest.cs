using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace w9wen.dotnet.Template.Web.Endpoints.Employee
{
  public class CreateEmployeeRequest
  {
    public const string Route = "[namespace]";

    [Required]
    public string? UserName { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? KnownAs { get; set; }

    public string? Gender { get; set; }

    public string? Introduction { get; set; }

    public string? Interests { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? Note { get; set; }

    // [Required]
    // public List<string>? AppRoleList { get; set; }

    public List<string>? Roles { get; set; }

  }
}