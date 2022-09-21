using Ardalis.Specification;
using w9wen.dotnet.Template.Core.ProjectAggregate;

namespace w9wen.dotnet.Template.Core.ProjectAggregate.Specifications;

public class ProjectByIdWithItemsSpec : Specification<Project>, ISingleResultSpecification
{
  public ProjectByIdWithItemsSpec(int projectId)
  {
    Query
        .Where(project => project.Id == projectId)
        .Include(project => project.Items);
  }
}
