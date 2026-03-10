using System;
using System.Collections.Generic;

namespace BabCrm.Service.ArchiveDataModels;

public partial class CrmRequestSubType
{
    public Guid Id { get; set; }

    public string? InternalEnglishName { get; set; }

    public string? InternalArabicName { get; set; }

    public string? SamaEnglishName { get; set; }

    public string? SamaArabicName { get; set; }
}
