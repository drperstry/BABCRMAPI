using System;
using System.Collections.Generic;

namespace Bab.BatchData.Models;

public partial class Department
{
    public Guid Id { get; set; }

    public string? DepartmentName { get; set; }

    public string? Code { get; set; }
}
