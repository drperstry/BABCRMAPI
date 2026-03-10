namespace BabCrm.Service
{
    using BabCrm.Core;
    using BabCrm.Core.Caching;
    using BabCrm.Logging;
    using BabCrm.Service.ArchiveDataModels;
    using BabCrm.Service.Models;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ServiceManager
    {
        private readonly IServiceStore _serviceStore;
        private readonly IArchiveSqlStore _archiveSqlStore;
        private readonly ICacheManager _cacheManager;


        private readonly Dictionary<string, string> _categories;
        private readonly Dictionary<string, string> _skillGroups;
        private readonly IEnumerable<InactiveProducts> _inactiveProducts;

        private readonly Dictionary<string, string> _channels;

        private string _nonCustomerCif;

        private string _nonCustomerLegalId;

        private string officialLetterTitleAr;

        private string officialLetterTitleEn;

        private string businessUnitId;

        private string teamId;

        private int notificationCheckStatus;

        private int notificationSetStatus;

        private int surveyResponseExpiryInterval;

        private readonly Dictionary<string, string> _caseStatusMapping;

        private readonly Dictionary<string, string> domainMappings;

        private readonly string _autodialerSharedFolderPath;

        private readonly string _sharedFolderPath;

        private readonly bool _uploadToSharePoint;




        public ServiceManager(IServiceStore serviceStore, ICacheManager inMemoryCacheManager, IConfiguration configuration, IArchiveSqlStore archiveSqlStore)
        {
            this._serviceStore = serviceStore;
            this._cacheManager = inMemoryCacheManager;
            _categories = configuration.GetSection("CategoryIdByCode").GetChildren()
                  .ToDictionary(x => x.Key, x => x.Value);

            _skillGroups = configuration.GetSection("SkillGroupMapping").GetChildren()
                 .ToDictionary(x => x.Key, x => x.Value);

            _channels = configuration.GetSection("ChannelIdByCode").GetChildren()
                  .ToDictionary(x => x.Key, x => x.Value);

            _inactiveProducts = configuration.GetSection("InactiveProducts").Get<List<InactiveProducts>>();

            _nonCustomerCif = configuration.GetValue<string>("NonCustomerCif");

            _nonCustomerLegalId = configuration.GetValue<string>("NonCustomerLegalId");

            _archiveSqlStore = archiveSqlStore;

            _caseStatusMapping = configuration.GetSection("CaseStatusMapping").GetChildren()
                 .ToDictionary(x => x.Key, x => x.Value);

            var officialLetterSection = configuration.GetSection("OfficialLetter");

            officialLetterTitleAr = officialLetterSection["TitleAr"];

            officialLetterTitleEn = officialLetterSection["TitleEn"];

            notificationCheckStatus = configuration.GetValue<int>("NotificationMethodConfiguration:CheckStatus");

            notificationSetStatus = configuration.GetValue<int>("NotificationMethodConfiguration:SetStatus");

            var rmConfig = configuration.GetSection("RMConfiguration");

            businessUnitId = rmConfig["BusinessUnitId"];

            teamId = rmConfig["TeamId"];

            surveyResponseExpiryInterval = configuration.GetValue<int>("SurveyResponseExpiryInterval");

            domainMappings = configuration.GetSection("DomainMappings").GetChildren().ToDictionary(x => x.Key, x => x.Value);

            _autodialerSharedFolderPath = configuration["AutodialerSharedFolderPath"];
            if (string.IsNullOrWhiteSpace(_autodialerSharedFolderPath))
            {
                throw new Exception("AutodialerSharedFolderPath is not set in appsettings.");
            }

            _uploadToSharePoint = configuration.GetValue<bool>("UploadToSharepoint");
            _sharedFolderPath = configuration.GetValue<string>("SharedFolderPath");

        }

        public async Task<string> WhoAmI() => await _serviceStore.WhoAmI();

        public async Task<IEnumerable<SamaLookupModel>> GetProducts() =>
            await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.Products, async () => await _serviceStore.GetProducts());

        public async Task<IEnumerable<SamaLookupModel>> GetChannelProducts(string channelCode) => await _serviceStore.GetProducts(channelCode);

        public async Task<IEnumerable<BaseLookupModel>> GetCategories() =>
            await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.Categories, async () => await _serviceStore.GetCategories());

        public async Task<IEnumerable<BaseLookupModel>> GetChannelCategories(string channelCode)
        {
            var channels = await _serviceStore.GetCategories(channelCode);

            if (channels == null)
            {
                return Enumerable.Empty<BaseLookupModel>();
            }

            return channels;

        }

        public async Task<IEnumerable<SamaLookupModel>> GetTicketTypes(string categoryCode, string samaProductCode = "")
        {
            var ticketTypes = await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.TicketTypes + samaProductCode,
                async () => await _serviceStore.GetTicketTypes(samaProductCode));

            if (categoryCode.IsEmpty())
            {
                return ticketTypes;
            }

            return ticketTypes?.Where(t => t.RelatedRecordCode.Equals(categoryCode));
        }

        public async Task<IEnumerable<BaseLookupModel>> GetDepartments() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.Departments,
             async () => await _serviceStore.GetDepartments());




        public async Task<IEnumerable<BaseLookupModel>> GetInternalProducts() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.InternalProducts,
             async () => await _serviceStore.GetInternalProducts());

        public async Task<IEnumerable<LookupModel>> GetTicketStatusOptions() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.TicketStatusOptions,
             async () => await _serviceStore.GetTicketStatusCodes());

        public async Task<IEnumerable<BaseLookupModel>> GetLeadProducts() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.LeadProducts,
             async () => await _serviceStore.GetLeadProducts());

        public async Task<IEnumerable<BaseLookupModel>> GetCustomerMessageTypes() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.CustomerMessageTypes,
             async () => await _serviceStore.GetCustomerMessageTypes());

        public async Task<IEnumerable<BaseLookupModel>> GetCustomerMessageSubjects() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.CustomerMessageSubjects,
             async () => await _serviceStore.GetCustomerMessageSubjects());

        public async Task<IEnumerable<Currency>> GetTransactionCurrencies() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.TransactionCurrencies,
             async () => await _serviceStore.GetTransactionCurrencies());

        public async Task<IEnumerable<LookupModel>> GetCardTypes() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.CardTypes,
             async () => await _serviceStore.GetCardTypes());

        public async Task<IEnumerable<LookupModel>> GetNotificationChannels() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.NotificationChannels,
            async () => await _serviceStore.GetNotificationChannels());

        public async Task<IEnumerable<LookupModel>> GetLegalIdTypes() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.LegalIdTypes,
            async () => await _serviceStore.GetLegalIdTypes());

        public async Task<IEnumerable<LookupModel>> GetMonthlySalaryOptions() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.MonthlySalaryOptions,
            async () => await _serviceStore.GetMonthlySalaryOptions());

        public async Task<IEnumerable<LookupModel>> GetDividions() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.Divisions,
            async () => await _serviceStore.GetDividions());

        public async Task<IEnumerable<LookupModel>> GetPreferedTimeOptions() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.PreferedTimeOptions,
            async () => await _serviceStore.GetPreferedTimeOptions());

        public async Task<IEnumerable<SamaLookupModel>> GetSubTypes(string ticketTypeCode)
        {
            var ticketSubTypes = await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.TicketSubTypes,
            async () => await _serviceStore.GetSubTypes());

            if (ticketTypeCode.IsEmpty())
            {
                return ticketSubTypes;
            }

            return ticketSubTypes.Where(t => t.RelatedRecordCode == ticketTypeCode);
        }

        public async Task<IEnumerable<BaseLookupModel>> GetChannels() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.Channels,
            async () => await _serviceStore.GetChannels());

        public async Task<IEnumerable<BaseLookupModel>> GetCountries() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.Countries,
            async () => await _serviceStore.GetCountries());

        public async Task<IEnumerable<BaseLookupModel>> GetLanguages() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.Languages,
            async () => await _serviceStore.GetLanguages());

        public async Task<IEnumerable<BaseLookupModel>> GetTeleSalesChannels() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.TeleSalesChannels,
            async () => await _serviceStore.GetTeleSalesChannels());

        public async Task<IEnumerable<LookupModel>> GetTppTicketCategories() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.TppTicketCategories,
                async () => await _serviceStore.GetTppTicketCategories());


        public async Task<IEnumerable<LookupModel>> GetTppTicketTypes(string categoryCode) => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.TppTicketTypes + categoryCode,
                async () => await _serviceStore.GetTppTicketTypes(categoryCode));


        public async Task<IEnumerable<LookupModel>> GetBankEnvironments() => await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.BankEnvironments,
                async () => await _serviceStore.GetBankEnvironments());


        public async Task<SubmissionResponse> GetTickets(string cif, string channelCode, TicketFilter filter, CultureInfo cultureInfo)
        {
            try
            {
                if (cif.IsEmpty() && filter.NationalId.IsEmpty() && filter.MobileNumber.IsEmpty())
                {
                    return SubmissionResponse.Error("1000", "At least one identifier must be provided: CIF, NationalId, or MobileNumber.");
                }

                var contactId = cif.IsEmpty() ? "" : await _serviceStore.GetContactIdByCif(cif);

                //if (contactId.IsEmpty())
                //{
                //    return SubmissionResponse.Error("1000", "Invalid CIF");
                //}

                var channels = await GetChannels();
                //var channelId = channels.FirstOrDefault(x => x.Code.Equals(channelCode))?.Id;
                var channelId = (channels?.FirstOrDefault(x => x?.Code != null && x.Code.Equals(channelCode))?.Id) ?? "";

                if (!filter.CategoryCode.IsEmpty())
                {
                    filter.CategoryCode = _categories[filter.CategoryCode];
                }

                //var cardTypesTask = GetCardTypes();
                var replyMethodsTask = GetNotificationChannels();
                var productsTask = GetProducts();
                var caseTypesTask = GetTicketTypes("");
                var caseSubTypesTask = GetSubTypes("");
                var transactionCurrenciesTask = GetTransactionCurrencies();
                var categoriesTask = GetCategories();
                var departmentsTask = GetDepartments();
                var languagesTask = GetLanguages();

                var caseTypes = await caseTypesTask;
                var caseSubTypes = await caseSubTypesTask;

                filter.TypeCode = filter.TypeCode.IsEmpty() ? null : caseTypes.FirstOrDefault(x => x.SamaValues.Code == filter.TypeCode).SamaValues.Id;
                filter.SubTypeCode = filter.SubTypeCode.IsEmpty() ? null : caseSubTypes.FirstOrDefault(x => x.SamaValues.Code == filter.SubTypeCode).SamaValues.Id;

                IEnumerable<CaseViewModel> groupedCases = Enumerable.Empty<CaseViewModel>();

                var result = await _serviceStore.GetTickets(contactId, channelId, filter, cultureInfo.Name);

                if (result != null)
                {
                    //var cardTypes = await cardTypesTask;
                    var replyMethods = await replyMethodsTask;
                    var products = await productsTask;
                    var transactionCurrencies = await transactionCurrenciesTask;
                    var categories = await categoriesTask;
                    var departments = await departmentsTask;
                    var languages = await languagesTask;

                    foreach (var item in result)
                    {
                        //item.TransactionCardType = item.TransactionCardType.IsEmpty() ? null : cardTypes.FirstOrDefault(x => x.Code.Equals(item.TransactionCardType)).Name;
                        item.ReplyMethod = item.ReplyMethod.IsEmpty() ? null : replyMethods?.FirstOrDefault(x => x.Code.Equals(item.ReplyMethod))?.Name;
                        item.Product = item.Product.IsEmpty() ? null : products?.FirstOrDefault(x => x.SamaValues.Id.Equals(item.Product))?.SamaValues?.Name[cultureInfo]?.Value;
                        item.CaseType = item.CaseType.IsEmpty() ? null : caseTypes?.FirstOrDefault(x => x.SamaValues.Id.Equals(item.CaseType))?.SamaValues?.Name[cultureInfo]?.Value;
                        item.CaseSubType = item.CaseSubType.IsEmpty() ? null : caseSubTypes?.FirstOrDefault(x => x.SamaValues.Id.Equals(item.CaseSubType))?.SamaValues?.Name[cultureInfo]?.Value;
                        item.TransactionCurrency = item.TransactionCurrency.IsEmpty() ? null : transactionCurrencies?.FirstOrDefault(x => x.CurrencyId.Equals(item.TransactionCurrency))?.CurrencyName;
                        item.CaseCategory = item.CaseCategory.IsEmpty() ? null : categories?.FirstOrDefault(x => x.Id.Equals(item.CaseCategory))?.Name[cultureInfo]?.Value;
                        item.Department = item.Department.IsEmpty() ? null : departments?.FirstOrDefault(x => x.Id.Equals(item.Department))?.Name[cultureInfo]?.Value;
                        item.PreferredSMSLanguage = item.PreferredSMSLanguage.IsEmpty() ? null : languages?.FirstOrDefault(x => x.Id.Equals(item.PreferredSMSLanguage))?.Name[cultureInfo]?.Value;

                        if (item.IsEligibleForOfficialLetter == "Y" && item.IsOfficialLetterGenerated)
                        {
                            var officialLetterAttachement = new AttachmentInfo
                            {
                                FileName = item.OfficialLetterFileName,
                                AttachmentId = item.OfficialLetterFileName,
                                RecordId = null,
                                FilePath = item.OfficialLetterPath,
                                TitleEn = officialLetterTitleEn,
                                TitleAr = officialLetterTitleAr
                            };

                            item.Attachments.Add(officialLetterAttachement);
                        }
                    }


                    var recordsId = result.Select(r => r.CaseId).Distinct();

                    var attachmentsInfo = await GetAttachmentsInfo(RecordType.INCIDENT, recordsId);

                    if (!attachmentsInfo.IsEmpty())
                    {
                        foreach (var item in result.ToList())
                        {
                            item.Attachments.AddRange(attachmentsInfo.Where(a => a.RecordId.Equals(item.CaseId, StringComparison.OrdinalIgnoreCase)));
                        }
                    }

                    groupedCases = GroupCases(result);
                }

                if (!cif.IsEmpty() || !filter.MobileNumber.IsEmpty())
                {
                    var archivedCases = await GetArchivedCases(cif, filter.StartDate, filter.EndDate, filter.IsInternal, cultureInfo, filter.StatusCode, filter.MobileNumber);

                    var allCases = groupedCases.SafeConcat(archivedCases);

                    return allCases.Any() ? SubmissionResponse.Ok(allCases) : SubmissionResponse.Ok("No cases found");
                }
                else
                {
                    return groupedCases.Any() ? SubmissionResponse.Ok(groupedCases) : SubmissionResponse.Ok("No cases found");
                }

            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetTickets), null);
                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<SubmissionResponse> GetArchivedData(string tableName, CommonFilter filter, CultureInfo cultureInfo)
        {
            var archivedDataResult = await _archiveSqlStore.GetArchivedData(tableName, filter);

            if (tableName == "Case_Request" && archivedDataResult.ExtraData != null)
            {
                var cases = (IEnumerable<CaseRequest>)archivedDataResult.ExtraData;

                var result = cases?.Select(p => new CaseRequestModel
                {
                    AccountNumber = p.AccountNumber,
                    TransactionAuthorizationCode = p.TransactionAuthorizationCode,
                    Description = p.Description,
                    TransactionReference = p.TransactionReference,
                    TransactionCardType = p.TransactionCardType,
                    CustomerName = p.CustomerName,
                    AgentComments = p.AgentComments,
                    Branch = p.Branch,
                    CaseType = p.CaseType,
                    Cif = p.Cif,
                    CompletionDate = p.CompletionDate,
                    CrmCategoryId = p.CrmCategoryId,
                    CrmProductId = p.CrmProductId,
                    Source = p.Source,
                    SrNum = p.SrNum,
                    CrmStatusId = p.CrmStatusId,
                    SrStatus = p.SrStatus,
                    CrmSubTypeId = p.CrmSubTypeId,
                    CrmTypeId = p.CrmTypeId,
                    Department = p.Department,
                    Email = p.Email,
                    FinalReplyStatment = p.FinalReplyStatment,
                    FirstName = p.FirstName,
                    IsEligibleOfficialLetter = p.IsEligibleOfficialLetter,
                    LastName = p.LastName,
                    MobileNumber = p.MobileNumber,
                    MwResponse = p.MwResponse,
                    PrductLookUpId = p.PrductLookUpId,
                    Priority = p.Priority,
                    ProductName = p.ProductName,
                    RequestSubType = p.RequestSubType,
                    RequestType = p.RequestType,
                    ReveiveMethod = p.ReveiveMethod,
                    SubmissionDate = p.SubmissionDate,
                    SubType = p.SubType,
                    TransactionDate = p.TransactionDate,
                    Type = p.Type,
                    UserId = p.UserId,
                    CrmCategory = cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmCategory?.EnglishName : p.CrmCategory?.ArabicName,
                    CrmType = cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmType?.InternalEnglishName : p.CrmType?.InternalArabicName,
                    CrmProduct = cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmProduct?.InternalEnglishName : p.CrmProduct?.InternalArabicName,
                    CrmSubType = cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmSubType?.InternalEnglishName : p.CrmSubType?.InternalArabicName,
                    CrmStatus = cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmStatus?.EnglishName : p.CrmStatus?.ArabicName
                });

                return SubmissionResponse.Ok(result);
            }

            return archivedDataResult;
        }

        private async Task<IEnumerable<CaseViewModel>> GetArchivedCases(string cif, DateTime? startDate, DateTime? endDate, bool? isInternal, CultureInfo cultureInfo, string statusCode, string mobileNumber)
        {
            var archivedCases = await _archiveSqlStore.GetArchivedCases(cif, startDate, endDate, mobileNumber);

            if (!string.IsNullOrEmpty(statusCode))
            {
                var status = _caseStatusMapping[statusCode];

                archivedCases = archivedCases.Where(s => s.CrmStatusId.ToString() == status);
            }
            ;

            var cases = archivedCases?.Select(p => new CaseViewModel
            {
                AccountNumber = p?.AccountNumber,
                TransactionAuthorizationCode = p?.TransactionAuthorizationCode,
                Description = p?.Description,
                TransactionReference = p?.TransactionReference,
                TransactionCardType = p?.TransactionCardType,
                CustomerName = p?.CustomerName,
                CaseSubType = isInternal.Value ? (cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmSubType?.InternalEnglishName : p.CrmSubType?.InternalArabicName) : (cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmSubType?.SamaEnglishName : p.CrmSubType?.SamaArabicName),
                CaseType = isInternal.Value ? (cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmType?.InternalEnglishName : p.CrmType?.InternalArabicName) : (cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmType?.SamaEnglishName : p.CrmType?.SamaArabicName),
                Product = isInternal.Value ? (cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmProduct?.InternalEnglishName : p.CrmProduct?.InternalArabicName) : (cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmProduct?.SamaEnglishName : p.CrmProduct?.SamaArabicName),
                CaseCategory = cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmCategory?.EnglishName : p.CrmCategory?.ArabicName,
                Status = cultureInfo.Name.EqualsIgnorCase("en") ? p.CrmStatus?.EnglishName : p.CrmStatus?.ArabicName,
                IsFromArchive = true
            });

            return cases;
        }

        public async Task<SubmissionResponse> GetTicketByKey(string key, CultureInfo cultureInfo)
        {
            try
            {
                var keyValues = GetValuesFromKey(key);

                //var cardTypesTask = GetCardTypes();
                var replyMethodsTask = GetNotificationChannels();
                var productsTask = GetProducts();
                var caseTypesTask = GetTicketTypes("");
                var caseSubTypesTask = GetSubTypes("");
                var transactionCurrenciesTask = GetTransactionCurrencies();
                var categoriesTask = GetCategories();
                var departmentsTask = GetDepartments();
                var languagesTask = GetLanguages();

                var result = await _serviceStore.GetTicketByKey(keyValues.CaseNumber, keyValues.CifLast4Digits, keyValues.GuidLast4Digits, cultureInfo.Name);

                if (result != null)
                {
                    var recordsId = result.Select(r => r.CaseId);

                    var attachmentsInfo = await GetAttachmentsInfo(RecordType.INCIDENT, recordsId);

                    if (!attachmentsInfo.IsEmpty())
                    {
                        foreach (var item in result.ToList())
                        {
                            item.Attachments.AddRange(attachmentsInfo.Where(a => a.RecordId.Equals(item.CaseId, StringComparison.OrdinalIgnoreCase)));
                        }
                    }

                    IEnumerable<CaseViewModel> groupedCases = GroupCases(result);

                    var finalRresult = groupedCases.FirstOrDefault();

                    //var cardTypes = await cardTypesTask;
                    var replyMethods = await replyMethodsTask;
                    var products = await productsTask;
                    var caseTypes = await caseTypesTask;
                    var caseSubTypes = await caseSubTypesTask;
                    var transactionCurrencies = await transactionCurrenciesTask;
                    var categories = await categoriesTask;
                    var departments = await departmentsTask;
                    var languages = await languagesTask;

                    //finalRresult.TransactionCardType = finalRresult.TransactionCardType.IsEmpty() ? null : cardTypes.FirstOrDefault(x => x.Code.Equals(finalRresult.TransactionCardType)).Name;
                    finalRresult.ReplyMethod = finalRresult.ReplyMethod.IsEmpty() ? null : replyMethods.FirstOrDefault(x => x.Code.Equals(finalRresult.ReplyMethod)).Name;
                    finalRresult.Product = finalRresult.Product.IsEmpty() ? null : products.FirstOrDefault(x => x.SamaValues.Id.Equals(finalRresult.Product)).SamaValues.Name[cultureInfo].Value;
                    finalRresult.CaseType = finalRresult.CaseType.IsEmpty() ? null : caseTypes.FirstOrDefault(x => x.SamaValues.Id.Equals(finalRresult.CaseType)).SamaValues.Name[cultureInfo].Value;
                    finalRresult.CaseSubType = finalRresult.CaseSubType.IsEmpty() ? null : caseSubTypes.FirstOrDefault(x => x.SamaValues.Id.Equals(finalRresult.CaseSubType)).SamaValues.Name[cultureInfo].Value;
                    finalRresult.TransactionCurrency = finalRresult.TransactionCurrency.IsEmpty() ? null : transactionCurrencies.FirstOrDefault(x => x.CurrencyId.Equals(finalRresult.TransactionCurrency)).CurrencyName;
                    finalRresult.CaseCategory = finalRresult.CaseCategory.IsEmpty() ? null : categories.FirstOrDefault(x => x.Id.Equals(finalRresult.CaseCategory)).Name[cultureInfo].Value;
                    finalRresult.Department = finalRresult.Department.IsEmpty() ? null : departments.FirstOrDefault(x => x.Id.Equals(finalRresult.Department)).Name[cultureInfo].Value;
                    finalRresult.PreferredSMSLanguage = finalRresult.PreferredSMSLanguage.IsEmpty() ? null : languages.FirstOrDefault(x => x.Id.Equals(finalRresult.PreferredSMSLanguage)).Name[cultureInfo].Value;


                    return SubmissionResponse.Ok(finalRresult);
                }

                return SubmissionResponse.Ok("No cases found");
            }
            catch (Exception ex)
            {
                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<SubmissionResponse> GetTicketByKeyV2(string key, CultureInfo cultureInfo)
        {
            try
            {
                string caseGuid = null;

                string caseNumber = null;

                bool isKeyGuid = IsValueGuid(key);

                if (isKeyGuid)
                {
                    caseGuid = key;
                }
                else
                {
                    caseNumber = ExtractCaseNumberFromKey(key);
                }

                //var cardTypesTask = GetCardTypes();
                var replyMethodsTask = GetNotificationChannels();
                var productsTask = GetProducts();
                var caseTypesTask = GetTicketTypes("");
                var caseSubTypesTask = GetSubTypes("");
                var transactionCurrenciesTask = GetTransactionCurrencies();
                var categoriesTask = GetCategories();
                var departmentsTask = GetDepartments();
                var languagesTask = GetLanguages();

                var result = await _serviceStore.GetTicketByKey(caseNumber, caseGuid, cultureInfo.Name);

                if (result != null)
                {
                    var recordsId = result.Select(r => r.CaseId);

                    var attachmentsInfo = await GetAttachmentsInfo(RecordType.INCIDENT, recordsId);

                    if (!attachmentsInfo.IsEmpty())
                    {
                        foreach (var item in result.ToList())
                        {
                            item.Attachments.AddRange(attachmentsInfo.Where(a => a.RecordId.Equals(item.CaseId, StringComparison.OrdinalIgnoreCase)));
                        }
                    }

                    IEnumerable<CaseViewModel> groupedCases = GroupCases(result);

                    var finalRresult = groupedCases.FirstOrDefault();

                    var caseId = finalRresult.CaseId;
                    bool isSurveyExpired = false;

                    var caseResolutionTime = await _serviceStore.GetCaseResolutionTime(caseId);

                    if (caseResolutionTime != null)
                    {
                        isSurveyExpired = CheckSurveyExpiry(caseResolutionTime);
                    }

                    if (finalRresult.CaseSurveyResponses == null)
                    {
                        finalRresult.CaseSurveyResponses = new List<CaseSurveyResponse>();

                        //200000001 Does not exist
                        //200000002 Expired
                        var surveyResponse = new CaseSurveyResponse();
                        surveyResponse.SurveyResponseStatus = isSurveyExpired ? "200000002" : "200000001";
                        finalRresult.CaseSurveyResponses.Add(surveyResponse);
                    }
                    else
                    {
                        if (isSurveyExpired)
                        {
                            foreach (var item in finalRresult.CaseSurveyResponses)
                            {
                                item.SurveyResponseStatus = "200000002";
                            }
                        }
                    }

                    //var cardTypes = await cardTypesTask;
                    var replyMethods = await replyMethodsTask;
                    var products = await productsTask;
                    var caseTypes = await caseTypesTask;
                    var caseSubTypes = await caseSubTypesTask;
                    var transactionCurrencies = await transactionCurrenciesTask;
                    var categories = await categoriesTask;
                    var departments = await departmentsTask;
                    var languages = await languagesTask;

                    //finalRresult.TransactionCardType = finalRresult.TransactionCardType.IsEmpty() ? null : cardTypes.FirstOrDefault(x => x.Code.Equals(finalRresult.TransactionCardType)).Name;
                    finalRresult.ReplyMethod = finalRresult.ReplyMethod.IsEmpty() ? null : replyMethods.FirstOrDefault(x => x.Code.Equals(finalRresult.ReplyMethod)).Name;
                    finalRresult.Product = finalRresult.Product.IsEmpty() ? null : products.FirstOrDefault(x => x.SamaValues.Id.Equals(finalRresult.Product)).SamaValues.Name[cultureInfo].Value;
                    finalRresult.CaseType = finalRresult.CaseType.IsEmpty() ? null : caseTypes.FirstOrDefault(x => x.SamaValues.Id.Equals(finalRresult.CaseType)).SamaValues.Name[cultureInfo].Value;
                    finalRresult.CaseSubType = finalRresult.CaseSubType.IsEmpty() ? null : caseSubTypes.FirstOrDefault(x => x.SamaValues.Id.Equals(finalRresult.CaseSubType)).SamaValues.Name[cultureInfo].Value;
                    finalRresult.TransactionCurrency = finalRresult.TransactionCurrency.IsEmpty() ? null : transactionCurrencies.FirstOrDefault(x => x.CurrencyId.Equals(finalRresult.TransactionCurrency)).CurrencyName;
                    finalRresult.CaseCategory = finalRresult.CaseCategory.IsEmpty() ? null : categories.FirstOrDefault(x => x.Id.Equals(finalRresult.CaseCategory)).Name[cultureInfo].Value;
                    finalRresult.Department = finalRresult.Department.IsEmpty() ? null : departments.FirstOrDefault(x => x.Id.Equals(finalRresult.Department)).Name[cultureInfo].Value;
                    finalRresult.PreferredSMSLanguage = finalRresult.PreferredSMSLanguage.IsEmpty() ? null : languages.FirstOrDefault(x => x.Id.Equals(finalRresult.PreferredSMSLanguage)).Name[cultureInfo].Value;


                    return SubmissionResponse.Ok(finalRresult);
                }

                return SubmissionResponse.Ok("No cases found");
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetTicketByKeyV2), null);
                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<SubmissionResponse> CreateNewTicket(TicketRequest request)
        {
            try
            {
                string contactCif = "";


                if (string.IsNullOrEmpty(request.CIF) && !string.IsNullOrEmpty(request.GovernmentId))
                {
                    var contactInfo = await _serviceStore.GetContactInfoByLegalId(request.GovernmentId);

                    if (contactInfo != null)
                    {
                        contactCif = contactInfo.Cif;
                        request.PreferredSMSLanguage = contactInfo.PrefferedLanguage;
                    }

                }

                if (string.IsNullOrEmpty(contactCif))
                {
                    contactCif = request.CIF.IsEmpty() ? _nonCustomerCif : request.CIF;
                }

                var contactInfoByCif = await _serviceStore.GetContactInfoByCif(contactCif);
                var contactId = contactInfoByCif.ContactId;

                if (!request.CIF.IsEmpty())
                {
                    request.PreferredSMSLanguage = contactInfoByCif.PrefferedLanguage;
                }

                if (contactId.IsEmpty())
                {
                    return SubmissionResponse.Error("1000", "Invalid CIF");
                }

                //TODO double check if a new product was added
                var categoryId = _categories[request.CategoryCode];
                var channels = await GetChannels();
                var channelId = channels.FirstOrDefault(x => x.Code.Equals(request.ChannelCode)).Id;

                var products = await GetProducts();
                request.InternalProductId = products?.FirstOrDefault(x => x.SamaValues.Code.Equals(request.ProductServiceId))?.Id;

                if (request.InternalProductId == null)
                {
                    request.InternalProductId = _inactiveProducts?.FirstOrDefault(x => x.Product.Equals(request.ProductServiceId))?.InternalProductId;
                }

                var ticketTypes = await GetTicketTypes(request.CategoryCode);
                request.InternalTicketTypeId = ticketTypes?.FirstOrDefault(x => x.SamaValues.Code.Equals(request.TypeId)).Id;

                var ticketSubTypes = await GetSubTypes(request.TypeId);
                request.InternalTicketSubTypeId = ticketSubTypes?.FirstOrDefault(x => x.SamaValues.Code.Equals(request.SubTypeId)).Id;

                //check if required
                if (!request.PreferredSMSLanguage.IsEmpty())
                {
                    var languages = await GetLanguages();
                    request.PreferredSMSLanguage = languages?.FirstOrDefault(x => x.Code.Equals(request.PreferredSMSLanguage)).Id;
                }

                //sama type
                var typeId = ticketTypes?.FirstOrDefault(x => x.SamaValues.Code.Equals(request.TypeId)).SamaValues.Id;

                //sama subtype
                var subTypeId = ticketSubTypes?.FirstOrDefault(x => x.SamaValues.Code.Equals(request.SubTypeId)).SamaValues.Id;

                //sama product
                var productServiceId = products?.FirstOrDefault(x => x.SamaValues.Code.Equals(request.ProductServiceId))?.SamaValues?.Id;

                if (productServiceId == null)
                {
                    request.ProductServiceId = _inactiveProducts?.FirstOrDefault(x => x.Product.Equals(request.ProductServiceId))?.ProductId;
                }
                else
                {
                    request.ProductServiceId = products?.FirstOrDefault(x => x.SamaValues.Code.Equals(request.ProductServiceId))?.SamaValues?.Id;
                }

                if (!request.TransactionCurrencyId.IsEmpty())
                {
                    var transactionCurrencies = await GetTransactionCurrencies();
                    request.TransactionCurrencyId = transactionCurrencies.FirstOrDefault(x => x.CurrencyCode.Equals(request.TransactionCurrencyId)).CurrencyId;
                }

                if (!request.DepartmentId.IsEmpty())
                {
                    var departments = await GetDepartments();
                    request.DepartmentId = departments.FirstOrDefault(x => x.Code.Equals(request.DepartmentId)).Id;
                }

                request.CategoryCode = categoryId;
                request.ChannelCode = channelId;
                request.TypeId = typeId;
                request.SubTypeId = subTypeId;

                if (request.ReplyMethod == notificationCheckStatus || request.ReplyMethod == null)
                {
                    request.ReplyMethod = notificationSetStatus;
                }


                var ticketId = await _serviceStore.CreateNewTicket(request, contactId);

                if (ticketId.IsEmpty())
                {
                    return SubmissionResponse.Error("2001", "Failed to create new ticket");
                }

                var result = new NewTicketInfoModel();

                if (request.Attachment != null)
                {
                    if (!_uploadToSharePoint)
                    {
                        var uploadAttachmentResponse = await UploadAttachment(request.Attachment, ticketId);

                        if (uploadAttachmentResponse.Success)
                        {
                            await _serviceStore.UpdateTicketAttachmentPath(uploadAttachmentResponse.FilePath, ticketId);
                        }

                        result.CaseNumber = await _serviceStore.GetTicketNumberById(ticketId);
                        result.CaseId = ticketId;
                        result.AttachmentInfo = uploadAttachmentResponse;

                        return SubmissionResponse.Ok(result);
                    }
                    else
                    {
                        //not implemented.
                    }
                }

                result.CaseNumber = await _serviceStore.GetTicketNumberById(ticketId);
                result.CaseId = ticketId;

                return SubmissionResponse.Ok(result);
            }
            catch (Exception ex)
            {

                Logger.ApiLog(null, ex, nameof(CreateNewTicket), null);
                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<SubmissionResponse> CreateProspect(ProspectRequestModel prospectRequest)
        {
            try
            {
                if (!prospectRequest.PreferredProduct.IsEmpty())
                {
                    var products = await GetLeadProducts();
                    prospectRequest.PreferredProduct = products?.FirstOrDefault(x => x.Code != null && x.Code.Equals(prospectRequest.PreferredProduct))?.Id;
                }

                if (!prospectRequest.Nationality.IsEmpty())
                {
                    var countries = await GetCountries();
                    prospectRequest.Nationality = countries?.FirstOrDefault(x => x.Code != null && x.Code.Equals(prospectRequest.Nationality))?.Id;
                }

                if (!prospectRequest.Channel.IsEmpty())
                {
                    var teleSalesChannels = await GetTeleSalesChannels();
                    prospectRequest.Channel = teleSalesChannels?.FirstOrDefault(x => x.Code != null && x.Code.Equals(prospectRequest.Channel))?.Id;
                }

                var result = await _serviceStore.CreateProspect(prospectRequest);

                return result.IsEmpty() ? SubmissionResponse.Error("2002", "Failed to create new prospect") : SubmissionResponse.Ok(result);
            }
            catch (Exception ex)
            {

                Logger.ApiLog(null, ex, nameof(CreateProspect), null);
                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<SubmissionResponse> CreateCustomerMessage(CustomerMessageRequest customerMessage)
        {

            customerMessage.CustomerId = await _serviceStore.GetContactIdByCif(customerMessage.CustomerId);

            if (customerMessage.CustomerId.IsEmpty())
            {
                return SubmissionResponse.Error("1000", "Invalid CIF");
            }

            var customerMessageTypesTask = GetCustomerMessageTypes();
            var customerMessageSubjectsTask = GetCustomerMessageSubjects();

            await Task.WhenAll(customerMessageTypesTask, customerMessageSubjectsTask);

            var customerMessageTypes = await customerMessageTypesTask;
            var customerMessageSubjects = await customerMessageSubjectsTask;

            customerMessage.CustomerMessageSubject = customerMessageSubjects.First(x => x.Code.Equals(customerMessage.CustomerMessageSubject)).Id;
            customerMessage.CustomerMessageType = customerMessageTypes.First(x => x.Code.Equals(customerMessage.CustomerMessageType)).Id;

            var result = await _serviceStore.CreateCustomerMessage(customerMessage);

            return result.IsEmpty() ? SubmissionResponse.Error("2003", "Failed to create new customer message") : SubmissionResponse.Ok(result);
        }

        public async Task<SubmissionResponse> CreateCustomerMessageReply(CustomerMessageReply customerMessageReply)
        {
            var relatedMessage = await _serviceStore.GetCustomerMessageById(customerMessageReply.RelatedMessageId);

            if (relatedMessage == null)
            {
                return SubmissionResponse.Error("1001", "Related message not found");
            }

            var customerId = await _serviceStore.GetCustomerIdByCustomerMessageId(customerMessageReply.RelatedMessageId);

            customerMessageReply.CustomerId = customerId;

            var result = await _serviceStore.CreateCustomerMessageReply(customerMessageReply, relatedMessage);

            return result.IsEmpty() ? SubmissionResponse.Error("2004", "Failed to create new customer message reply.") : SubmissionResponse.Ok(result);
        }

        public async Task<SubmissionResponse> GetCustomerMessagesByCustomerId(MessagType messagType, string customerId, int pageSize, int pageIndex, CultureInfo cultureInfo)
        {
            customerId = await _serviceStore.GetContactIdByCif(customerId);

            if (customerId.IsEmpty())
            {
                return SubmissionResponse.Error("1000", "Invalid CIF");
            }


            var result = await _serviceStore.GetCustomerMessages(messagType, customerId, pageSize, pageIndex);

            var statusCodes = await _serviceStore.GetStatusCodes(Constants.CustomerMessageLogicalName);

            if (!statusCodes.IsEmpty())
            {
                if (result.Item1 != null && result.Item1.Any())
                {
                    foreach (var item in result.Item1)
                    {
                        item.StatusName = statusCodes.FirstOrDefault(x => x.Code == item.Status).Name;
                    }
                }
            }

            if (result.Item1 != null && result.Item1.Any())
            {
                var finalResult = new
                {
                    CustomerMessages = result.Item1.Select(l => l.ToModel(cultureInfo)),
                    TotalCount = result.Item2,
                    TotalPages = (int)Math.Ceiling((double)result.Item2 / pageSize)
                };

                return SubmissionResponse.Ok(finalResult);
            }

            return SubmissionResponse.Ok("No customer messages found");
        }

        public async Task<SubmissionResponse> GetCustomerMessagesDetails(string customerId, string customerMessageId, CultureInfo cultureInfo)
        {
            customerId = await _serviceStore.GetContactIdByCif(customerId);

            if (customerId.IsEmpty())
            {
                return SubmissionResponse.Error("1000", "Invalid CIF");
            }

            var result = await _serviceStore.GetCustomerMessageDetails(customerId, customerMessageId);

            if (result == null)
            {
                return SubmissionResponse.Error("5000", "Internal error");
            }

            return SubmissionResponse.Ok(result.ToModel(cultureInfo));
        }

        public async Task<SubmissionResponse> DeleteCustomerMessage(DeleteCustomerMessageRequest request)
        {
            var isDeleted = await _serviceStore.DeleteCustomerMessage(request);

            if (isDeleted)
            {
                return SubmissionResponse.Ok();
            }

            return SubmissionResponse.Error("3000", "Failed to delete customer message");
        }

        public async Task<SubmissionResponse> GetSurveyTemplateByCode(string code, CultureInfo cultureInfo)
        {
            var result = await _serviceStore.GetSurveyTemplateByCode(code);

            return SubmissionResponse.Ok(result.ToModel(cultureInfo));
        }

        public async Task<SubmissionResponse> GetSurveyQuestionsBySurveyCode(string surveyTemplateCode, CultureInfo cultureInfo)
        {
            var result = await _serviceStore.GetSurveyQuestionsBySurveyCode(surveyTemplateCode);

            return SubmissionResponse.Ok(result.Select(l => l.ToModel(cultureInfo)));
        }

        public async Task<SubmissionResponse> GetSurveyAnswersByQuestionCode(string questionCode, CultureInfo cultureInfo)
        {
            var result = await _serviceStore.GetSurveyAnswersByQuestionCode(questionCode);

            return SubmissionResponse.Ok(result.Select(l => l.ToModel(cultureInfo)));
        }

        public async Task<SubmissionResponse> GetSurveyTemplateQA(string templateId)
        {
            var result = await _serviceStore.GetSurveyTemplateQA(templateId);

            var groupedQuestions = result
                                    .GroupBy(q => q.QuestionId)
                                    .Select(g => new SurveyQAViewModel
                                    {
                                        Id = g.Key,
                                        Name = g.First().QuestionName,
                                        Weight = g.First().QuestionWeight,
                                        Code = g.First().QuestionCode,
                                        Order = g.First().QuestionOrder,
                                        SurveyTemplateId = g.First().SurveyTemplateId,
                                        Answers = g.Select(q => new SurveyAnswerModel
                                        {
                                            Id = q.AnswerId,
                                            Name = q.AnswerName,
                                            Score = q.AnswerScore,
                                            Code = q.AnswerCode,
                                            Order = q.AnswerOrder,
                                            QuestionId = q.QuestionId,
                                        }).ToList()
                                    })
                                    .ToList();

            return SubmissionResponse.Ok(groupedQuestions);
        }

        public async Task<SubmissionResponse> CreateSurveyResponse(SurveyResponse surveyResponse)
        {
            try
            {
                surveyResponse.CustomerId = await _serviceStore.GetContactIdByLegalId(surveyResponse.CustomerId);

                if (surveyResponse.CustomerId.IsEmpty())
                {
                    return SubmissionResponse.Error("1000", "Invalid CIF");
                }

                var result = await _serviceStore.CreateSurveyResponse(surveyResponse);

                return result.IsEmpty() ? SubmissionResponse.Error("2005", "Failed to create survey response") : SubmissionResponse.Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SubmissionResponse> CreateSurveyResponseV2(SurveyResponse surveyResponse)
        {
            try
            {
                surveyResponse.CustomerId = await _serviceStore.GetContactIdByLegalId(surveyResponse.CustomerId);

                if (surveyResponse.CustomerId.IsEmpty() && !string.IsNullOrEmpty(_nonCustomerLegalId))
                {
                    surveyResponse.CustomerId = await _serviceStore.GetContactIdByLegalId(_nonCustomerLegalId);
                }

                if (surveyResponse.CustomerId.IsEmpty())
                {
                    return SubmissionResponse.Error("1000", "Invalid Legal Id");
                }

                var status = await _serviceStore.GetSurveyResponseStatus(surveyResponse.CaseNumberId);

                if (status != null && status == Constants.SubmittedSurveyResponseCode)
                {
                    return SubmissionResponse.Error("2005", "This case has an already submitted response");
                }

                var caseResolutionTime = await _serviceStore.GetCaseResolutionTime(surveyResponse.CaseNumberId);

                if (caseResolutionTime != null)
                {
                    var isSurveyExpired = CheckSurveyExpiry(caseResolutionTime);

                    if (isSurveyExpired)
                    {
                        return SubmissionResponse.Error("2005", $"Your response exceeds the {surveyResponseExpiryInterval} day window and the survey is no more available for completion");
                    }
                }

                var result = await _serviceStore.CreateSurveyResponseV2(surveyResponse);

                return result.IsEmpty() ? SubmissionResponse.Error("2005", "Failed to create survey response") : SubmissionResponse.Ok(result);

            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetTickets), null);
                return SubmissionResponse.Error("500", ex.Message);
            }
        }

        public async Task<SubmissionResponse> GetCallbackRequests(string cif, string callbackStatus)
        {
            try
            {
                var contactId = await _serviceStore.GetContactIdByCif(cif);

                if (contactId.IsEmpty())
                {
                    return SubmissionResponse.Error("1000", "Invalid CIF");
                }

                var callbacks = await _serviceStore.GetCallbackRequests(cif, callbackStatus);

                if (callbacks != null && !callbacks.IsEmpty())
                {
                    var timeSlots = await GetCallbackTimeslots();

                    foreach (var callback in callbacks)
                    {
                        if (callback.TimeSlotCode != null && !string.IsNullOrWhiteSpace(callback.TimeSlotCode))
                        {
                            callback.TimeSlot = timeSlots.FirstOrDefault(x => x.Code.Equals(callback.TimeSlotCode))?.Value;
                        }
                    }

                }

                return callbacks?.Any() == true ? SubmissionResponse.Ok(callbacks) : SubmissionResponse.Ok("No callbacks found");
            }

            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetCallbackRequests), null);

                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<SubmissionResponse> AssignCallbackRequest(CallbackRequest request)
        {
            try
            {
                var caseInsensitiveDict = new Dictionary<string, string>(domainMappings, StringComparer.OrdinalIgnoreCase);

                var contactId = await _serviceStore.GetContactIdByCif(request.Cif);

                if (contactId.IsNullOrEmpty())
                {
                    return SubmissionResponse.Error("1000", "Invalid CIF");
                }

                string rmId = "";

                string domain = "";

                (string localPart, string domainPart) = SplitEmailAddress(request.RmEmailAddress);

                if (caseInsensitiveDict.TryGetValue(domainPart, out string value))
                {
                    domain = value;
                }
                else
                {
                    return SubmissionResponse.Error("2000", "please add the specified domain to the configuration");
                }

                string firstName = "";

                string lastName = "";

                if (request.RmName.Contains(" "))
                {
                    int firstSpaceIndex = request.RmName.IndexOf(' ');

                    string firstPart = request.RmName.Substring(0, firstSpaceIndex);

                    string remainingPart = request.RmName.Substring(firstSpaceIndex + 1);

                    var splitRmName = request.RmName.Split(" ");

                    firstName = firstPart;

                    lastName = remainingPart;
                }
                else
                {
                    firstName = request.RmName;

                    lastName = "";
                }

                var rmDetails = new RmInternalDetails
                {
                    Domain = domain,
                    Name = localPart,
                    FirstName = firstName,
                    LastName = lastName,
                    Mobile = request.RmMobile,
                    Email = request.RmEmailAddress,
                    BusinessUnitId = businessUnitId,
                    TeamId = teamId
                };

                rmId = await _serviceStore.GetRmByEmail(request.RmEmailAddress);

                if (!rmId.IsEmpty())
                {
                    var isAlreadyAssociated = await _serviceStore.IsRmAssociatedWithTeam(rmDetails.TeamId, rmId);

                    if (!isAlreadyAssociated)
                    {
                        var associated = await _serviceStore.AssociateRmWithTeam(rmDetails.TeamId, rmId);

                        if (!associated)
                        {
                            return SubmissionResponse.Error("2000", $"Could not associate existing RM to team");
                        }
                    }
                }

                var callbacks = await _serviceStore.GetCallbackRequests(contactId, "1", rmId, request.CustomerChosenDate.ToString("yyyy-MM-dd"), request.TimeInterval);

                if (callbacks != null && callbacks.Any())
                {
                    return SubmissionResponse.Error("2000", "There is a callback request within the requested range");
                }

                if (rmId.IsEmpty())
                {
                    rmId = await _serviceStore.CreateRmRecord(rmDetails);

                    if (rmId.IsEmpty())
                    {
                        return SubmissionResponse.Error("2000", $"Could not create RM record for {request.RmName}");
                    }

                    var isTeamAssociated = await _serviceStore.AssociateRmWithTeam(rmDetails.TeamId, rmId);

                    if (!isTeamAssociated)
                    {
                        return SubmissionResponse.Error("2000", $"Could not associate RM to team");
                    }
                }

                request.InternalRmId = rmId;

                request.InternalUserId = contactId;

                var result = await _serviceStore.AssignCallbackRequest(request);

                return result.IsEmpty() ? SubmissionResponse.Error("2000", "Failed to assign a callback request") : SubmissionResponse.Ok(result);

            }
            catch (Exception ex)
            {
                Logger.ApiLog(request, ex, nameof(AssignCallbackRequest), null);

                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<SubmissionResponse> CreateAuthorizationRequest(AuthorizationRequest request)
        {
            try
            {
                if (request.Cif.IsNullOrEmpty() || request.ApplicationNo.IsNullOrEmpty())
                {
                    return SubmissionResponse.Error("1000", "Please make sure that all the request fields are sent");
                }

                var contactId = await _serviceStore.GetContactIdByCif(request.Cif);

                if (contactId.IsNullOrEmpty())
                {
                    return SubmissionResponse.Error("1000", "Invalid CIF");
                }

                request.ContactId = contactId;

                var result = await _serviceStore.CreateAuthorizationRequest(request);

                return result.IsEmpty() ? SubmissionResponse.Error("2000", "Failed to create an authorization request") : SubmissionResponse.Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(request, ex, nameof(CreateAuthorizationRequest), null);

                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<IEnumerable<EntitySpecificOptionSet>> GetCallbackTimeslots()
        {
            try
            {
                var timeSlots = await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.CallmebackTimeSlots,
                async () => await _serviceStore.GetCallbackTimeslots());

                return timeSlots;
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetCallbackTimeslots), null);

                throw;
            }
        }

        public async Task<IEnumerable<EntitySpecificOptionSet>> GetSkillGroups()
        {
            try
            {
                var skillGroups = await _cacheManager.GetOrAddCachedObjectAsync(CacheConstants.SkillGroups,
                async () => await _serviceStore.GetSkillGroups());

                return skillGroups;
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetSkillGroups), null);

                throw;
            }
        }

        public async Task<bool> CheckCifExistance(string cif)
        {
            try
            {
                var result = await _serviceStore.GetContactIdByCif(cif);

                if (result.IsNullOrEmpty())
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                Logger.ApiLog(null, ex, nameof(CheckCifExistance), null);

                throw;
            }
        }

        public async Task<SubmissionResponse> CreateCustomer(CustomerModel customer)
        {
            try
            {
                var segments = await _serviceStore.GetSegments();

                string segmentId = segments.FirstOrDefault(y => string.Equals(y.Code, customer.Segment, StringComparison.OrdinalIgnoreCase))?.Id;

                if (segmentId.IsNullOrEmpty())
                {
                    return SubmissionResponse.Error("1000", $"the sent segment with value {customer.Segment} is not available in CRM");
                }

                var customerId = await _serviceStore.GetContactIdByCif(customer.CIF);

                var languages = await _serviceStore.GetLanguages();

                customer.CustomerGuid = customerId;

                if (customer.PreferredLanguage == "2")
                {
                    customer.PreferredLanguage = "A";
                }
                else
                {
                    customer.PreferredLanguage = "E";
                }

                customer.PreferredLanguage = languages.FirstOrDefault(x => string.Equals(x.Code, customer.PreferredLanguage, StringComparison.OrdinalIgnoreCase)).Id;

                customer.Segment = segmentId;

                var createdCustomertId = await _serviceStore.CreateCustomer(customer);

                return createdCustomertId.IsEmpty() ? SubmissionResponse.Error("2000", "Failed to create a new contact") : SubmissionResponse.Ok(createdCustomertId);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(customer, ex, nameof(CreateCustomer), null);

                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        public async Task<SubmissionResponse> GetEmployeeRMs(string code)
        {
            try
            {
                var result = await _serviceStore.GetEmployeeRMs(code);

                if (result == null)
                {
                    return SubmissionResponse.Error("2000", "Employee with the specified code is not found");
                }

                return SubmissionResponse.Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetEmployeeRMs), null);
                return SubmissionResponse.Error("5000", ex.Message);
            }
        }

        /// <summary>
        /// Creates a text file in the shared folder named by the campaign's SkillGroup.
        /// Each line in the file follows the format:
        /// PhoneNumber,FullName,CampaignCode-ProductTypeName,CIF
        /// </summary>
        /// <param name="campaign">The campaign info containing the skill group, campaign code, and leads.</param>
        /// <param name="sharedFolderPath">The full path of the shared folder to save the file.</param>
        public async Task<SubmissionResponse> SaveCampaignInfoToFile(string campaignId)
        {
            try
            {
                // Fetch the first page.
                var campaign = await _serviceStore.GetCampaignInfo(campaignId, pageIndex: 1);
                if (campaign == null)
                    return SubmissionResponse.Error("1000", "Invalid campaignId");

                if (string.IsNullOrWhiteSpace(campaign.SkillGroup))
                    return SubmissionResponse.Error("2000", "Campaign missing skill group");


                var skillGroup = _skillGroups[campaign.SkillGroup];

                var campaignCode = campaign.CampaignCode;

                // Aggregate leads from all pages.
                var aggregatedLeads = new List<AutoDialerLeadInfo>();
                aggregatedLeads.AddRange(campaign.Leads);

                int pageIndex = 1;
                // If the current page has exactly 5000 leads, there might be additional pages.
                while (campaign.Leads.Count == 5000)
                {
                    pageIndex++;
                    var nextPageCampaign = await _serviceStore.GetCampaignInfo(campaignId, pageIndex);
                    if (nextPageCampaign == null)
                        break;

                    aggregatedLeads.AddRange(nextPageCampaign.Leads);
                    // Update the campaign variable to the latest response (if needed).
                    campaign = nextPageCampaign;
                }

                // Build the full file path using the SkillGroup as the file name.
                string fileName = $"{skillGroup}.txt";
                string filePath = Path.Combine(_autodialerSharedFolderPath, fileName);

                var cleanedAggregatedLeads = aggregatedLeads
                                             .DistinctBy(x => x.PhoneNumber)
                                             .ToList();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var lead in cleanedAggregatedLeads)
                    {
                        // Clean the phone number: remove KSA country code if present.
                        string cleanedPhone = CleanPhoneNumber(lead.PhoneNumber);

                        //skip the line if the phone number > 10 characters
                        if (cleanedPhone.Length > 10)
                            continue;

                        // Clean the full name: trim and limit to 50 characters.
                        string cleanedFullName = lead.FullName.SafeSubstring(50);
                        // Concatenate CampaignCode and ProductTypeName with a hyphen.
                        string campaignProduct = $"{campaignCode}-{lead.ProductTypeName}";
                        // Clean the camaign product: trim and limit to 50 characters.
                        string cleanedCampaignProduct = campaignProduct.SafeSubstring(50);

                        // Clean the LegalId: trim and limit to 30 characters.
                        string cleanedLegalId = lead.LegalId.SafeSubstring(30);

                        //string legalIdOrCif = string.IsNullOrWhiteSpace(lead.CIF) ? lead.LegalId : lead.CIF;

                        // Format the line (CIF may be empty).
                        string line = $"{cleanedPhone},{cleanedFullName},{cleanedCampaignProduct},{cleanedLegalId}";
                        writer.WriteLine(line);
                    }
                }

                return SubmissionResponse.Ok();
            }
            catch (Exception ex)
            {
                Logger.ApiLog(new { CampaignId = campaignId }, ex, nameof(SaveCampaignInfoToFile), null);
                return SubmissionResponse.Error("5000", ex.Message);
            }
        }
        private static IEnumerable<CaseViewModel> GroupCases(IEnumerable<Case> cases)
        {
            var groupedCases = cases?.GroupBy(x => x.CaseId)
                           .Select(c => new CaseViewModel
                           {
                               CaseId = c.Key,
                               AttachmentPath = c.First().AttachementPath,
                               CustomerId = c.First().CustomerId,
                               LegalId = c.First().LegalId,
                               CaseType = c.First().CaseType,
                               Cif = c.First().Cif,
                               CaseSubType = c.First().CaseSubType,
                               AccountNumber = c.First().AccountNumber,
                               Attachments = c.First().Attachments,
                               ClaimAmount = c.First().ClaimAmount,
                               TransactionAuthorizationCode = c.First().TransactionAuthorizationCode,
                               CallBackReason = c.First().CallBackReason,
                               CaseCategory = c.First().CaseCategory,
                               CaseNumber = c.First().CaseNumber,
                               CaseStatus = c.First().CaseStatus,
                               Status = c.First().Status,
                               CaseTitle = c.First().CaseTitle,
                               Department = c.First().Department,
                               Description = c.First().Description,
                               ClosedDate = c.First().ClosedDate,
                               Channel = c.First().Channel,
                               OpenedDate = c.First().OpenedDate,
                               TransactionDate = c.First().TransactionDate,
                               ClaimReason = c.First().ClaimReason,
                               CreatedBy = c.First().CreatedBy,
                               CustomerMobile = c.First().CustomerMobile,
                               CustomerName = c.First().CustomerName,
                               FinalReplyStatement = c.First().FinalReplyStatement,
                               IsEligibleForOfficialLetter = c.First().IsEligibleForOfficialLetter,
                               POSTerminalId = c.First().POSTerminalId,
                               PreferredCallBackTime = c.First().PreferredCallBackTime,
                               PreferredSMSLanguage = c.First().PreferredSMSLanguage,
                               Product = c.First().Product,
                               ReplyMethod = c.First().ReplyMethod,
                               TransactionCardType = c.First().TransactionCardType,
                               TransactionCurrency = c.First().TransactionCurrency,
                               TransactionMerchantId = c.First().TransactionMerchantId,
                               TransactionReference = c.First().TransactionReference,
                               AttachementSharePointUrl = c.First().AttachementSharePointUrl,
                               CaseSurveyResponses = c.First().SurveyResponseId.IsEmpty() ? null : c.GroupBy(k => k.SurveyResponseId)
                               .Select(s => new CaseSurveyResponse
                               {
                                   SurveyComment = s.First().SurveyComment,
                                   //SurveySubmissionDate = s.First().SurveySubmissionDate,
                                   SurveyReferenceId = s.First().SurveyReferenceId,
                                   SurveyResponseStatus = s.First().SurveyResponseStatus,
                                   SurveyResponseDetails = s.First().SurveyQuestionId.IsEmpty() ? new List<CaseSurveyResponseDetails>() : s.GroupBy(k => k.SurveyQuestionId)
                                   .Select(d => new CaseSurveyResponseDetails
                                   {
                                       SurveyQuestionName = d.First().SurveyQuestionName,
                                       SurveyAnswerName = d.First().SurveyAnswerName,
                                       SurveyTextAnswer = d.First().SurveyTextAnswer
                                   }).ToList(),
                               }).ToList(),
                               CaseSurveyTemplates = c.First().SurveyTemplateId.IsEmpty() ? null : c.GroupBy(s => s.SurveyTemplateId)
                               .Select(t => new CaseSurveyTemplate
                               {
                                   SurveyTemplateCode = t.First().SurveyTemplateCode,
                                   SurveyTemplateId = t.First().SurveyTemplateId,
                                   SurveyTemplateName = t.First().SurveyTemplateName,
                                   SurveyTemplateQuestions = t.First().SurveyTemplateQuestionId.IsEmpty() ? null : t.GroupBy(k => k.SurveyTemplateQuestionId)
                                   //SurveyTemplateQuestions = 
                                   .Select(a => new SurveyTemplateQuestion
                                   {
                                       SurveyTemplateQuestionId = a.First().SurveyTemplateQuestionId,
                                       SurveyTemplateQuestionCode = a.First().SurveyTemplateQuestionCode,
                                       SurveyTemplateQuestionName = a.First().SurveyTemplateQuestionName,
                                       SurveyTemplateQuestionOrder = a.First().SurveyTemplateQuestionOrder,
                                       SurveyTemplateQuestionWeight = a.First().SurveyTemplateQuestionWeight,
                                       SurveyTemplateAnswers = a.First().SurveyTemplateAnswerId.IsEmpty() ? null : a.GroupBy(k => k.SurveyTemplateAnswerId)
                                       //SurveyTemplateAnswers = 
                                       .Select(a => new SurveyTemplateAnswer
                                       {
                                           SurveyTemplateAnswerId = a.First().SurveyTemplateAnswerId,
                                           SurveyTemplateAnswerCode = a.First().SurveyTemplateAnswerCode,
                                           SurveyTemplateAnswerName = a.First().SurveyTemplateAnswerName,
                                           SurveyTemplateAnswerOrder = a.First().SurveyTemplateAnswerOrder,
                                           SurveyTemplateAnswerScore = a.First().SurveyTemplateAnswerScore,
                                           IsOther = a.First().IsOther,
                                       }).ToList(),
                                   }).ToList()
                               }).ToList()
                           });

            return groupedCases;
        }

        private TicketKeyFilter GetValuesFromKey(string key)
        {

            var result = new TicketKeyFilter();

            result.GuidLast4Digits = key.Substring(key.Length - 4, 4);
            result.CifLast4Digits = key.Substring(key.Length - 8, 4);
            result.CaseNumber = key.Remove(key.Length - 8);

            return result;
        }

        private async Task<IEnumerable<AttachmentInfo>> GetAttachmentsInfo(RecordType recordType, IEnumerable<string> recordIds)
        {
            var attachmentsInfo = await _serviceStore.GetAttachmentsInfo(recordType, recordIds);

            if (attachmentsInfo.IsEmpty())
            {
                return null;
            }

            return attachmentsInfo;
        }

        private (string, string) SplitEmailAddress(string email)
        {
            Regex regex = new Regex(@"^([^@]+)@([^@.]+)");
            Match match = regex.Match(email);
            if (match.Success)
            {
                string localPart = match.Groups[1].Value;
                string domainPart = match.Groups[2].Value;
                return (localPart, domainPart);
            }
            return (null, null);
        }

        private bool CheckSurveyExpiry(string caseCloseDate)
        {
            DateTime targetDateTime = DateTime.Parse(caseCloseDate);

            // Get today's date
            DateTime today = DateTime.Now;

            if (today.Date > targetDateTime.Date.AddDays(surveyResponseExpiryInterval))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsValueGuid(string key)
        {
            bool isValidGuid = Guid.TryParse(key, out Guid result);

            return isValidGuid;
        }

        private string ExtractCaseNumberFromKey(string key)
        {
            string extractedKey = key.Substring(0, key.Length - 4);

            return extractedKey;
        }

        /// <summary>
        /// Removes the KSA country code from the phone number if present.
        /// For numbers starting with "+966" or "00966", the prefix is removed and replaced with "0".
        /// </summary>
        /// <param name="phone">The original phone number.</param>
        /// <returns>The cleaned phone number.</returns>
        private static string CleanPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return phone;

            phone = phone.Trim();

            // Check for KSA country code prefixes
            if (phone.StartsWith("+966"))
            {
                // Remove "+966" and add a leading 0
                phone = "0" + phone.Substring(4);
            }
            else if (phone.StartsWith("00966"))
            {
                // Remove "00966" and add a leading 0
                phone = "0" + phone.Substring(5);
            }
            else if (phone.StartsWith("966"))
            {
                // Remove "00966" and add a leading 0
                phone = "0" + phone.Substring(3);
            }

            return phone;
        }




        public async Task<CollectionResult<TppTicketModel>> GetTppTickets(int pageSize, int pageIndex, string? tppId, string? tppUserId)
        {
            var result = await _serviceStore.GetTppTickets(pageSize, pageIndex, tppId, tppUserId);

            if (result.TotalCount > 0)
            {
                var bankEnvironmentOptions = (await GetBankEnvironments())?.ToList();
                var statusCodes = await _serviceStore.GetStatusCodes(Constants.StatusCodeOptionSet);

                foreach (var item in result.Items)
                {
                    if (!string.IsNullOrWhiteSpace(item.BankEnvironment))
                    {
                        var matchedEnv = bankEnvironmentOptions?
                            .FirstOrDefault(x => x.Code.Trim() == item.BankEnvironment.Trim());
                        item.BankEnvironment = matchedEnv?.Name;
                    }
                    else
                    {
                        item.BankEnvironment = null;
                    }

                    if (!string.IsNullOrWhiteSpace(item.StatusCode))
                    {
                        var matchedStatus = statusCodes?
                            .FirstOrDefault(x => x.Code.Trim() == item.StatusCode.Trim());

                        item.StatusCode = matchedStatus?.Name?["en"]?.Value;
                    }
                    else
                    {
                        item.StatusCode = null;
                    }
                }
            }

            return result;
        }

        public async Task<SubmissionResponse> CreateTppTicket(CreateTppTicketModel ticket)
        {
            var ticketId = await _serviceStore.CreateTppTicket(ticket);

            if (string.IsNullOrEmpty(ticketId))
                return SubmissionResponse.Error("Failed to create TPP ticket");

            return SubmissionResponse.Ok(new { Id = ticketId });
        }

        private async Task<UploadAttachmentResponse> UploadAttachment(UploadAttachmentRequest attachment, string requestId)
        {
            try
            {
                // Convert base64 string to byte array
                byte[] fileBytes = Convert.FromBase64String(attachment.Base64Data);

                // Save the attachment to the shared folder
                string fileName = attachment.FileName;
                string filePath = Path.Combine(_sharedFolderPath, requestId, fileName);
                Directory.CreateDirectory($@"{_sharedFolderPath}\{requestId}");
                await File.WriteAllBytesAsync(filePath, fileBytes);

                int lastIndex = filePath.LastIndexOf('\\');

                filePath = filePath.Substring(0, lastIndex);

                var result = new UploadAttachmentResponse
                {
                    Success = true,
                    ErrorMessage = null,
                    AttachmentName = fileName.RemoveExtension(),
                    FileName = fileName,
                    FilePath = filePath
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new UploadAttachmentResponse
                {
                    Success = false,
                    ErrorMessage = "Failed to upload attachment",

                };
                Logger.Log(ex, $"Failed to upload attachment for ticket: {requestId}");
                return result;
            }
        }


    }
}