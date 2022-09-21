using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using w9wen.dotnet.Template.Core.Entities;

namespace w9wen.dotnet.Template.Web.Interfaces
{
  public interface ITokenService
  {
    Task<string> CreateToken(AppUserEntity user);
  }
}