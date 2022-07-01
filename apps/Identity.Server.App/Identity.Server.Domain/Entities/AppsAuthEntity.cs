using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Identity.Server.Domain.Entities;

[Dapper.Contrib.Extensions.Table("apps_auth")]
public sealed class AppsAuthEntity
{
    [Key]
    public int AppAuthId { get; set; }
    public string ClientKey { get; set; }
    public string ClientSecret { get; set; }
}
