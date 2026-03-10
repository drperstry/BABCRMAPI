using System;
using System.Collections.Generic;

namespace BabCrm.Service.ArchiveDataModels;

public partial class CaseRequest
{
    public string? SrNum { get; set; }

    public string? CustomerName { get; set; }

    public string? Cif { get; set; }

    public string? ProductName { get; set; }

    public string? PrductLookUpId { get; set; }

    public string? RequestType { get; set; }

    public string? RequestSubType { get; set; }

    public string? SrStatus { get; set; }

    public string? CaseType { get; set; }

    public string? Type { get; set; }

    public string? SubType { get; set; }

    public string? MobileNumber { get; set; }

    public string? Department { get; set; }

    public string? ReveiveMethod { get; set; }

    public string? Branch { get; set; }

    public string? Priority { get; set; }

    public string? Source { get; set; }

    public string? Description { get; set; }

    public string? IsEligibleOfficialLetter { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? AccountNumber { get; set; }

    public string? FinalReplyStatment { get; set; }

    public string? TransactionReference { get; set; }

    public string? TransactionAuthorizationCode { get; set; }

    public string? TransactionDate { get; set; }

    public string? TransactionCardType { get; set; }

    public string? MwResponse { get; set; }

    public string? AgentComments { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string? UserId { get; set; }

    public Guid? CrmCategoryId { get; set; }

    public Guid? CrmTypeId { get; set; }

    public Guid? CrmSubTypeId { get; set; }

    public Guid? CrmProductId { get; set; }

    public Guid? CrmStatusId { get; set; }

    public virtual CrmRequestCategory? CrmCategory { get; set; }

    public virtual CrmProduct? CrmProduct { get; set; }

    public virtual CrmRequestStatus? CrmStatus { get; set; }

    public virtual CrmRequestSubType? CrmSubType { get; set; }

    public virtual CrmRequestType? CrmType { get; set; }
}
