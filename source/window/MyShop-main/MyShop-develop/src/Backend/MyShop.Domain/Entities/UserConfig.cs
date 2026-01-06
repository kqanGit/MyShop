using System;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class UserConfig
{
    public int SettingId { get; set; }

    public int? UserId { get; set; }

    public int? PerPage { get; set; }

    public string? LastModule { get; set; }

    public virtual User? User { get; set; }
}
