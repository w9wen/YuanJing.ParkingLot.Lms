using w9wen.dotnet.Template.Web.Configurations;

namespace w9wen.dotnet.Template.Web.Endpoints.Employee
{
  public class EmployeeListRequest : PaginationParams
  {
    public const string Route = "[namespace]";

    public string? Gender { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string OrderBy { get; set; } = "updatedDateTime";

  }
}