namespace Bab.BatchData.Models;

public partial class CustomFinancialAccount 
{
    public Guid Id { get; set; }

    public DateTime? BatchDate { get; set; }

    public decimal? AccountCode { get; set; }

    public string? AccountNumber { get; set; }

    public string? AccountType { get; set; }

    public string? ArabicName { get; set; }

    public string? Branch { get; set; }

    public string? Currency { get; set; }

    public decimal? Cif { get; set; }

    public string? EnglishName { get; set; }

    public string? Household { get; set; }

    public string? Ibannumber { get; set; }

    public string? NickName { get; set; }

    public DateTime? OpenDate { get; set; }

    public decimal? MobileNumber { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? GrandFatherName { get; set; }

    public string? LastName { get; set; }
}
