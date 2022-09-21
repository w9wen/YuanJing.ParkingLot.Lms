using System.Globalization;
using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.IdentityModel.Tokens;
using w9wen.dotnet.Template.Core.Entities;
using w9wen.dotnet.Template.Infrastructure.Data;
using w9wen.dotnet.Template.Web.Interfaces;
using w9wen.dotnet.Template.Web.Services;

namespace w9wen.dotnet.Template.Web.Configurations
{
  public static class CoreServicesConfiguration
  {
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {

      // builder.Services.AddControllersWithViews().AddNewtonsoftJson();
      services.AddControllers(options => options.UseNamespaceRouteToken());
      services.AddCors();

      // builder.Services.AddRazorPages();
      services.AddHttpContextAccessor();

      services.AddLocalization(opt =>
      {
        opt.ResourcesPath = "Resources";
      });

      services.Configure<RequestLocalizationOptions>(options =>
      {
        List<CultureInfo> supportedCultures = new List<CultureInfo>
        {
          new CultureInfo("en-US"),
          new CultureInfo("zh-TW")
        };

        options.DefaultRequestCulture = new RequestCulture("zh-TW");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
      });

      services.AddAutoMapper(typeof(MappingProfile).Assembly);
      services.AddHangfire(config =>
            config.UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection")));
      services.AddScoped<ITokenService, TokenService>();

      services.AddHangfireServer();

    }

    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddIdentityCore<AppUserEntity>(options =>
      {
        options.Password.RequireNonAlphanumeric = false;
      })
        .AddUserManager<AppUserManager>()
        .AddRoles<AppRoleEntity>()
        .AddRoleManager<AppRoleManager>()
        .AddSignInManager<SignInManager<AppUserEntity>>()
        .AddRoleValidator<RoleValidator<AppRoleEntity>>()
        .AddEntityFrameworkStores<AppDbContext>();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
          };

          options.Events = new JwtBearerEvents
          {
            OnMessageReceived = context =>
            {
              var accessToken = context.Request.Query["access_token"];
              var path = context.HttpContext.Request.Path;
              // if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
              // {
              //   context.Token = accessToken;
              // }
              return Task.CompletedTask;
            }
          };
        });

      services.AddAuthorization(options =>
      {
        options.AddPolicy("Modify", policy => policy.RequireRole(AppRoleTypeEnum.SuperAdmin.ToString(),
                                                                 AppRoleTypeEnum.Admin.ToString()));
      });
    }

  }
}
