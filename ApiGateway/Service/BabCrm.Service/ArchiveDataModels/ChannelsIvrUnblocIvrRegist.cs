using System;
using System.Collections.Generic;

namespace BabCrm.Service.ArchiveDataModels;

public partial class ChannelsIvrUnblocIvrRegist
{
    public string? CustomerName { get; set; }

    public string? SrNum { get; set; }

    public string? Cif { get; set; }

    public string? ProductName { get; set; }

    public string? PrductLookUpId { get; set; }

    public string? RequestType { get; set; }

    public string? RequestSubType { get; set; }

    public string? SrStatus { get; set; }

    public string? AccountNumber { get; set; }

    public string? IvrUserId { get; set; }

    public string? VerballyAuthenticated { get; set; }

    public string? MwResponse { get; set; }

    public string? AgentComments { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string? UserId { get; set; }
}
