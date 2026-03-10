namespace Bab.BatchData.Models;

public class CustomCreditCard
{
    public Guid Id { get; set; }

    public DateTime? BatchDate { get; set; }

    public string? CmsaccountNumber { get; set; }

    public string? CmsaccountStatus { get; set; }

    public decimal? ActualTotalAvailableAmount { get; set; }

    public string? CardClass { get; set; }

    public DateTime? CardExpiryDate { get; set; }

    public string? CardNumber { get; set; }

    public string? CardStatus { get; set; }

    public DateTime? CardStatusDate { get; set; }

    public string? CardType { get; set; }

    public string? CardProductCode { get; set; }

    public string? CardProductDesc { get; set; }

    public string? CmsreplacementNumber { get; set; }

    public string? Currency { get; set; }

    public DateTime? IssusDate { get; set; }

    public string? IssusedBranch { get; set; }

    public string? LinkedAccount { get; set; }

    public string? MailAddress { get; set; }

    public decimal? MaxPaymentNo { get; set; }

    public decimal? MinimumDueAmount { get; set; }

    public string? NickName { get; set; }

    public decimal? PrimaryCard { get; set; }

    public decimal? Cif { get; set; }

    public decimal? MobileNumber { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? GrandFatherName { get; set; }

    public string? LastName { get; set; }
}
