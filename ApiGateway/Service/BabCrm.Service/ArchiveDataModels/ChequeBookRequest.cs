using System;
using System.Collections.Generic;

namespace BabCrm.Service.ArchiveDataModels;

public partial class ChequeBookRequest
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

    public string? ChequeBookType { get; set; }

    public decimal? ChequeBookSize { get; set; }

    public decimal? ChequeBookQuantity { get; set; }

    public string? ReceivingBranch { get; set; }

    public string? MwResponse { get; set; }

    public string? SrArea { get; set; }

    public string? SrSubArea { get; set; }

    public string? SrStatId { get; set; }

    public string? SrTitle { get; set; }

    public string? Login { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? ActCloseDt { get; set; }

    public string? Csn { get; set; }

    public string? AliasName { get; set; }

    public string? Transactiondesc { get; set; }

    public DateTime? Attrib31 { get; set; }

    public DateTime? Attrib26 { get; set; }

    public string? Investmentaccountnum { get; set; }

    public DateTime? Attrib28 { get; set; }

    public string? ChCheqName2 { get; set; }

    public string? BpServiceDesc { get; set; }

    public string? Name { get; set; }
}
