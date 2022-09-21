using System.ComponentModel.DataAnnotations;

namespace w9wen.dotnet.Template.Web.Endpoints.ProjectEndpoints;

public class CreateProjectRequest
{
  public const string Route = "/Projects";

  [Required]
  public string? Name { get; set; }
}
