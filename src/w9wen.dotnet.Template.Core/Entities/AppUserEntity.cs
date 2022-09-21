using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using w9wen.SharedKernel;
using w9wen.SharedKernel.Interfaces;


namespace w9wen.dotnet.Template.Core.Entities
{
  public class AppUserEntity : IdentityUser<int>, IAggregateRoot
  {
    public DateTime DateOfBirth { get; set; }

    public string? KnownAs { get; set; }

    public string? Gender { get; set; }

    public string? Introduction { get; set; }

    public string? Interests { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }


    public ICollection<AppUserRoleEntity>? AppUserRoles { get; set; }

    #region BaseEntity

    [DisplayName("備註")]
    [DataType(DataType.MultilineText)]
    [StringLength(200)]
    public string? Note { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    [Required]
    [DisplayName("建立時間")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
    [Editable(false)]
    [UIHint("DateTime")]
    public DateTime CreatedDateTime { get; set; }

    /// <summary>
    /// 建立者
    /// </summary>
    [Required]
    [DisplayName("建立者")]
    [Editable(false)]
    [StringLength(50)]
    public string? CreatedUser { get; set; }

    /// <summary>
    /// 更新次數
    /// </summary>
    [Required]
    [DisplayName("更新次數")]
    [Editable(false)]
    public byte UpdatedTimes { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    [Required]
    [DisplayName("更新時間")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
    [Editable(false)]
    [UIHint("DateTime")]
    public DateTime UpdatedDateTime { get; set; }

    /// <summary>
    /// 更新者
    /// </summary>
    [Required]
    [DisplayName("更新者")]
    [Editable(false)]
    [StringLength(50)]
    public string? UpdatedUser { get; set; }

    /// <summary>
    /// 生效標記
    /// </summary>
    [Required]
    [DisplayName("生效標記")]
    [Editable(false)]
    public bool ValidFlag { get; set; }

    public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();

    #endregion BaseEntity

  }
}