using System;
using System.Collections.Generic;

namespace BabCrm.Service.ArchiveDataModels;

public partial class StopCheque
{
    public string? SrNum { get; set; }

    public string? RequestType { get; set; }

    public string? RequestSubType { get; set; }

    public string? SrStatus { get; set; }

    public string? Description { get; set; }

    public string? UserId { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string? Cif { get; set; }

    public string? CustomerName { get; set; }

    public string? ProductName { get; set; }

    public string? PrductLookUpId { get; set; }

    public string? AccountNumber { get; set; }

    public string? ReceivingBranch { get; set; }

    public string? MwResponse { get; set; }

    public string? ChequeType { get; set; }

    public decimal? Cheque { get; set; }

    public string? PaidTo { get; set; }

    public DateTime? ChequeDate { get; set; }

    public decimal? FromNumber { get; set; }

    public decimal? ToNumber { get; set; }

    public string? Description1 { get; set; }

    public string? DisclaimerAgreed { get; set; }

    public string? SmsDisclaimer { get; set; }

    public string? StopReason { get; set; }

    public string? ChequeBookType { get; set; }

    public decimal? ChequeBookSize { get; set; }

    public decimal? ChequeBookQuantity { get; set; }
}
