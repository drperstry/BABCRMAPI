using System;
using System.Collections.Generic;

namespace BabCrm.Service.ArchiveDataModels;

public partial class RequestStatus
{
    public Guid? Id { get; set; }

    public string? EnglishName { get; set; }

    public string? ArabicName { get; set; }
}
