using w9wen.dotnet.Template.Core.ProjectAggregate;
using w9wen.SharedKernel;

namespace w9wen.dotnet.Template.Core.ProjectAggregate.Events;

public class ToDoItemCompletedEvent : BaseDomainEvent
{
  public ToDoItem CompletedItem { get; set; }

  public ToDoItemCompletedEvent(ToDoItem completedItem)
  {
    CompletedItem = completedItem;
  }
}
