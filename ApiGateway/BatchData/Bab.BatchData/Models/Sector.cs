using System;
using System.Collections.Generic;

namespace Bab.BatchData.Models;

public partial class Sector
{
    public Guid Id { get; set; }

    public string? SectorName { get; set; }

    public string? Code { get; set; }
}
