using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using w9wen.dotnet.Template.Core.Entities;

namespace w9wen.dotnet.Template.Infrastructure.Data
{
  public class AppRoleManager : RoleManager<AppRoleEntity>
  {
    #region Fields

    private readonly AppUserManager _appUserManager;

    private readonly AppDbContext _appDbContext;

    #endregion Fields

    #region Constructor

    public AppRoleManager(IRoleStore<AppRoleEntity> store,
                          IEnumerable<IRoleValidator<AppRoleEntity>> roleValidators,
                          ILookupNormalizer keyNormalizer,
                          IdentityErrorDescriber errors,
                          ILogger<RoleManager<AppRoleEntity>> logger,
                          AppUserManager appUserManager,
                          IServiceProvider services) : base(store,
                                                                roleValidators,
                                                                keyNormalizer,
                                                                errors,
                                                                logger)
    {
      _appUserManager = appUserManager;

      _appDbContext = new AppDbContext(
        services.GetRequiredService<DbContextOptions<AppDbContext>>(), null);

    }

    #endregion Constructor

    #region Override

    public override async Task<IdentityResult> CreateAsync(AppRoleEntity role)
    {
      var userName = this._appUserManager.CurrentName ?? "System";
      var now = DateTime.UtcNow;

      role.CreatedUser = userName;
      role.CreatedDateTime = now;
      role.UpdatedUser = userName;
      role.UpdatedDateTime = now;
      role.ValidFlag = true;

      return await base.CreateAsync(role);
    }

    public override async Task<bool> RoleExistsAsync(string roleName)
    {
      var roleEntity = await _appDbContext.Roles.SingleOrDefaultAsync(x => x.Name == roleName && x.ValidFlag);
      return roleEntity != null;
    }

    #endregion Override
  }
}