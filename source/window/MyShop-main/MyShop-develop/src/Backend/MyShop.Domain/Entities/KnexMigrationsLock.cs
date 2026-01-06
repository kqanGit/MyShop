using System;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class KnexMigrationsLock
{
    public int Index { get; set; }

    public int? IsLocked { get; set; }
}
