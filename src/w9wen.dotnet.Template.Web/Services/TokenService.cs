using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using w9wen.dotnet.Template.Core.Entities;
using w9wen.dotnet.Template.Infrastructure.Data;
using w9wen.dotnet.Template.Web.Interfaces;

namespace w9wen.dotnet.Template.Web.Services
{
  public class TokenService : ITokenService
  {
    private readonly IConfiguration _config;
    private readonly AppUserManager _appUserManager;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config, AppUserManager appUserManager)
    {
      _config = config;
      _appUserManager = appUserManager;
      _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
    }

    public async Task<string> CreateToken(AppUserEntity user)
    {
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
      };

      var roleList = await _appUserManager.GetRolesAsync(user);
      claims.AddRange(roleList.Select(x => new Claim(ClaimTypes.Role, x)));

      var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(7),
        SigningCredentials = creds,
        
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}