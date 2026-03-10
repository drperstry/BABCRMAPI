namespace Bab.BatchData.Models;

public class CustomFinancePaymentHistory 
{
    public Guid Id { get; set; }

    public DateTime BatchDate { get; set; }

    public decimal Cif { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Date { get; set; }

    public decimal? OutstandingAmount { get; set; }

    public string? InstallmentNumber { get; set; }

    public string? LoanId { get; set; }

    public decimal? MobileNumber { get; set; }

    public string? Email { get; set; }
}