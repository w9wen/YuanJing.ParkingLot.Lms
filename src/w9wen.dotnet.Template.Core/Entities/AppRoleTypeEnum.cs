using System.ComponentModel.DataAnnotations;

namespace w9wen.dotnet.Template.Core.Entities
{
  public enum AppRoleTypeEnum
  {
    /// <summary>
    /// 最高管理者
    /// </summary>
    [Display(Name = "最高管理者")]
    SuperAdmin,

    /// <summary>
    /// 管理者
    /// </summary>
    [Display(Name = "管理者")]
    Admin,

    /// <summary>
    /// 操作員
    /// </summary>
    [Display(Name = "操作員")]
    Operator,

    /// <summary>
    /// 會員
    /// </summary>
    [Display(Name = "會員")]
    Member,
  }
}
