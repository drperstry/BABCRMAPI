namespace Bab.BatchData.Models;

public class CustomLoan 
{
    public Guid Id { get; set; }

    public DateTime? BatchDate { get; set; }

    public string? BranchName { get; set; }

    public decimal? Cif { get; set; }

    public string? City { get; set; }

    public decimal? ConsumerAmount { get; set; }

    public decimal? DueAmount { get; set; }

    public string? GlobalDpr { get; set; }

    public decimal? InstallmentAmount { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string? LinkedAccountBranch { get; set; }

    public string? LinkedAccountNumber { get; set; }

    public string? LoanCategory { get; set; }

    public string? LoanCategoryDescription { get; set; }

    public string? LoanCurrency { get; set; }

    public string? LoanId { get; set; }

    public DateTime? MaturityDate { get; set; }

    public DateTime? NextPaymentDate { get; set; }

    public decimal? OutstandingAmount { get; set; }

    public decimal? PastDueNumberOfdays { get; set; }

    public decimal? PrincipalAmount { get; set; }

    public string? ProductDpr { get; set; }

    public decimal? ProfitAnnualRate { get; set; }

    public decimal? ProfitFlatRate { get; set; }

    public decimal? RemainingProfitAmount { get; set; }

    public decimal? RemainingInstallments { get; set; }

    public DateTime? ContractStartDate { get; set; }

    public decimal? TotalNumberOfInstallments { get; set; }

    public decimal? CarPrice { get; set; }

    public bool? CommitmentFeeFlag { get; set; }

    public string? DelinquencyStatus { get; set; }

    public string? DepartmentCode { get; set; }

    public string? DepartmentCodeText { get; set; }

    public string? EmployerCode { get; set; }

    public decimal? ExchangeRate { get; set; }

    public DateTime? ExecutionDate { get; set; }

    public decimal? GracePeriod { get; set; }

    public string? InternalStaff { get; set; }

    public DateTime? LastInstallmentDate { get; set; }

    public DateTime? LastInstallmentBpdate { get; set; }

    public DateTime? NextInstallmentIncPd { get; set; }

    public decimal? OverdueAmountBase { get; set; }

    public decimal? OverdueAmount { get; set; }

    public string? OverdueDate { get; set; }

    public string? LoanRequestRef { get; set; }

    public string? ProductDescription { get; set; }

    public string? ProductCode { get; set; }

    public decimal? SalaryAmount { get; set; }

    public decimal? TotalLdpaidAmount { get; set; }

    public DateTime? CarDeliveryDateGregorian { get; set; }

    public string? ChassisNumber { get; set; }

    public string? DealerCity { get; set; }

    public string? DealerName { get; set; }

    public string? DeptClass { get; set; }

    public decimal? BalloonPaymentAmount { get; set; }

    public DateTime? BalloonPaymentDate { get; set; }

    public decimal? MobileNumber { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? GrandFatherName { get; set; }

    public string? LastName { get; set; }
}
