using w9wen.dotnet.Template.Core.ProjectAggregate;
using w9wen.dotnet.Template.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using w9wen.dotnet.Template.Core.Entities;

namespace w9wen.dotnet.Template.Web;

public static class SeedData
{
  public static readonly Project TestProject1 = new Project("Test Project", PriorityStatus.Backlog);

  private static DateTime now = DateTime.Now;

  private static string systemUser = "System";

  public static readonly ToDoItem ToDoItem1 = new ToDoItem
  {
    Title = "Get Sample Working",
    Description = "Try to get the sample to build.",
    CreatedUser = systemUser,
    CreatedDateTime = now,
    UpdatedUser = systemUser,
    UpdatedDateTime = now,
    ValidFlag = true,
  };
  public static readonly ToDoItem ToDoItem2 = new ToDoItem
  {
    Title = "Review Solution",
    Description = "Review the different projects in the solution and how they relate to one another.",
    CreatedUser = systemUser,
    CreatedDateTime = now,
    UpdatedUser = systemUser,
    UpdatedDateTime = now,
    ValidFlag = true,
  };
  public static readonly ToDoItem ToDoItem3 = new ToDoItem
  {
    Title = "Run and Review Tests",
    Description = "Make sure all the tests run and review what they are doing.",
    CreatedUser = systemUser,
    CreatedDateTime = now,
    UpdatedUser = systemUser,
    UpdatedDateTime = now,
    ValidFlag = true,
  };

  public static async Task Initialize(IServiceProvider serviceProvider)
  {
    using (var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
    {

      dbContext.Database.MigrateAsync().GetAwaiter().GetResult();

      // Look for any TODO items.
      if (!dbContext.ToDoItems.Any())
      {
        PopulateTestData(dbContext);
      }

      var appUserManager = serviceProvider.GetRequiredService<AppUserManager>();
      var appRoleManager = serviceProvider.GetRequiredService<AppRoleManager>();

      await SeedUsers(appUserManager, appRoleManager);


    }
  }
  public static void PopulateTestData(AppDbContext dbContext)
  {
    foreach (var item in dbContext.Projects)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.ToDoItems)
    {
      dbContext.Remove(item);
    }
    dbContext.SaveChanges();

    TestProject1.CreatedUser = systemUser;
    TestProject1.CreatedDateTime = now;
    TestProject1.UpdatedUser = systemUser;
    TestProject1.UpdatedDateTime = now;
    TestProject1.ValidFlag = true;

    TestProject1.AddItem(ToDoItem1);
    TestProject1.AddItem(ToDoItem2);
    TestProject1.AddItem(ToDoItem3);
    dbContext.Projects.Add(TestProject1);

    dbContext.SaveChanges();
  }


  private static async Task SeedUsers(AppUserManager appUserManager,
                                     AppRoleManager appRoleManager)
  {

    if (!await appRoleManager.RoleExistsAsync(AppRoleTypeEnum.SuperAdmin.ToString()))
      await appRoleManager.CreateAsync(new AppRoleEntity { Name = AppRoleTypeEnum.SuperAdmin.ToString() });

    if (!await appRoleManager.RoleExistsAsync(AppRoleTypeEnum.Admin.ToString()))
      await appRoleManager.CreateAsync(new AppRoleEntity { Name = AppRoleTypeEnum.Admin.ToString() });

    if (!await appRoleManager.RoleExistsAsync(AppRoleTypeEnum.Operator.ToString()))
      await appRoleManager.CreateAsync(new AppRoleEntity { Name = AppRoleTypeEnum.Operator.ToString() });

    if (!await appRoleManager.RoleExistsAsync(AppRoleTypeEnum.Member.ToString()))
      await appRoleManager.CreateAsync(new AppRoleEntity { Name = AppRoleTypeEnum.Member.ToString() });

    // if (await appUserManager.Users.AnyAsync()) return;
    // var userData = await File.ReadAllTextAsync("UserSeedData.json");

    // var users = JsonSerializer.Deserialize<List<AppUserEntity>>(userData);
    // if (users == null) return;

    // foreach (var user in users)
    // {
    //   await appUserManager.CreateAsync(user, "P@$$w0rd");
    //   await appUserManager.AddToRoleAsync(user, Enum.GetName(AppRoleTypeEnum.Member));
    // }

    var appUserEntity = new AppUserEntity();

    appUserEntity = await appUserManager.FindByNameAsync("Admin");

    if (appUserEntity == null)
    {
      appUserEntity = new AppUserEntity { UserName = "Admin" };

      var identityResult = await appUserManager.CreateAsync(appUserEntity, "Aa!23456");

      if (identityResult.Succeeded)
      {
        await appUserManager.AddToRoleAsync(appUserEntity, AppRoleTypeEnum.Admin.ToString());
      }
    }

    appUserEntity = await appUserManager.FindByNameAsync("w9wen");

    if (appUserEntity == null)
    {
      appUserEntity = new AppUserEntity { UserName = "w9wen" };

      var identityResult = await appUserManager.CreateAsync(appUserEntity, "Aa!23456");

      if (identityResult.Succeeded)
      {
        await appUserManager.AddToRoleAsync(appUserEntity, AppRoleTypeEnum.SuperAdmin.ToString());
      }
    }
  }
}
