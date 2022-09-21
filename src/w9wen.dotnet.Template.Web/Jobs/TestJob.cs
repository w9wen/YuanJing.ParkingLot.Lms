using Hangfire;

namespace w9wen.dotnet.Template.Web.Jobs
{
  public class TestJob
  {
    private readonly ILogger<TestJob> _logger;

    public TestJob(ILogger<TestJob> logger)
    {
      _logger = logger;
    }

    [JobDisplayName("TestJob")]
    public async Task Execute(int seconds)
    {
      await Task.Run(() =>
      {
        _logger.LogDebug("Start TestJob");
        Task.Delay(seconds * 1000).Wait();
        _logger.LogDebug("Stop TestJob");
      });
    }
  }
}
