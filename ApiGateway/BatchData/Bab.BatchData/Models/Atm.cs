using System;
using System.Collections.Generic;


namespace Bab.BatchData.Models;

public partial class Atm
{
    public Guid Id { get; set; }

    public string? AtmName { get; set; }

    public string? Code { get; set; }

    public string? AtmLocationUrl { get; set; }
}
