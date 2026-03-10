namespace Bab.BatchData.Models;

public class CustomDebitCard
{
    public Guid Id { get; set; }

    public DateTime BatchDate { get; set; }

    public string? AffiliatedAccountBranch { get; set; }

    public string? AffiliatedAccountNumber { get; set; }

    public string? AtmPinStatus { get; set; }

    public string? BankAlBiladDailyLimit { get; set; }

    public DateTime? CardExpiryDate { get; set; }

    public string? CardHolderName { get; set; }

    public string? CardMailAddress { get; set; }

    public string? CardNumber { get; set; }

    public string? CardStatus { get; set; }

    public string? CardType { get; set; }

    public string? CardProduct { get; set; }

    public string? CardProductDesc { get; set; }

    public decimal? Cif { get; set; }

    public string? CmsreplacementNumber { get; set; }

    public string? DeliveryMethod { get; set; }

    public decimal? InternationalDailyLimit { get; set; }

    public string? InternetTransactionStatus { get; set; }

    public string? IssuanceType { get; set; }

    public string? IssueBranchName { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? LastPoslimitChangeDate { get; set; }

    public string? LinkedAccountNumber { get; set; }

    public DateTime? OriginalIssueDate { get; set; }

    public string? PrimaryFlag { get; set; }

    public DateTime? ReIssueDate { get; set; }

    public decimal? SpandailyLimit { get; set; }

    public bool? VisaDebitCard { get; set; }

    public string? WhitelistCountry { get; set; }

    public decimal? EcomLimit { get; set; }

    public decimal? Wdlimit { get; set; }

    public decimal? PurchaseLimit { get; set; }

    public decimal? MobileNumber { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? GrandFatherName { get; set; }

    public string? LastName { get; set; }
}
