using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using w9wen.dotnet.Template.Core.Entities;

namespace w9wen.dotnet.Template.Infrastructure.Data
{
  public class AppUserManager : UserManager<AppUserEntity>
  {
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly AppDbContext _appDbContext;

    public AppUserManager(IUserStore<AppUserEntity> store,
                          IOptions<IdentityOptions> optionsAccessor,
                          IPasswordHasher<AppUserEntity> passwordHasher,
                          IEnumerable<IUserValidator<AppUserEntity>> userValidators,
                          IEnumerable<IPasswordValidator<AppUserEntity>> passwordValidators,
                          ILookupNormalizer keyNormalizer,
                          IdentityErrorDescriber errors,
                          IServiceProvider services,
                          ILogger<UserManager<AppUserEntity>> logger) : base(store,
                                                                             optionsAccessor,
                                                                             passwordHasher,
                                                                             userValidators,
                                                                             passwordValidators,
                                                                             keyNormalizer,
                                                                             errors,
                                                                             services,
                                                                             logger)
    {
      _httpContextAccessor = services.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
      _appDbContext = new AppDbContext(
          services.GetRequiredService<DbContextOptions<AppDbContext>>(), null);

    }


    public string CurrentName => GetCurrentUserAsync();

    private string? GetCurrentUserAsync()
    {
      if (_httpContextAccessor?.HttpContext != null)
      {
        var claimsPrincipal = _httpContextAccessor.HttpContext.User;

        return claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
      }
      return null;
    }

    public override async Task<AppUserEntity> FindByIdAsync(string userId)
    {
      var user = await base.Users.SingleOrDefaultAsync(u => u.Id.ToString() == userId && u.ValidFlag);
      return user;
    }

    public override async Task<AppUserEntity> FindByNameAsync(string userName)
    {
      var user = await base.Users.SingleOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower() && u.ValidFlag);
      return user;
    }

    public override IQueryable<AppUserEntity> Users
      => base.Users
        .Where(u => u.ValidFlag)
        .Include(r => r.AppUserRoles.Where(ur => ur.ValidFlag))
        .ThenInclude(r => r.AppRole);

    public override async Task<IdentityResult> CreateAsync(AppUserEntity user, string password)
    {
      var userName = this.CurrentName ?? "System";
      var now = DateTime.UtcNow;

      user.CreatedUser = userName;
      user.CreatedDateTime = now;
      user.UpdatedUser = userName;
      user.UpdatedDateTime = now;
      user.ValidFlag = true;

      return await base.CreateAsync(user, password);
    }

    public override async Task<IdentityResult> UpdateAsync(AppUserEntity user)
    {
      var userName = this.CurrentName ?? "System";
      var now = DateTime.UtcNow;
      user.UpdatedUser = userName;
      user.UpdatedDateTime = now;
      return await base.UpdateAsync(user);
    }

    public override async Task<IdentityResult> AddToRoleAsync(AppUserEntity user, string role)
    {
      var identityErrorList = new List<IdentityError>();
      try
      {
        var appRole = await _appDbContext.AppRoleDB.SingleOrDefaultAsync(x => x.Name == role && x.ValidFlag);
        if (appRole != null)
        {
          var userName = this.CurrentName ?? "System";
          var now = DateTime.UtcNow;

          var userRole = new AppUserRoleEntity
          {
            UserId = user.Id,
            RoleId = appRole.Id,
            CreatedUser = userName,
            CreatedDateTime = now,
            UpdatedUser = userName,
            UpdatedDateTime = now,
            ValidFlag = true,
          };
          await _appDbContext.AppUserRoleDB.AddAsync(userRole);
          await _appDbContext.SaveChangesAsync();
          return IdentityResult.Success;
        }

        var identityError = new IdentityError() { Description = $"Could not find : {role}" };
        identityErrorList.Add(identityError);

        return IdentityResult.Failed(identityErrorList.ToArray());

      }
      catch (Exception ex)
      {
        return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = ex.Message } });
      }
    }

    public override async Task<IdentityResult> AddToRolesAsync(AppUserEntity user, IEnumerable<string> roles)
    {
      var appUserRoleListForAdd = new List<AppUserRoleEntity>();
      var appUserRoleListForUpdate = new List<AppUserRoleEntity>();
      var identityErrorList = new List<IdentityError>();

      try
      {
        var userName = this.CurrentName ?? "System";
        var now = DateTime.UtcNow;
        foreach (var roleName in roles)
        {
          var role = await _appDbContext.AppRoleDB.SingleOrDefaultAsync(x => x.Name == roleName && x.ValidFlag);
          if (role != null)
          {
            /// Find all the user roles include ValidFlag == false.
            var userRole = await _appDbContext.AppUserRoleDB.SingleOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            if (userRole != null)
            {
              if (!userRole.ValidFlag)
              {
                userRole.UpdatedUser = userName;
                userRole.UpdatedDateTime = now;
                userRole.ValidFlag = true;
                appUserRoleListForUpdate.Add(userRole);
              }
            }
            else
            {
              userRole = new AppUserRoleEntity
              {
                UserId = user.Id,
                RoleId = role.Id,
                CreatedUser = userName,
                CreatedDateTime = now,
                UpdatedUser = userName,
                UpdatedDateTime = now,
                ValidFlag = true,
              };
              appUserRoleListForAdd.Add(userRole);
            }
          }
          else
          {
            var identityError = new IdentityError() { Description = $"Could not find : {roleName}" };
            identityErrorList.Add(identityError);
          }
        }

        if (identityErrorList.Count > 0) return IdentityResult.Failed(identityErrorList.ToArray());

        if (appUserRoleListForUpdate.Count > 0)
        {
          _appDbContext.AppUserRoleDB.UpdateRange(appUserRoleListForUpdate);
        }
        if (appUserRoleListForAdd.Count > 0)
        {
          await _appDbContext.AppUserRoleDB.AddRangeAsync(appUserRoleListForAdd);
        }

        await _appDbContext.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (Exception ex)
      {
        return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = ex.Message } });
      }
    }

    public override async Task<IdentityResult> RemoveFromRolesAsync(AppUserEntity user, IEnumerable<string> roles)
    {
      try
      {
        var userRoleList = await _appDbContext.Roles.Where(x => roles.Contains(x.Name) && x.ValidFlag).ToListAsync();
        foreach (var userRole in userRoleList)
        {
          var appUserRole = await _appDbContext.UserRoles.SingleOrDefaultAsync(x => x.UserId == user.Id && x.RoleId == userRole.Id && x.ValidFlag);
          if (appUserRole != null)
          {
            var userName = this.CurrentName ?? "System";
            var now = DateTime.UtcNow;
            appUserRole.UpdatedUser = userName;
            appUserRole.UpdatedDateTime = now;
            appUserRole.ValidFlag = false;
            _appDbContext.UserRoles.Update(appUserRole);
          }
        }

        await _appDbContext.SaveChangesAsync();

        return IdentityResult.Success;
      }
      catch (Exception ex)
      {
        return IdentityResult.Failed(new IdentityError[] { new IdentityError { Description = ex.Message } });
      }

    }

    public override async Task<IList<string>> GetRolesAsync(AppUserEntity user)
    {
      var appUserRole = await _appDbContext.UserRoles
        .Where(x => x.UserId == user.Id && x.ValidFlag)
        .Include(ur => ur.AppRole)
          .ThenInclude(r => r.AppUserRoles)
          .Select(r => r.AppRole.Name)
          .ToListAsync();
      return appUserRole;
    }
  }
}