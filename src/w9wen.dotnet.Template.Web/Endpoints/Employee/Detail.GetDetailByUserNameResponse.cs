using System.ComponentModel.DataAnnotations;

namespace src.w9wen.dotnet.Template.Web.Endpoints.Employee
{
  public class GetDetailByUserNameResponse
  {
    [Required]
    public EmployeeDto? EmployeeItem { get; set; }
  }
}