using System.ComponentModel.DataAnnotations;
using w9wen.dotnet.Template.Core.Entities;

namespace src.w9wen.dotnet.Template.Web.Endpoints.Employee;

public class EmployeeDto
{
  [Required]
  public int Id { get; set; }

  [Required]
  public string? UserName { get; set; }

  public DateTime CreatedDateTime { get; set; }

  public DateTime UpdatedDateTime { get; set; }

  //   public ICollection<AppUserRoleEntity>? AppUserRoles { get; set; }

  public DateTime DateOfBirth { get; set; }

  public string? KnownAs { get; set; }

  public string? Gender { get; set; }

  public string? Introduction { get; set; }

  public string? Interests { get; set; }

  public string? City { get; set; }

  public string? Country { get; set; }

  public List<string>? Roles { get; set; }

}