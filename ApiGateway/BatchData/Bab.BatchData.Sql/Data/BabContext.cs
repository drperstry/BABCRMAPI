using Bab.BatchData.Models;
using Microsoft.EntityFrameworkCore;

namespace Bab.BatchData.Sql.Data;

public partial class BabContext : DbContext
{
    public BabContext()
    {
    }

    public BabContext(DbContextOptions<BabContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atm> Atms { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<CustomCreditCard> CustomCreditCards { get; set; }

    public virtual DbSet<DebitCard> DebitCards { get; set; }

    public virtual DbSet<CustomDebitCard> CustomDebitCards { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<FinancePaymentHistory> FinancePaymentHistories { get; set; }

    public virtual DbSet<CustomFinancePaymentHistory> CustomFinancePaymentHistories { get; set; }

    public virtual DbSet<FinancialAccount> FinancialAccounts { get; set; }

    public DbSet<CustomFinancialAccount> CustomFinancialAccount { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<CustomLoan> CustomLoans { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BAB_BatchData;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Atm>(entity =>
        {
            entity.ToTable("ATM");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AtmLocationUrl)
                .HasMaxLength(100)
                .HasColumnName("SQL_ATMLocationUrl");
            entity.Property(e => e.AtmName)
                .HasMaxLength(100)
                .HasColumnName("SQL_ATMName");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("SQL_Code");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("Branch");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BranchLocationUrl)
                .HasMaxLength(100)
                .HasColumnName("SQL_BranchLocationUrl");
            entity.Property(e => e.BranchName)
                .HasMaxLength(100)
                .HasColumnName("SQL_BranchName");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("SQL_Code");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("Contact");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.BirthdateGregorian)
                .HasColumnType("date")
                .HasColumnName("SQL_BirthdateGregorian");
            entity.Property(e => e.BirthdateHijri)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_BirthdateHijri");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("SQL_City");
            entity.Property(e => e.CustRisk)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CustRisk");
            entity.Property(e => e.CustomerNameArabic)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("SQL_CustomerNameArabic");
            entity.Property(e => e.CustomerNameEnglish)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("SQL_CustomerNameEnglish");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("SQL_Email");
            entity.Property(e => e.Employer)
                .HasMaxLength(150)
                .HasColumnName("SQL_Employer");
            entity.Property(e => e.FamilyName)
                .HasMaxLength(100)
                .HasColumnName("SQL_FamilyName");
            entity.Property(e => e.FamilyNameEn)
                .HasMaxLength(100)
                .HasColumnName("SQL_FamilyNameEn");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("SQL_FirstName");
            entity.Property(e => e.FirstNameEn)
                .HasMaxLength(100)
                .HasColumnName("SQL_FirstNameEn");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("SQL_Gender");
            entity.Property(e => e.GrandfatherName)
                .HasMaxLength(100)
                .HasColumnName("SQL_GrandfatherName");
            entity.Property(e => e.GrandfatherNameEn)
                .HasMaxLength(100)
                .HasColumnName("SQL_GrandfatherNameEn");
            entity.Property(e => e.IdType)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_IdType");
            entity.Property(e => e.LegalId)
                .HasMaxLength(100)
                .HasColumnName("SQL_LegalId");
            entity.Property(e => e.LegalStatusCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_LegalStatusCode");
            entity.Property(e => e.LegalStatusDescription)
                .HasMaxLength(100)
                .HasColumnName("SQL_LegalStatusDescription");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_MaritalStatus");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("SQL_MiddleName");
            entity.Property(e => e.MiddleNameEn)
                .HasMaxLength(100)
                .HasColumnName("SQL_MiddleNameEn");
            entity.Property(e => e.MobileNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MobileNumber");
            entity.Property(e => e.Nationality)
                .HasMaxLength(100)
                .HasColumnName("SQL_Nationality");
            entity.Property(e => e.PreferredLanguage)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PreferredLanguage");
            entity.Property(e => e.SamaClass)
                .HasMaxLength(50)
                .HasColumnName("SQL_SamaClass");
            entity.Property(e => e.Sector)
                .HasMaxLength(50)
                .HasColumnName("SQL_Sector");
            entity.Property(e => e.Segment)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_Segment");
            entity.Property(e => e.SegmentDescription)
                .HasMaxLength(100)
                .HasColumnName("SQL_SegmentDescription");
        });

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.ToTable("CreditCard");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.ActualTotalAvailableAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_ActualTotalAvailableAmount");
            entity.Property(e => e.CardClass)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardClass");
            entity.Property(e => e.CardExpiryDate)
                .HasColumnType("date")
                .HasColumnName("SQL_CardExpiryDate");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardNumber");
            entity.Property(e => e.CardProductCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardProductCode");
            entity.Property(e => e.CardProductDesc)
                .HasMaxLength(100)
                .HasColumnName("SQL_CardProductDesc");
            entity.Property(e => e.CardStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardStatus");
            entity.Property(e => e.CardStatusDate)
                .HasColumnType("date")
                .HasColumnName("SQL_CardStatusDate");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardType");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.CmsaccountNumber)
                .HasMaxLength(100)
                .HasColumnName("SQL_CMSAccountNumber");
            entity.Property(e => e.CmsaccountStatus)
                .HasMaxLength(100)
                .HasColumnName("SQL_CMSAccountStatus");
            entity.Property(e => e.CmsreplacementNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_CMSReplacementNumber");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .HasColumnName("SQL_Currency");
            entity.Property(e => e.IssusDate)
                .HasColumnType("date")
                .HasColumnName("SQL_IssusDate");
            entity.Property(e => e.IssusedBranch)
                .HasMaxLength(50)
                .HasColumnName("SQL_IssusedBranch");
            entity.Property(e => e.LinkedAccount)
                .HasMaxLength(50)
                .HasColumnName("SQL_LinkedAccount");
            entity.Property(e => e.MailAddress)
                .HasMaxLength(200)
                .HasColumnName("SQL_MailAddress");
            entity.Property(e => e.MaxPaymentNo)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MaxPaymentNo");
            entity.Property(e => e.MinimumDueAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MinimumDueAmount");
            entity.Property(e => e.NickName)
                .HasMaxLength(100)
                .HasColumnName("SQL_NickName");
            entity.Property(e => e.PrimaryCard)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PrimaryCard");
            entity.Property(e => e.FirstName)
             .HasMaxLength(100)
             .HasColumnName("SQL_FirstName");
            entity.Property(e => e.MiddleName)
               .HasMaxLength(100)
               .HasColumnName("SQL_MiddleName");
            entity.Property(e => e.GrandFatherName)
               .HasMaxLength(100)
               .HasColumnName("SQL_GrandfatherName");
            entity.Property(e => e.LastName)
               .HasMaxLength(100)
               .HasColumnName("SQL_FamilyName");

        });

        modelBuilder.Entity<CustomCreditCard>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.ActualTotalAvailableAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_ActualTotalAvailableAmount");
            entity.Property(e => e.CardClass)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardClass");
            entity.Property(e => e.CardExpiryDate)
                .HasColumnType("date")
                .HasColumnName("SQL_CardExpiryDate");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardNumber");
            entity.Property(e => e.CardProductCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardProductCode");
            entity.Property(e => e.CardProductDesc)
                .HasMaxLength(100)
                .HasColumnName("SQL_CardProductDesc");
            entity.Property(e => e.CardStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardStatus");
            entity.Property(e => e.CardStatusDate)
                .HasColumnType("date")
                .HasColumnName("SQL_CardStatusDate");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardType");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.CmsaccountNumber)
                .HasMaxLength(100)
                .HasColumnName("SQL_CMSAccountNumber");
            entity.Property(e => e.CmsaccountStatus)
                .HasMaxLength(100)
                .HasColumnName("SQL_CMSAccountStatus");
            entity.Property(e => e.CmsreplacementNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_CMSReplacementNumber");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .HasColumnName("SQL_Currency");
            entity.Property(e => e.IssusDate)
                .HasColumnType("date")
                .HasColumnName("SQL_IssusDate");
            entity.Property(e => e.IssusedBranch)
                .HasMaxLength(50)
                .HasColumnName("SQL_IssusedBranch");
            entity.Property(e => e.LinkedAccount)
                .HasMaxLength(50)
                .HasColumnName("SQL_LinkedAccount");
            entity.Property(e => e.MailAddress)
                .HasMaxLength(200)
                .HasColumnName("SQL_MailAddress");
            entity.Property(e => e.MaxPaymentNo)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MaxPaymentNo");
            entity.Property(e => e.MinimumDueAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MinimumDueAmount");
            entity.Property(e => e.NickName)
                .HasMaxLength(100)
                .HasColumnName("SQL_NickName");
            entity.Property(e => e.PrimaryCard)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PrimaryCard");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("SQL_Email");
            entity.Property(e => e.MobileNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MobileNumber");
            entity.Property(e => e.FirstName)
             .HasMaxLength(100)
             .HasColumnName("SQL_FirstName");
            entity.Property(e => e.MiddleName)
               .HasMaxLength(100)
               .HasColumnName("SQL_MiddleName");
            entity.Property(e => e.GrandFatherName)
               .HasMaxLength(100)
               .HasColumnName("SQL_GrandfatherName");
            entity.Property(e => e.LastName)
               .HasMaxLength(100)
               .HasColumnName("SQL_FamilyName");
        });

        modelBuilder.Entity<DebitCard>(entity =>
        {
            entity.ToTable("DebitCard");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.AffiliatedAccountBranch)
                .HasMaxLength(50)
                .HasColumnName("SQL_AffiliatedAccountBranch");
            entity.Property(e => e.AffiliatedAccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_AffiliatedAccountNumber");
            entity.Property(e => e.AtmPinStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_AtmPinStatus");
            entity.Property(e => e.BankAlBiladDailyLimit)
                .HasMaxLength(50)
                .HasColumnName("SQL_BankAlBiladDailyLimit");
            entity.Property(e => e.CardExpiryDate)
                .HasColumnType("date")
                .HasColumnName("SQL_CardExpiryDate");
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(150)
                .HasColumnName("SQL_CardHolderName");
            entity.Property(e => e.CardMailAddress)
                .HasMaxLength(100)
                .HasColumnName("SQL_CardMailAddress");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardNumber");
            entity.Property(e => e.CardProduct)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardProduct");
            entity.Property(e => e.CardProductDesc)
                .HasMaxLength(100)
                .HasColumnName("SQL_CardProductDesc");
            entity.Property(e => e.CardStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardStatus");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardType");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.CmsreplacementNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_CMSReplacementNumber");
            entity.Property(e => e.DeliveryMethod)
                .HasMaxLength(50)
                .HasColumnName("SQL_DeliveryMethod");
            entity.Property(e => e.EcomLimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_EcomLimit");
            entity.Property(e => e.InternationalDailyLimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_InternationalDailyLimit");
            entity.Property(e => e.InternetTransactionStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_InternetTransactionStatus");
            entity.Property(e => e.IssuanceType)
                .HasMaxLength(50)
                .HasColumnName("SQL_IssuanceType");
            entity.Property(e => e.IssueBranchName)
                .HasMaxLength(50)
                .HasColumnName("SQL_IssueBranchName");
            entity.Property(e => e.IssueDate)
                .HasColumnType("date")
                .HasColumnName("SQL_IssueDate");
            entity.Property(e => e.LastPoslimitChangeDate)
                .HasColumnType("date")
                .HasColumnName("SQL_LastPOSLimitChangeDate");
            entity.Property(e => e.LinkedAccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_LinkedAccountNumber");
            entity.Property(e => e.OriginalIssueDate)
                .HasColumnType("date")
                .HasColumnName("SQL_OriginalIssueDate");
            entity.Property(e => e.PrimaryFlag)
                .HasMaxLength(50)
                .HasColumnName("SQL_PrimaryFlag");
            entity.Property(e => e.PurchaseLimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PurchaseLimit");
            entity.Property(e => e.ReIssueDate)
                .HasColumnType("date")
                .HasColumnName("SQL_ReIssueDate");
            entity.Property(e => e.SpandailyLimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_SPANDailyLimit");
            entity.Property(e => e.VisaDebitCard).HasColumnName("SQL_VisaDebitCard");
            entity.Property(e => e.Wdlimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_WDLimit");
            entity.Property(e => e.WhitelistCountry)
                .HasMaxLength(50)
                .HasColumnName("SQL_WhitelistCountry");
            entity.Property(e => e.FirstName)
             .HasMaxLength(100)
             .HasColumnName("SQL_FirstName");
            entity.Property(e => e.MiddleName)
               .HasMaxLength(100)
               .HasColumnName("SQL_MiddleName");
            entity.Property(e => e.GrandFatherName)
               .HasMaxLength(100)
               .HasColumnName("SQL_GrandfatherName");
            entity.Property(e => e.LastName)
               .HasMaxLength(100)
               .HasColumnName("SQL_FamilyName");
        });

        modelBuilder.Entity<CustomDebitCard>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.AffiliatedAccountBranch)
                .HasMaxLength(50)
                .HasColumnName("SQL_AffiliatedAccountBranch");
            entity.Property(e => e.AffiliatedAccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_AffiliatedAccountNumber");
            entity.Property(e => e.AtmPinStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_AtmPinStatus");
            entity.Property(e => e.BankAlBiladDailyLimit)
                .HasMaxLength(50)
                .HasColumnName("SQL_BankAlBiladDailyLimit");
            entity.Property(e => e.CardExpiryDate)
                .HasColumnType("date")
                .HasColumnName("SQL_CardExpiryDate");
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(150)
                .HasColumnName("SQL_CardHolderName");
            entity.Property(e => e.CardMailAddress)
                .HasMaxLength(100)
                .HasColumnName("SQL_CardMailAddress");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardNumber");
            entity.Property(e => e.CardProduct)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardProduct");
            entity.Property(e => e.CardProductDesc)
                .HasMaxLength(100)
                .HasColumnName("SQL_CardProductDesc");
            entity.Property(e => e.CardStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardStatus");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .HasColumnName("SQL_CardType");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.CmsreplacementNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_CMSReplacementNumber");
            entity.Property(e => e.DeliveryMethod)
                .HasMaxLength(50)
                .HasColumnName("SQL_DeliveryMethod");
            entity.Property(e => e.EcomLimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_EcomLimit");
            entity.Property(e => e.InternationalDailyLimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_InternationalDailyLimit");
            entity.Property(e => e.InternetTransactionStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_InternetTransactionStatus");
            entity.Property(e => e.IssuanceType)
                .HasMaxLength(50)
                .HasColumnName("SQL_IssuanceType");
            entity.Property(e => e.IssueBranchName)
                .HasMaxLength(50)
                .HasColumnName("SQL_IssueBranchName");
            entity.Property(e => e.IssueDate)
                .HasColumnType("date")
                .HasColumnName("SQL_IssueDate");
            entity.Property(e => e.LastPoslimitChangeDate)
                .HasColumnType("date")
                .HasColumnName("SQL_LastPOSLimitChangeDate");
            entity.Property(e => e.LinkedAccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_LinkedAccountNumber");
            entity.Property(e => e.OriginalIssueDate)
                .HasColumnType("date")
                .HasColumnName("SQL_OriginalIssueDate");
            entity.Property(e => e.PrimaryFlag)
                .HasMaxLength(50)
                .HasColumnName("SQL_PrimaryFlag");
            entity.Property(e => e.PurchaseLimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PurchaseLimit");
            entity.Property(e => e.ReIssueDate)
                .HasColumnType("date")
                .HasColumnName("SQL_ReIssueDate");
            entity.Property(e => e.SpandailyLimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_SPANDailyLimit");
            entity.Property(e => e.VisaDebitCard).HasColumnName("SQL_VisaDebitCard");
            entity.Property(e => e.Wdlimit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_WDLimit");
            entity.Property(e => e.WhitelistCountry)
                .HasMaxLength(50)
                .HasColumnName("SQL_WhitelistCountry");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("SQL_Email");
            entity.Property(e => e.MobileNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MobileNumber");
            entity.Property(e => e.FirstName)
             .HasMaxLength(100)
             .HasColumnName("SQL_FirstName");
            entity.Property(e => e.MiddleName)
               .HasMaxLength(100)
               .HasColumnName("SQL_MiddleName");
            entity.Property(e => e.GrandFatherName)
               .HasMaxLength(100)
               .HasColumnName("SQL_GrandfatherName");
            entity.Property(e => e.LastName)
               .HasMaxLength(100)
               .HasColumnName("SQL_FamilyName");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("DigitalDocumentDepartment");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("SQL_Code");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("SQL_DepartmentName");
        });

        modelBuilder.Entity<FinancePaymentHistory>(entity =>
        {
            entity.ToTable("FinancePaymentHistory");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.Amount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_Amount");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("SQL_Date");
            entity.Property(e => e.InstallmentNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_InstallmentNumber");
            entity.Property(e => e.LoanId)
                .HasMaxLength(100)
                .HasColumnName("SQL_LoanId");
            entity.Property(e => e.OutstandingAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_OutstandingAmount");
        });

        modelBuilder.Entity<CustomFinancePaymentHistory>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.Amount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_Amount");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("SQL_Date");
            entity.Property(e => e.InstallmentNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_InstallmentNumber");
            entity.Property(e => e.LoanId)
                .HasMaxLength(100)
                .HasColumnName("SQL_LoanId");
            entity.Property(e => e.OutstandingAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_OutstandingAmount");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("SQL_Email");
            entity.Property(e => e.MobileNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MobileNumber");
        });

        modelBuilder.Entity<FinancialAccount>(entity =>
        {
            entity.ToTable("FinancialAccount");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.AccountCode)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_AccountCode");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_AccountNumber");
            entity.Property(e => e.AccountType)
                .HasMaxLength(100)
                .HasColumnName("SQL_AccountType");
            entity.Property(e => e.ArabicName)
                .HasMaxLength(100)
                .HasColumnName("SQL_ArabicName");
            entity.Property(e => e.Branch)
                .HasMaxLength(100)
                .HasColumnName("SQL_Branch");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .HasColumnName("SQL_Currency");
            entity.Property(e => e.EnglishName)
                .HasMaxLength(100)
                .HasColumnName("SQL_EnglishName");
            entity.Property(e => e.Household)
                .HasMaxLength(50)
                .HasColumnName("SQL_Household");
            entity.Property(e => e.Ibannumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_IBANNumber");
            entity.Property(e => e.NickName)
                .HasMaxLength(50)
                .HasColumnName("SQL_NickName");
            entity.Property(e => e.OpenDate)
                .HasColumnType("date")
                .HasColumnName("SQL_OpenDate");
        });


        modelBuilder.Entity<CustomFinancialAccount>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.AccountCode)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_AccountCode");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_AccountNumber");
            entity.Property(e => e.AccountType)
                .HasMaxLength(100)
                .HasColumnName("SQL_AccountType");
            entity.Property(e => e.ArabicName)
                .HasMaxLength(100)
                .HasColumnName("SQL_ArabicName");
            entity.Property(e => e.Branch)
                .HasMaxLength(100)
                .HasColumnName("SQL_Branch");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .HasColumnName("SQL_Currency");
            entity.Property(e => e.EnglishName)
                .HasMaxLength(100)
                .HasColumnName("SQL_EnglishName");
            entity.Property(e => e.Household)
                .HasMaxLength(50)
                .HasColumnName("SQL_Household");
            entity.Property(e => e.Ibannumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_IBANNumber");
            entity.Property(e => e.NickName)
                .HasMaxLength(50)
                .HasColumnName("SQL_NickName");
            entity.Property(e => e.OpenDate)
                .HasColumnType("date")
                .HasColumnName("SQL_OpenDate");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("SQL_Email"); 
            entity.Property(e => e.MobileNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MobileNumber");
            entity.Property(e => e.FirstName)
             .HasMaxLength(100)
             .HasColumnName("SQL_FirstName");
            entity.Property(e => e.MiddleName)
               .HasMaxLength(100)
               .HasColumnName("SQL_MiddleName");
            entity.Property(e => e.GrandFatherName)
               .HasMaxLength(100)
               .HasColumnName("SQL_GrandfatherName");
            entity.Property(e => e.LastName)
               .HasMaxLength(100)
               .HasColumnName("SQL_FamilyName");
        });


        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("Loan");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.BalloonPaymentAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_BalloonPaymentAmount");
            entity.Property(e => e.BalloonPaymentDate)
                .HasColumnType("date")
                .HasColumnName("SQL_BalloonPaymentDate");
            entity.Property(e => e.BranchName)
                .HasMaxLength(100)
                .HasColumnName("SQL_BranchName");
            entity.Property(e => e.CarDeliveryDateGregorian)
                .HasColumnType("date")
                .HasColumnName("SQL_CarDeliveryDateGregorian");
            entity.Property(e => e.CarPrice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CarPrice");
            entity.Property(e => e.ChassisNumber)
                .HasMaxLength(100)
                .HasColumnName("SQL_ChassisNumber");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("SQL_City");
            entity.Property(e => e.CommitmentFeeFlag).HasColumnName("SQL_CommitmentFeeFlag");
            entity.Property(e => e.ConsumerAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_ConsumerAmount");
            entity.Property(e => e.ContractStartDate)
                .HasColumnType("date")
                .HasColumnName("SQL_ContractStartDate");
            entity.Property(e => e.DealerCity)
                .HasMaxLength(100)
                .HasColumnName("SQL_DealerCity");
            entity.Property(e => e.DealerName)
                .HasMaxLength(100)
                .HasColumnName("SQL_DealerName");
            entity.Property(e => e.DelinquencyStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_DelinquencyStatus");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_DepartmentCode");
            entity.Property(e => e.DepartmentCodeText)
                .HasMaxLength(100)
                .HasColumnName("SQL_DepartmentCodeText");
            entity.Property(e => e.DeptClass)
                .HasMaxLength(50)
                .HasColumnName("SQL_DeptClass");
            entity.Property(e => e.DueAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_DueAmount");
            entity.Property(e => e.EmployerCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_EmployerCode");
            entity.Property(e => e.ExchangeRate)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("SQL_ExchangeRate");
            entity.Property(e => e.ExecutionDate)
                .HasColumnType("date")
                .HasColumnName("SQL_ExecutionDate");
            entity.Property(e => e.GlobalDpr)
                .HasMaxLength(100)
                .HasColumnName("SQL_GlobalDPR");
            entity.Property(e => e.GracePeriod)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_GracePeriod");
            entity.Property(e => e.InstallmentAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_InstallmentAmount");
            entity.Property(e => e.InternalStaff)
                .HasMaxLength(50)
                .HasColumnName("SQL_InternalStaff");
            entity.Property(e => e.LastInstallmentBpdate)
                .HasColumnType("date")
                .HasColumnName("SQL_LastInstallmentBPDate");
            entity.Property(e => e.LastInstallmentDate)
                .HasColumnType("date")
                .HasColumnName("SQL_LastInstallmentDate");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("date")
                .HasColumnName("SQL_LastUpdated");
            entity.Property(e => e.LinkedAccountBranch)
                .HasMaxLength(200)
                .HasColumnName("SQL_LinkedAccountBranch");
            entity.Property(e => e.LinkedAccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_LinkedAccountNumber");
            entity.Property(e => e.LoanCategory)
                .HasMaxLength(100)
                .HasColumnName("SQL_LoanCategory");
            entity.Property(e => e.LoanCategoryDescription)
                .HasMaxLength(200)
                .HasColumnName("SQL_LoanCategoryDescription");
            entity.Property(e => e.LoanCurrency)
                .HasMaxLength(50)
                .HasColumnName("SQL_LoanCurrency");
            entity.Property(e => e.LoanId)
                .HasMaxLength(50)
                .HasColumnName("SQL_LoanId");
            entity.Property(e => e.LoanRequestRef)
                .HasMaxLength(50)
                .HasColumnName("SQL_LoanRequestRef");
            entity.Property(e => e.MaturityDate)
                .HasColumnType("date")
                .HasColumnName("SQL_MaturityDate");
            entity.Property(e => e.NextInstallmentIncPd)
                .HasColumnType("date")
                .HasColumnName("SQL_NextInstallmentIncPD");
            entity.Property(e => e.NextPaymentDate)
                .HasColumnType("date")
                .HasColumnName("SQL_NextPaymentDate");
            entity.Property(e => e.OutstandingAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_OutstandingAmount");
            entity.Property(e => e.OverdueAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_OverdueAmount");
            entity.Property(e => e.OverdueAmountBase)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_OverdueAmountBase");
            entity.Property(e => e.OverdueDate)
                .HasMaxLength(50)
                .HasColumnName("SQL_OverdueDate");
            entity.Property(e => e.PastDueNumberOfdays)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PastDueNumberOFDays");
            entity.Property(e => e.PrincipalAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PrincipalAmount");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_ProductCode");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(50)
                .HasColumnName("SQL_ProductDescription");
            entity.Property(e => e.ProductDpr)
                .HasMaxLength(100)
                .HasColumnName("SQL_ProductDPR");
            entity.Property(e => e.ProfitAnnualRate)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("SQL_ProfitAnnualRate");
            entity.Property(e => e.ProfitFlatRate)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("SQL_ProfitFlatRate");
            entity.Property(e => e.RemainingInstallments)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_RemainingInstallments");
            entity.Property(e => e.RemainingProfitAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_RemainingProfitAmount");
            entity.Property(e => e.SalaryAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_SalaryAmount");
            entity.Property(e => e.TotalLdpaidAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_TotalLDPaidAmount");
            entity.Property(e => e.TotalNumberOfInstallments)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_TotalNumberOfInstallments");
            entity.Property(e => e.FirstName)
               .HasMaxLength(100)
               .HasColumnName("SQL_FirstName");
            entity.Property(e => e.MiddleName)
               .HasMaxLength(100)
               .HasColumnName("SQL_MiddleName");
            entity.Property(e => e.GrandFatherName)
               .HasMaxLength(100)
               .HasColumnName("SQL_GrandfatherName");
            entity.Property(e => e.LastName)
               .HasMaxLength(100)
               .HasColumnName("SQL_FamilyName");
        });

        modelBuilder.Entity<CustomLoan>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BatchDate).HasColumnType("date");
            entity.Property(e => e.BalloonPaymentAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_BalloonPaymentAmount");
            entity.Property(e => e.BalloonPaymentDate)
                .HasColumnType("date")
                .HasColumnName("SQL_BalloonPaymentDate");
            entity.Property(e => e.BranchName)
                .HasMaxLength(100)
                .HasColumnName("SQL_BranchName");
            entity.Property(e => e.CarDeliveryDateGregorian)
                .HasColumnType("date")
                .HasColumnName("SQL_CarDeliveryDateGregorian");
            entity.Property(e => e.CarPrice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CarPrice");
            entity.Property(e => e.ChassisNumber)
                .HasMaxLength(100)
                .HasColumnName("SQL_ChassisNumber");
            entity.Property(e => e.Cif)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_CIF");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("SQL_City");
            entity.Property(e => e.CommitmentFeeFlag).HasColumnName("SQL_CommitmentFeeFlag");
            entity.Property(e => e.ConsumerAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_ConsumerAmount");
            entity.Property(e => e.ContractStartDate)
                .HasColumnType("date")
                .HasColumnName("SQL_ContractStartDate");
            entity.Property(e => e.DealerCity)
                .HasMaxLength(100)
                .HasColumnName("SQL_DealerCity");
            entity.Property(e => e.DealerName)
                .HasMaxLength(100)
                .HasColumnName("SQL_DealerName");
            entity.Property(e => e.DelinquencyStatus)
                .HasMaxLength(50)
                .HasColumnName("SQL_DelinquencyStatus");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_DepartmentCode");
            entity.Property(e => e.DepartmentCodeText)
                .HasMaxLength(100)
                .HasColumnName("SQL_DepartmentCodeText");
            entity.Property(e => e.DeptClass)
                .HasMaxLength(50)
                .HasColumnName("SQL_DeptClass");
            entity.Property(e => e.DueAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_DueAmount");
            entity.Property(e => e.EmployerCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_EmployerCode");
            entity.Property(e => e.ExchangeRate)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("SQL_ExchangeRate");
            entity.Property(e => e.ExecutionDate)
                .HasColumnType("date")
                .HasColumnName("SQL_ExecutionDate");
            entity.Property(e => e.GlobalDpr)
                .HasMaxLength(100)
                .HasColumnName("SQL_GlobalDPR");
            entity.Property(e => e.GracePeriod)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_GracePeriod");
            entity.Property(e => e.InstallmentAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_InstallmentAmount");
            entity.Property(e => e.InternalStaff)
                .HasMaxLength(50)
                .HasColumnName("SQL_InternalStaff");
            entity.Property(e => e.LastInstallmentBpdate)
                .HasColumnType("date")
                .HasColumnName("SQL_LastInstallmentBPDate");
            entity.Property(e => e.LastInstallmentDate)
                .HasColumnType("date")
                .HasColumnName("SQL_LastInstallmentDate");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("date")
                .HasColumnName("SQL_LastUpdated");
            entity.Property(e => e.LinkedAccountBranch)
                .HasMaxLength(200)
                .HasColumnName("SQL_LinkedAccountBranch");
            entity.Property(e => e.LinkedAccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SQL_LinkedAccountNumber");
            entity.Property(e => e.LoanCategory)
                .HasMaxLength(100)
                .HasColumnName("SQL_LoanCategory");
            entity.Property(e => e.LoanCategoryDescription)
                .HasMaxLength(200)
                .HasColumnName("SQL_LoanCategoryDescription");
            entity.Property(e => e.LoanCurrency)
                .HasMaxLength(50)
                .HasColumnName("SQL_LoanCurrency");
            entity.Property(e => e.LoanId)
                .HasMaxLength(50)
                .HasColumnName("SQL_LoanId");
            entity.Property(e => e.LoanRequestRef)
                .HasMaxLength(50)
                .HasColumnName("SQL_LoanRequestRef");
            entity.Property(e => e.MaturityDate)
                .HasColumnType("date")
                .HasColumnName("SQL_MaturityDate");
            entity.Property(e => e.NextInstallmentIncPd)
                .HasColumnType("date")
                .HasColumnName("SQL_NextInstallmentIncPD");
            entity.Property(e => e.NextPaymentDate)
                .HasColumnType("date")
                .HasColumnName("SQL_NextPaymentDate");
            entity.Property(e => e.OutstandingAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_OutstandingAmount");
            entity.Property(e => e.OverdueAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_OverdueAmount");
            entity.Property(e => e.OverdueAmountBase)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_OverdueAmountBase");
            entity.Property(e => e.OverdueDate)
                .HasMaxLength(50)
                .HasColumnName("SQL_OverdueDate");
            entity.Property(e => e.PastDueNumberOfdays)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PastDueNumberOFDays");
            entity.Property(e => e.PrincipalAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_PrincipalAmount");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .HasColumnName("SQL_ProductCode");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(50)
                .HasColumnName("SQL_ProductDescription");
            entity.Property(e => e.ProductDpr)
                .HasMaxLength(100)
                .HasColumnName("SQL_ProductDPR");
            entity.Property(e => e.ProfitAnnualRate)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("SQL_ProfitAnnualRate");
            entity.Property(e => e.ProfitFlatRate)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("SQL_ProfitFlatRate");
            entity.Property(e => e.RemainingInstallments)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_RemainingInstallments");
            entity.Property(e => e.RemainingProfitAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_RemainingProfitAmount");
            entity.Property(e => e.SalaryAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_SalaryAmount");
            entity.Property(e => e.TotalLdpaidAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_TotalLDPaidAmount");
            entity.Property(e => e.TotalNumberOfInstallments)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_TotalNumberOfInstallments");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("SQL_Email");
            entity.Property(e => e.MobileNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SQL_MobileNumber");
            entity.Property(e => e.FirstName)
              .HasMaxLength(100)
              .HasColumnName("SQL_FirstName");
            entity.Property(e => e.MiddleName)
               .HasMaxLength(100)
               .HasColumnName("SQL_MiddleName");
            entity.Property(e => e.GrandFatherName)
               .HasMaxLength(100)
               .HasColumnName("SQL_GrandfatherName");
            entity.Property(e => e.LastName)
               .HasMaxLength(100)
               .HasColumnName("SQL_FamilyName");
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.ToTable("DigitalDocumentSector");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("SQL_Code");
            entity.Property(e => e.SectorName)
                .HasMaxLength(100)
                .HasColumnName("SQL_SectorName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
