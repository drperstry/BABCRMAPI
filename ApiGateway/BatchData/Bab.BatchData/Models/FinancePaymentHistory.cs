using System;
using System.Collections.Generic;

namespace Bab.BatchData.Models;

public partial class FinancePaymentHistory
{
    public Guid Id { get; set; }

    public DateTime BatchDate { get; set; }

    public decimal Cif { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Date { get; set; }

    public decimal? OutstandingAmount { get; set; }

    public string? InstallmentNumber { get; set; }

    public string? LoanId { get; set; }
}
