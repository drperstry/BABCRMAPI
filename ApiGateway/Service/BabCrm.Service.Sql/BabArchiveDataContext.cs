using BabCrm.Service.ArchiveDataModels;
using Microsoft.EntityFrameworkCore;

namespace BabCrm.Service.Sql;

public partial class BabArchiveDataContext : DbContext
{
    public BabArchiveDataContext()
    {
    }

    public BabArchiveDataContext(DbContextOptions<BabArchiveDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountActivation> AccountActivations { get; set; }

    public virtual DbSet<BeneficiaryActivation> BeneficiaryActivations { get; set; }

    public virtual DbSet<BrUbInternationalBank> BrUbInternationalBanks { get; set; }

    public virtual DbSet<BrUbWu> BrUbWus { get; set; }

    public virtual DbSet<CardPaymentTransferFromAcc> CardPaymentTransferFromAccs { get; set; }

    public virtual DbSet<CardRequestLimitMaintenance> CardRequestLimitMaintenances { get; set; }

    public virtual DbSet<CardRequestRenewal> CardRequestRenewals { get; set; }

    public virtual DbSet<CardRequestReplacement> CardRequestReplacements { get; set; }

    public virtual DbSet<CardRequestStopCard> CardRequestStopCards { get; set; }

    public virtual DbSet<CardRequestWhitelist> CardRequestWhitelists { get; set; }

    public virtual DbSet<CaseProcessHistory> CaseProcessHistories { get; set; }

    public virtual DbSet<CaseRequest> CaseRequests { get; set; }

    public virtual DbSet<ChannelsIvrIvrPinBlocking> ChannelsIvrIvrPinBlockings { get; set; }

    public virtual DbSet<ChannelsIvrUnblocIvrRegist> ChannelsIvrUnblocIvrRegists { get; set; }

    public virtual DbSet<ChannelsReqUnblockAuthent> ChannelsReqUnblockAuthents { get; set; }

    public virtual DbSet<ChequeBookRequest> ChequeBookRequests { get; set; }

    public virtual DbSet<CrAtmPinUnblockedAtmPin> CrAtmPinUnblockedAtmPins { get; set; }

    public virtual DbSet<CrCashAdvaTransferToAcc> CrCashAdvaTransferToAccs { get; set; }

    public virtual DbSet<CrLmChangePosDailyLimit> CrLmChangePosDailyLimits { get; set; }

    public virtual DbSet<CrmProduct> CrmProducts { get; set; }

    public virtual DbSet<CrmRequestCategory> CrmRequestCategories { get; set; }

    public virtual DbSet<CrmRequestStatus> CrmRequestStatuses { get; set; }

    public virtual DbSet<CrmRequestSubType> CrmRequestSubTypes { get; set; }

    public virtual DbSet<CrmRequestType> CrmRequestTypes { get; set; }

    public virtual DbSet<CsrBeneficiaryMaintenance> CsrBeneficiaryMaintenances { get; set; }

    public virtual DbSet<DeleteBeneficiary> DeleteBeneficiaries { get; set; }

    public virtual DbSet<EnjazReqMfaBlockedRemoval> EnjazReqMfaBlockedRemovals { get; set; }

    public virtual DbSet<EnjazRequest> EnjazRequests { get; set; }

    public virtual DbSet<EnjazRequestSmsNotification> EnjazRequestSmsNotifications { get; set; }

    public virtual DbSet<FtrBankAlBilad> FtrBankAlBilads { get; set; }

    public virtual DbSet<FtrInternationalBank> FtrInternationalBanks { get; set; }

    public virtual DbSet<FtrLocalBank> FtrLocalBanks { get; set; }

    public virtual DbSet<FtrOwnAccount> FtrOwnAccounts { get; set; }

    public virtual DbSet<FtrWuTfEeC> FtrWuTfEeCs { get; set; }

    public virtual DbSet<FundTransferRequest> FundTransferRequests { get; set; }

    public virtual DbSet<InternetPurchaseStatChange> InternetPurchaseStatChanges { get; set; }

    public virtual DbSet<IpoProcessingIpoSubscr> IpoProcessingIpoSubscrs { get; set; }

    public virtual DbSet<IpoRequest> IpoRequests { get; set; }

    public virtual DbSet<OnlineAccountActivation> OnlineAccountActivations { get; set; }

    public virtual DbSet<OtherRequest> OtherRequests { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<RetailMfaMfaBlockedRemoval> RetailMfaMfaBlockedRemovals { get; set; }

    public virtual DbSet<RetailRequest> RetailRequests { get; set; }

    public virtual DbSet<RetailRequestSmsNotific> RetailRequestSmsNotifics { get; set; }

    public virtual DbSet<SadadReqAddFavoriteBill> SadadReqAddFavoriteBills { get; set; }

    public virtual DbSet<SadadReqDeleteFavoriteBill> SadadReqDeleteFavoriteBills { get; set; }

    public virtual DbSet<SadadReqMoiPaymentRefund> SadadReqMoiPaymentRefunds { get; set; }

    public virtual DbSet<SadadReqUpdateFavoriteBill> SadadReqUpdateFavoriteBills { get; set; }

    public virtual DbSet<SadadRequestBillPayment> SadadRequestBillPayments { get; set; }

    public virtual DbSet<StopCheque> StopCheques { get; set; }

    public virtual DbSet<TokenRequestActivating> TokenRequestActivatings { get; set; }

    public virtual DbSet<TokenRequestCancelation> TokenRequestCancelations { get; set; }

    public virtual DbSet<TokenRequestRegistration> TokenRequestRegistrations { get; set; }

    public virtual DbSet<TokenRequestResendTknLink> TokenRequestResendTknLinks { get; set; }

    public virtual DbSet<TransFastEnjazEasyCharity> TransFastEnjazEasyCharities { get; set; }

    public virtual DbSet<UpdateBeneficiaryLocalBank> UpdateBeneficiaryLocalBanks { get; set; }

    public virtual DbSet<UpdateIdExpiryDate> UpdateIdExpiryDates { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountActivation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Account_Activation");

            entity.Property(e => e.ActiveAccount)
                .HasMaxLength(50)
                .HasColumnName("Active Account #");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<BeneficiaryActivation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Beneficiary_Activation");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.Cheque).HasColumnType("numeric(10, 0)");
            entity.Property(e => e.ChequeBookQuantity)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("Cheque Book Quantity");
            entity.Property(e => e.ChequeBookSize)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("Cheque Book Size");
            entity.Property(e => e.ChequeBookType)
                .HasMaxLength(30)
                .HasColumnName("Cheque Book Type");
            entity.Property(e => e.ChequeDate)
                .HasColumnType("datetime")
                .HasColumnName("Cheque Date");
            entity.Property(e => e.ChequeType)
                .HasMaxLength(30)
                .HasColumnName("Cheque Type");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Description1)
                .HasMaxLength(255)
                .HasColumnName("Description_");
            entity.Property(e => e.DisclaimerAgreed)
                .HasMaxLength(255)
                .HasColumnName("Disclaimer Agreed");
            entity.Property(e => e.FromNumber)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("From Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.PaidTo)
                .HasMaxLength(100)
                .HasColumnName("Paid To");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.ReceivingBranch)
                .HasMaxLength(100)
                .HasColumnName("Receiving Branch");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SmsDisclaimer)
                .HasMaxLength(1)
                .HasColumnName("SMS Disclaimer");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.StopReason)
                .HasMaxLength(40)
                .HasColumnName("Stop Reason");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.ToNumber)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("To Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<BrUbInternationalBank>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BR_UB_International_Bank");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.BeneficiaryDescription)
                .HasMaxLength(100)
                .HasColumnName("Beneficiary Description");
            entity.Property(e => e.BeneficiaryNickname)
                .HasMaxLength(100)
                .HasColumnName("Beneficiary Nickname");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("Phone Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.Purpose).HasMaxLength(100);
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<BrUbWu>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BR_UB_WU");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.BeneficiaryDescription)
                .HasMaxLength(100)
                .HasColumnName("Beneficiary Description");
            entity.Property(e => e.BeneficiaryNickname)
                .HasMaxLength(100)
                .HasColumnName("Beneficiary Nickname");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("Phone Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.Purpose).HasMaxLength(100);
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CardPaymentTransferFromAcc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Card_Payment_Transfer_From_Acc");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Amount).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .HasColumnName("Card Number");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CardRequestLimitMaintenance>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Card_Request_Limit_Maintenance");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .HasColumnName("Card Number");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UndateLimit)
                .HasMaxLength(30)
                .HasColumnName("Undate Limit");
            entity.Property(e => e.UpdateExpiry)
                .HasMaxLength(50)
                .HasColumnName("Update Expiry");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .HasColumnName("Update Type");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CardRequestRenewal>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Card_Request_Renewal");

            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CreditCardNumber)
                .HasMaxLength(30)
                .HasColumnName("Credit Card Number");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.DeliveryMethod)
                .HasMaxLength(30)
                .HasColumnName("Delivery Method");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.ReceivingBranch)
                .HasMaxLength(100)
                .HasColumnName("Receiving Branch");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CardRequestReplacement>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Card_Request_Replacement");

            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CreditCardNumber)
                .HasMaxLength(30)
                .HasColumnName("Credit Card Number");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CardRequestStopCard>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Card_Request_Stop_Card");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .HasColumnName("Card Number");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CardRequestWhitelist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Card_Request_Whitelist");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.AllCards)
                .HasMaxLength(1)
                .HasColumnName("All Cards");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.DebitCardNumber)
                .HasMaxLength(30)
                .HasColumnName("Debit Card Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.ResponsibilityDisclaimer)
                .HasMaxLength(1)
                .HasColumnName("Responsibility Disclaimer");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CaseProcessHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CASE_PROCESS_HISTORY");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Branch).HasMaxLength(30);
            entity.Property(e => e.CaseType)
                .HasMaxLength(30)
                .HasColumnName("Case Type");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(350);
            entity.Property(e => e.FinalReplyStatment)
                .HasMaxLength(100)
                .HasColumnName("Final Reply Statment");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First Name");
            entity.Property(e => e.IsEligibleOfficialLetter)
                .HasMaxLength(1)
                .HasColumnName("Is Eligible Official Letter");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last Name");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(100)
                .HasColumnName("Mobile Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.Priority).HasMaxLength(30);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.ReveiveMethod)
                .HasMaxLength(30)
                .HasColumnName("Reveive Method");
            entity.Property(e => e.Source).HasMaxLength(30);
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubType)
                .HasMaxLength(100)
                .HasColumnName("Sub Type");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TransactionAuthorizationCode)
                .HasMaxLength(100)
                .HasColumnName("Transaction Authorization Code");
            entity.Property(e => e.TransactionCardType)
                .HasMaxLength(50)
                .HasColumnName("Transaction Card Type");
            entity.Property(e => e.TransactionDate)
                .HasMaxLength(30)
                .HasColumnName("Transaction Date");
            entity.Property(e => e.TransactionReference)
                .HasMaxLength(30)
                .HasColumnName("Transaction Reference");
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CaseRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Case_Request");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Branch).HasMaxLength(30);
            entity.Property(e => e.CaseType)
                .HasMaxLength(30)
                .HasColumnName("Case Type");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(350);
            entity.Property(e => e.FinalReplyStatment)
                .HasMaxLength(100)
                .HasColumnName("Final Reply Statment");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First Name");
            entity.Property(e => e.IsEligibleOfficialLetter)
                .HasMaxLength(1)
                .HasColumnName("Is Eligible Official Letter");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last Name");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(100)
                .HasColumnName("Mobile Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.Priority).HasMaxLength(30);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.ReveiveMethod)
                .HasMaxLength(30)
                .HasColumnName("Reveive Method");
            entity.Property(e => e.Source).HasMaxLength(30);
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubType)
                .HasMaxLength(100)
                .HasColumnName("Sub Type");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TransactionAuthorizationCode)
                .HasMaxLength(100)
                .HasColumnName("Transaction Authorization Code");
            entity.Property(e => e.TransactionCardType)
                .HasMaxLength(50)
                .HasColumnName("Transaction Card Type");
            entity.Property(e => e.TransactionDate)
                .HasMaxLength(30)
                .HasColumnName("Transaction Date");
            entity.Property(e => e.TransactionReference)
                .HasMaxLength(30)
                .HasColumnName("Transaction Reference");
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");

            entity.HasOne(d => d.CrmCategory).WithMany()
                .HasForeignKey(d => d.CrmCategoryId)
                .HasConstraintName("FK_Case_Request_Category");

            entity.HasOne(d => d.CrmProduct).WithMany()
                .HasForeignKey(d => d.CrmProductId)
                .HasConstraintName("FK_Case_Request_CRM_Product");

            entity.HasOne(d => d.CrmStatus).WithMany()
                .HasForeignKey(d => d.CrmStatusId)
                .HasConstraintName("FK_Case_Request_CRM_Request_Status");

            entity.HasOne(d => d.CrmSubType).WithMany()
                .HasForeignKey(d => d.CrmSubTypeId)
                .HasConstraintName("FK_Case_Request_Request_Sub_Type");

            entity.HasOne(d => d.CrmType).WithMany()
                .HasForeignKey(d => d.CrmTypeId)
                .HasConstraintName("FK_Case_Request_Request_Type");
        });

        modelBuilder.Entity<ChannelsIvrIvrPinBlocking>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Channels_IVR_IVR_PIN_Blocking");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.IvrUserId)
                .HasMaxLength(50)
                .HasColumnName("IVR User ID");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
            entity.Property(e => e.VerballyAuthenticated)
                .HasMaxLength(1)
                .HasColumnName("Verbally Authenticated");
        });

        modelBuilder.Entity<ChannelsIvrUnblocIvrRegist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Channels_IVR_Unbloc_IVR_regist");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.IvrUserId)
                .HasMaxLength(50)
                .HasColumnName("IVR User ID");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
            entity.Property(e => e.VerballyAuthenticated)
                .HasMaxLength(1)
                .HasColumnName("Verbally Authenticated");
        });

        modelBuilder.Entity<ChannelsReqUnblockAuthent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Channels_Req_Unblock_Authent");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .HasColumnName("Account Number");
            entity.Property(e => e.ActiveAccount)
                .HasMaxLength(50)
                .HasColumnName("Active Account #");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<ChequeBookRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Cheque_Book_Request");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.ActCloseDt)
                .HasColumnType("datetime")
                .HasColumnName("ACT_CLOSE_DT");
            entity.Property(e => e.AliasName)
                .HasMaxLength(50)
                .HasColumnName("ALIAS_NAME");
            entity.Property(e => e.Attrib26)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_26");
            entity.Property(e => e.Attrib28)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_28");
            entity.Property(e => e.Attrib31)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_31");
            entity.Property(e => e.BpServiceDesc)
                .HasMaxLength(100)
                .HasColumnName("BP_SERVICE_DESC");
            entity.Property(e => e.ChCheqName2)
                .HasMaxLength(100)
                .HasColumnName("CH_CHEQ_NAME2");
            entity.Property(e => e.ChequeBookQuantity)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("Cheque Book Quantity");
            entity.Property(e => e.ChequeBookSize)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("Cheque Book Size");
            entity.Property(e => e.ChequeBookType)
                .HasMaxLength(30)
                .HasColumnName("Cheque Book Type");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.Created)
                .HasColumnType("datetime")
                .HasColumnName("CREATED");
            entity.Property(e => e.Csn)
                .HasMaxLength(50)
                .HasColumnName("CSN");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Investmentaccountnum)
                .HasMaxLength(100)
                .HasColumnName("INVESTMENTACCOUNTNUM");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("LOGIN");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.ReceivingBranch)
                .HasMaxLength(100)
                .HasColumnName("Receiving Branch");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrArea)
                .HasMaxLength(30)
                .HasColumnName("SR_AREA");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatId)
                .HasMaxLength(30)
                .HasColumnName("SR_STAT_ID");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SrSubArea)
                .HasMaxLength(30)
                .HasColumnName("SR_SUB_AREA");
            entity.Property(e => e.SrTitle)
                .HasMaxLength(100)
                .HasColumnName("SR_TITLE");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.Transactiondesc)
                .HasMaxLength(100)
                .HasColumnName("TRANSACTIONDESC");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CrAtmPinUnblockedAtmPin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CR_ATM_Pin_Unblocked_ATM_Pin");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .HasColumnName("Card Number");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CrCashAdvaTransferToAcc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CR_Cash_Adva_Transfer_To_Acc");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Amount).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .HasColumnName("Card Number");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TotalCash)
                .HasMaxLength(50)
                .HasColumnName("Total Cash");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CrLmChangePosDailyLimit>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CR_LM_Change_POS_Daily_Limit");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.AgreeOnChanges)
                .HasMaxLength(1)
                .HasColumnName("Agree On Changes");
            entity.Property(e => e.AgreeOnDisclaimer)
                .HasMaxLength(30)
                .HasColumnName("Agree On Disclaimer");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .HasColumnName("Card Number");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UndateLimit)
                .HasMaxLength(100)
                .HasColumnName("Undate Limit");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<CrmProduct>(entity =>
        {
            entity.ToTable("CRM_Product");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.InternalArabicName).HasMaxLength(100);
            entity.Property(e => e.InternalEnglishName).HasMaxLength(100);
            entity.Property(e => e.SamaArabicName).HasMaxLength(100);
            entity.Property(e => e.SamaEnglishName).HasMaxLength(100);
        });

        modelBuilder.Entity<CrmRequestCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Category");

            entity.ToTable("CRM_Request_Category");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ArabicName).HasMaxLength(100);
            entity.Property(e => e.EnglishName).HasMaxLength(100);
        });

        modelBuilder.Entity<CrmRequestStatus>(entity =>
        {
            entity.ToTable("CRM_Request_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ArabicName).HasMaxLength(100);
            entity.Property(e => e.EnglishName).HasMaxLength(100);
        });

        modelBuilder.Entity<CrmRequestSubType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Request_Sub_Type");

            entity.ToTable("CRM_Request_Sub_Type");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.InternalArabicName).HasMaxLength(100);
            entity.Property(e => e.InternalEnglishName).HasMaxLength(100);
            entity.Property(e => e.SamaArabicName).HasMaxLength(100);
            entity.Property(e => e.SamaEnglishName).HasMaxLength(100);
        });

        modelBuilder.Entity<CrmRequestType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Request_Type");

            entity.ToTable("CRM_Request_Type");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.InternalArabicName).HasMaxLength(100);
            entity.Property(e => e.InternalEnglishName).HasMaxLength(100);
            entity.Property(e => e.SamaArabicName).HasMaxLength(100);
            entity.Property(e => e.SamaEnglishName).HasMaxLength(100);
        });

        modelBuilder.Entity<CsrBeneficiaryMaintenance>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CSR_Beneficiary_Maintenance");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<DeleteBeneficiary>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Delete_Beneficiary");

            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<EnjazReqMfaBlockedRemoval>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Enjaz_Req_MFA_Blocked_Removal");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
            entity.Property(e => e.VerballyAuthenticated)
                .HasMaxLength(1)
                .HasColumnName("Verbally Authenticated");
        });

        modelBuilder.Entity<EnjazRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Enjaz_Request");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<EnjazRequestSmsNotification>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Enjaz_Request_SMS_Notification");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.NotificationLanguage)
                .HasMaxLength(30)
                .HasColumnName("Notification Language");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<FtrBankAlBilad>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FTR_Bank_AlBilad");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Amount).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.Beneficiary).HasMaxLength(25);
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.ToAccountNumber)
                .HasMaxLength(25)
                .HasColumnName("To Account Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<FtrInternationalBank>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FTR_International_Bank");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Amount).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.Beneficiary).HasMaxLength(25);
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.ToAccountNumber)
                .HasMaxLength(25)
                .HasColumnName("To Account Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<FtrLocalBank>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FTR_Local_Bank");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Amount).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.Beneficiary).HasMaxLength(25);
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.ToAccountNumber)
                .HasMaxLength(25)
                .HasColumnName("To Account Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<FtrOwnAccount>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FTR_OWN_ACCOUNT");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.ActiveAccount)
                .HasMaxLength(50)
                .HasColumnName("Active Account #");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Amount).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.ToAccountNumber)
                .HasMaxLength(25)
                .HasColumnName("To Account Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<FtrWuTfEeC>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FTR_WU_TF_EE_C");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Amount).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.Beneficiary).HasMaxLength(25);
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.ToAccountNumber)
                .HasMaxLength(25)
                .HasColumnName("To Account Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<FundTransferRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Fund_Transfer_Request");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Amount).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.Beneficiary).HasMaxLength(25);
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.ToAccountNumber)
                .HasMaxLength(25)
                .HasColumnName("To Account Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<InternetPurchaseStatChange>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Internet_Purchase_Stat_Change");

            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CreditCardNumber)
                .HasMaxLength(30)
                .HasColumnName("Credit Card Number");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.DebitCardNumber)
                .HasMaxLength(30)
                .HasColumnName("Debit Card Number");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<IpoProcessingIpoSubscr>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IPO_Processing_IPO_Subscr");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .HasColumnName("Account Number");
            entity.Property(e => e.ActiveIpoName)
                .HasMaxLength(100)
                .HasColumnName("Active IPO Name");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.DependentId)
                .HasMaxLength(15)
                .HasColumnName("Dependent Id");
            entity.Property(e => e.LegalMessageConfirmation)
                .HasMaxLength(1)
                .HasColumnName("Legal Message Confirmation");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SharePerSubscriber)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("Share Per Subscriber");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<IpoRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IPO_Request");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<OnlineAccountActivation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ONLINE_ACCOUNT_ACTIVATION");

            entity.Property(e => e.ActCloseDt)
                .HasColumnType("datetime")
                .HasColumnName("ACT_CLOSE_DT");
            entity.Property(e => e.AliasName)
                .HasMaxLength(50)
                .HasColumnName("ALIAS_NAME");
            entity.Property(e => e.Attrib26)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_26");
            entity.Property(e => e.Attrib28)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_28");
            entity.Property(e => e.Attrib31)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_31");
            entity.Property(e => e.BpServiceDesc)
                .HasMaxLength(100)
                .HasColumnName("BP_SERVICE_DESC");
            entity.Property(e => e.ChCheqName2)
                .HasMaxLength(100)
                .HasColumnName("CH_CHEQ_NAME2");
            entity.Property(e => e.Created)
                .HasColumnType("datetime")
                .HasColumnName("CREATED");
            entity.Property(e => e.Csn)
                .HasMaxLength(50)
                .HasColumnName("CSN");
            entity.Property(e => e.FtFrAccntNum)
                .HasMaxLength(50)
                .HasColumnName("FT_FR_ACCNT_NUM");
            entity.Property(e => e.Investmentaccountnum)
                .HasMaxLength(100)
                .HasColumnName("INVESTMENTACCOUNTNUM");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("LOGIN");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME");
            entity.Property(e => e.SrArea)
                .HasMaxLength(30)
                .HasColumnName("SR_AREA");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatId)
                .HasMaxLength(30)
                .HasColumnName("SR_STAT_ID");
            entity.Property(e => e.SrSubArea)
                .HasMaxLength(30)
                .HasColumnName("SR_SUB_AREA");
            entity.Property(e => e.SrTitle)
                .HasMaxLength(100)
                .HasColumnName("SR_TITLE");
            entity.Property(e => e.Transactiondesc)
                .HasMaxLength(100)
                .HasColumnName("TRANSACTIONDESC");
        });

        modelBuilder.Entity<OtherRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Other_Request");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.NotificationLanguage)
                .HasMaxLength(30)
                .HasColumnName("Notification Language");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .HasColumnName("Phone Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.Product).HasMaxLength(100);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubType)
                .HasMaxLength(100)
                .HasColumnName("Sub Type");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.Summery).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Request_Status");

            entity.Property(e => e.ArabicName).HasMaxLength(100);
            entity.Property(e => e.EnglishName).HasMaxLength(100);
        });

        modelBuilder.Entity<RetailMfaMfaBlockedRemoval>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Retail_MFA_MFA_Blocked_Removal");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
            entity.Property(e => e.VerballyAuthenticated)
                .HasMaxLength(1)
                .HasColumnName("Verbally Authenticated");
        });

        modelBuilder.Entity<RetailRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Retail_Request");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.PreferredLanguage)
                .HasMaxLength(30)
                .HasColumnName("Preferred Language");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<RetailRequestSmsNotific>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Retail_Request_SMS_Notific");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.PreferredLanguage)
                .HasMaxLength(30)
                .HasColumnName("Preferred Language");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<SadadReqAddFavoriteBill>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Sadad_Req_Add_Favorite_Bill");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(50)
                .HasColumnName("Bill Number");
            entity.Property(e => e.BillingAccount)
                .HasMaxLength(50)
                .HasColumnName("Billing Account");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MoiBillerNumber)
                .HasMaxLength(100)
                .HasColumnName("MOI Biller Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.Payment).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.PaymentRefNumber)
                .HasMaxLength(50)
                .HasColumnName("Payment Ref Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TransactionRefNumber)
                .HasMaxLength(50)
                .HasColumnName("Transaction Ref Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<SadadReqDeleteFavoriteBill>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Sadad_Req_Delete_Favorite_Bill");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(50)
                .HasColumnName("Bill Number");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<SadadReqMoiPaymentRefund>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Sadad_Req_MOI_Payment_Refund");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(50)
                .HasColumnName("Bill Number");
            entity.Property(e => e.BillingAccount)
                .HasMaxLength(50)
                .HasColumnName("Billing Account");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MoiBillerNumber)
                .HasMaxLength(100)
                .HasColumnName("MOI Biller Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.Payment).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.PaymentRefNumber)
                .HasMaxLength(30)
                .HasColumnName("Payment Ref Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.ServiceCode)
                .HasMaxLength(30)
                .HasColumnName("Service Code");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<SadadReqUpdateFavoriteBill>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Sadad_Req_Update_Favorite_Bill");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(50)
                .HasColumnName("Bill Number");
            entity.Property(e => e.BillingAccount)
                .HasMaxLength(50)
                .HasColumnName("Billing Account");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MoiBillerNumber)
                .HasMaxLength(100)
                .HasColumnName("MOI Biller Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.Payment).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("Phone Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<SadadRequestBillPayment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Sadad_Request_Bill_Payment");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(50)
                .HasColumnName("Bill Number");
            entity.Property(e => e.BillingAccount)
                .HasMaxLength(50)
                .HasColumnName("Billing Account");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MoiBillerNumber)
                .HasMaxLength(100)
                .HasColumnName("MOI Biller Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.Payment).HasColumnType("numeric(22, 7)");
            entity.Property(e => e.PaymentRefNumber)
                .HasMaxLength(50)
                .HasColumnName("Payment Ref Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TransactionRefNumber)
                .HasMaxLength(50)
                .HasColumnName("Transaction Ref Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<StopCheque>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Stop_Cheque");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("Account Number");
            entity.Property(e => e.Cheque).HasColumnType("numeric(10, 0)");
            entity.Property(e => e.ChequeBookQuantity)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("Cheque Book Quantity");
            entity.Property(e => e.ChequeBookSize)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("Cheque Book Size");
            entity.Property(e => e.ChequeBookType)
                .HasMaxLength(30)
                .HasColumnName("Cheque Book Type");
            entity.Property(e => e.ChequeDate)
                .HasColumnType("datetime")
                .HasColumnName("Cheque Date");
            entity.Property(e => e.ChequeType)
                .HasMaxLength(30)
                .HasColumnName("Cheque Type");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Description1)
                .HasMaxLength(255)
                .HasColumnName("Description_");
            entity.Property(e => e.DisclaimerAgreed)
                .HasMaxLength(255)
                .HasColumnName("Disclaimer Agreed");
            entity.Property(e => e.FromNumber)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("From Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.PaidTo)
                .HasMaxLength(100)
                .HasColumnName("Paid To");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.ReceivingBranch)
                .HasMaxLength(100)
                .HasColumnName("Receiving Branch");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SmsDisclaimer)
                .HasMaxLength(1)
                .HasColumnName("SMS Disclaimer");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.StopReason)
                .HasMaxLength(40)
                .HasColumnName("Stop Reason");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.ToNumber)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("To Number");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<TokenRequestActivating>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Token_Request_Activating");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TokenType)
                .HasMaxLength(30)
                .HasColumnName("Token Type");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<TokenRequestCancelation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Token_Request_Cancelation");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TokenType)
                .HasMaxLength(30)
                .HasColumnName("Token Type");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<TokenRequestRegistration>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Token_Request_Registration");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Agreement).HasMaxLength(1);
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TokenType)
                .HasMaxLength(30)
                .HasColumnName("Token Type");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<TokenRequestResendTknLink>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Token_Request_Resend_tkn_link");

            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TokenType)
                .HasMaxLength(30)
                .HasColumnName("Token Type");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<TransFastEnjazEasyCharity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TransFast_EnjazEasy_Charity");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.BeneficiaryDescription)
                .HasMaxLength(100)
                .HasColumnName("Beneficiary Description");
            entity.Property(e => e.BeneficiaryNickname)
                .HasMaxLength(100)
                .HasColumnName("Beneficiary Nickname");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("Phone Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.Purpose).HasMaxLength(100);
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<UpdateBeneficiaryLocalBank>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Update_Beneficiary_Local_Bank");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.BeneficiaryDescription)
                .HasMaxLength(100)
                .HasColumnName("Beneficiary Description");
            entity.Property(e => e.BeneficiaryNickname)
                .HasMaxLength(100)
                .HasColumnName("Beneficiary Nickname");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW_RESPONSE");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("Phone Number");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        modelBuilder.Entity<UpdateIdExpiryDate>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UPDATE_ID_EXPIRY_DATE");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .HasColumnName("Account Number");
            entity.Property(e => e.ActCloseDt)
                .HasColumnType("datetime")
                .HasColumnName("ACT_CLOSE_DT");
            entity.Property(e => e.AgentComments)
                .HasMaxLength(250)
                .HasColumnName("Agent Comments");
            entity.Property(e => e.AliasName)
                .HasMaxLength(50)
                .HasColumnName("ALIAS_NAME");
            entity.Property(e => e.Attrib26)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_26");
            entity.Property(e => e.Attrib28)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_28");
            entity.Property(e => e.Attrib31)
                .HasColumnType("datetime")
                .HasColumnName("ATTRIB_31");
            entity.Property(e => e.BpServiceDesc)
                .HasMaxLength(100)
                .HasColumnName("BP_SERVICE_DESC");
            entity.Property(e => e.Branch).HasMaxLength(30);
            entity.Property(e => e.CaseType)
                .HasMaxLength(30)
                .HasColumnName("Case Type");
            entity.Property(e => e.ChCheqName2)
                .HasMaxLength(100)
                .HasColumnName("CH_CHEQ_NAME2");
            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("Completion Date");
            entity.Property(e => e.Created)
                .HasColumnType("datetime")
                .HasColumnName("CREATED");
            entity.Property(e => e.Csn)
                .HasMaxLength(50)
                .HasColumnName("CSN");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer Name");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(350);
            entity.Property(e => e.FinalReplyStatment)
                .HasMaxLength(100)
                .HasColumnName("Final Reply Statment");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First Name");
            entity.Property(e => e.Investmentaccountnum)
                .HasMaxLength(100)
                .HasColumnName("INVESTMENTACCOUNTNUM");
            entity.Property(e => e.IsEligibleOfficialLetter)
                .HasMaxLength(1)
                .HasColumnName("Is Eligible Official Letter");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last Name");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("LOGIN");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(100)
                .HasColumnName("Mobile Number");
            entity.Property(e => e.MwResponse)
                .HasMaxLength(2000)
                .HasColumnName("MW Response");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME");
            entity.Property(e => e.PrductLookUpId)
                .HasMaxLength(15)
                .HasColumnName("Prduct_LookUP_Id");
            entity.Property(e => e.Priority).HasMaxLength(30);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("Product Name");
            entity.Property(e => e.RequestSubType)
                .HasMaxLength(30)
                .HasColumnName("Request Sub Type");
            entity.Property(e => e.RequestType)
                .HasMaxLength(30)
                .HasColumnName("Request Type");
            entity.Property(e => e.ReveiveMethod)
                .HasMaxLength(30)
                .HasColumnName("Reveive Method");
            entity.Property(e => e.Source).HasMaxLength(30);
            entity.Property(e => e.SrArea)
                .HasMaxLength(30)
                .HasColumnName("SR_AREA");
            entity.Property(e => e.SrNum)
                .HasMaxLength(64)
                .HasColumnName("SR_NUM");
            entity.Property(e => e.SrStatId)
                .HasMaxLength(30)
                .HasColumnName("SR_STAT_ID");
            entity.Property(e => e.SrStatus)
                .HasMaxLength(30)
                .HasColumnName("SR_STATUS");
            entity.Property(e => e.SrSubArea)
                .HasMaxLength(30)
                .HasColumnName("SR_SUB_AREA");
            entity.Property(e => e.SrTitle)
                .HasMaxLength(100)
                .HasColumnName("SR_TITLE");
            entity.Property(e => e.SubType)
                .HasMaxLength(100)
                .HasColumnName("Sub Type");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("datetime")
                .HasColumnName("Submission Date");
            entity.Property(e => e.TransactionAuthorizationCode)
                .HasMaxLength(100)
                .HasColumnName("Transaction Authorization Code");
            entity.Property(e => e.TransactionCardType)
                .HasMaxLength(50)
                .HasColumnName("Transaction Card Type");
            entity.Property(e => e.TransactionDate)
                .HasMaxLength(30)
                .HasColumnName("Transaction Date");
            entity.Property(e => e.TransactionReference)
                .HasMaxLength(30)
                .HasColumnName("Transaction Reference");
            entity.Property(e => e.Transactiondesc)
                .HasMaxLength(100)
                .HasColumnName("TRANSACTIONDESC");
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("USER ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
