using System;
using System.Collections.Generic;

namespace BabCrm.Service.ArchiveDataModels;

public partial class CrLmChangePosDailyLimit
{
    public string? CustomerName { get; set; }

    public string? SrNum { get; set; }

    public string? Cif { get; set; }

    public string? ProductName { get; set; }

    public string? PrductLookUpId { get; set; }

    public string? RequestType { get; set; }

    public string? RequestSubType { get; set; }

    public string? SrStatus { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string? UserId { get; set; }

    public string? MwResponse { get; set; }

    public string? AgentComments { get; set; }

    public string? CardNumber { get; set; }

    public string? UndateLimit { get; set; }

    public string? AgreeOnDisclaimer { get; set; }

    public string? AgreeOnChanges { get; set; }
}
