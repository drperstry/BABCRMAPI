using BabCrm.Service.Models;

public class TppTicketModel
{
    public string Id { get; set; }
    public string TicketNumber { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UserId { get; set; }
    public string StatusCode { get; set; }
    public string SeverityTPP { get; set; }
    public string SeverityPASP { get; set; }
    public string ReportingOrganization { get; set; }
    public string ProblemDetails { get; set; }
    public string Phone { get; set; }
    public string OwnerId { get; set; }
    public string Email { get; set; }
    public DateTime? DateClosed { get; set; }
    public string BankBrand { get; set; }
    public string BankEnvironment { get; set; }
    public DateTime? ExpectedResolutionStartDate { get; set; }
    public DateTime? ExpectedResolutionEndDate { get; set; }
    public LookupModel Category { get; set; }
    public LookupModel Type { get; set; }
}
