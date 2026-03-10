using System;
using System.Collections.Generic;

namespace Bab.BatchData.Models;

public partial class Branch
{
    public Guid Id { get; set; }

    public string? BranchName { get; set; }

    public string? Code { get; set; }

    public string? BranchLocationUrl { get; set; }
}
