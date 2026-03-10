using BabCrm.Service.Models;

namespace BabCrm.Service
{
    public interface IServiceStore
    {
        Task<string> WhoAmI();

        Task<IEnumerable<SamaLookupModel>> GetProducts();

        Task<IEnumerable<SamaLookupModel>> GetProducts(string channelCode);

        Task<IEnumerable<BaseLookupModel>> GetCategories();


        Task<IEnumerable<BaseLookupModel>> GetInternalProducts();

        Task<IEnumerable<BaseLookupModel>> GetLeadProducts();

        Task<IEnumerable<BaseLookupModel>> GetCategories(string channelCode);

        Task<IEnumerable<SamaLookupModel>> GetTicketTypes(string samaProductCode);

        Task<IEnumerable<Currency>> GetTransactionCurrencies();

        Task<IEnumerable<SamaLookupModel>> GetSubTypes();

        Task<IEnumerable<BaseLookupModel>> GetDepartments();

        Task<IEnumerable<BaseLookupModel>> GetCustomerMessageTypes();

        Task<IEnumerable<BaseLookupModel>> GetCustomerMessageSubjects();

        Task<IEnumerable<BaseLookupModel>> GetChannels();

        Task<IEnumerable<BaseLookupModel>> GetStatusCodes(string entityName);

        Task<string> GetContactIdByCif(string cif);

        Task<string> GetCaseResolutionTime(string caseId);

        Task<string> GetSurveyResponseStatus(string caseId);

        Task<ContactInfo> GetContactInfoByLegalId(string legalid);

        Task<ContactInfo> GetContactInfoByCif(string cif);

        Task<string> GetContactIdByLegalId(string legalid);

        Task<string> GetRmByEmail(string email);

        Task<string> CreateCustomer(CustomerModel customer);

        Task<IEnumerable<LookupModel>> GetCardTypes();
        Task<IEnumerable<LookupModel>> GetBankEnvironments();

        Task<IEnumerable<LookupModel>> GetNotificationChannels();

        Task<IEnumerable<Case>> GetTickets(string contactId, string channelId, TicketFilter filter, string language = "Ar");

        Task<IEnumerable<Case>> GetTicketByKey(string caseNumber, string cifLast4Digits, string guidLast4Digits, string language = "Ar");
        Task<IEnumerable<Case>> GetTicketByKey(string caseNumber, string caseGuid, string language = "Ar");

        Task<string> CreateNewTicket(TicketRequest ticket, string contactId);

        Task<string> UpdateTicketAttachmentPath(string attachmentPath, string requestId);


        Task<string> GetTicketNumberById(string ticketId);

        Task<string> GetCustomerIdByCustomerMessageId(string customerMessageId);

        Task<string> CreateProspect(ProspectRequestModel prospectRequest);

        Task<string> CreateCustomerMessage(CustomerMessageRequest customerMessage);

        Task<string> CreateCustomerMessageReply(CustomerMessageReply customerMessageReply, CustomerMessageResponse relatedMessage);

        Task<string> CreateAuthorizationRequest(AuthorizationRequest authorizationRequest);

        Task<(IEnumerable<CustomerMessageModel>, int)> GetCustomerMessages(MessagType messagType, string customerId, int pageSize, int pageIndex);

        Task<CustomerMessageDetails> GetCustomerMessageDetails(string customerId, string customerMessageId);

        Task<CustomerMessageResponse> GetCustomerMessageById(string customerMessageId);

        Task<bool> DeleteCustomerMessage(DeleteCustomerMessageRequest request);

        Task<SurveyTemplateModel> GetSurveyTemplateByCode(string code);

        Task<IEnumerable<SurveyQuestionModel>> GetSurveyQuestionsBySurveyCode(string surveyTemplateCode);

        Task<IEnumerable<SurveyAnswerModel>> GetSurveyAnswersByQuestionCode(string questionCode);

        Task<string> CreateSurveyResponse(SurveyResponse surveyResponse);

        Task<string> CreateSurveyResponseV2(SurveyResponse surveyResponse);

        //Task<IEnumerable<string>> AddAttachments(IEnumerable<Attachment> attachments);

        Task<IEnumerable<AttachmentInfo>> GetAttachmentsInfo(RecordType recordType, IEnumerable<string> recordIds);

        Task<IEnumerable<AttachmentInfo>> GetAttachmentsInfo(RecordType recordType, string recordId);

        Task<List<SurveyQA>> GetSurveyTemplateQA(string templateId);

        Task<IEnumerable<LookupModel>> GetLegalIdTypes();

        Task<IEnumerable<LookupModel>> GetMonthlySalaryOptions();

        Task<IEnumerable<LookupModel>> GetDividions();

        Task<IEnumerable<LookupModel>> GetPreferedTimeOptions();

        Task<IEnumerable<BaseLookupModel>> GetCountries();

        Task<IEnumerable<BaseLookupModel>> GetLanguages();

        Task<IEnumerable<BaseLookupModel>> GetSegments();

        Task<IEnumerable<BaseLookupModel>> GetTeleSalesChannels();

        Task<IEnumerable<CallbackRequestResponse>> GetCallbackRequests(string cif, string callbackStatus);

        Task<IEnumerable<CallbackRequestResponse>> GetCallbackRequests(string customerId, string callbackStatus, string rmId, string chosenDate, string timeInterval);

        Task<IEnumerable<EntitySpecificOptionSet>> GetCallbackTimeslots();

        Task<IEnumerable<EntitySpecificOptionSet>> GetSkillGroups();

        Task<string> AssignCallbackRequest(CallbackRequest request);
        Task<bool> IsRmAssociatedWithTeam(string teamId, string rmId);

        Task<string> CreateRmRecord(RmInternalDetails request);

        Task<string> CreateAccount(AccountModel account);

        Task<bool> AssociateRmWithTeam(string teamId, string rmId);

        Task<EmployeeRMs> GetEmployeeRMs(string code);

        Task<AutoDialerCampaignInfo> GetCampaignInfo(string campaignId, int pageIndex = 1);

        Task<IEnumerable<LookupModel>> GetTppTicketCategories();

        Task<IEnumerable<LookupModel>> GetTppTicketTypes(string categoryCode);

        Task<CollectionResult<TppTicketModel>> GetTppTickets(int pageSize, int pageIndex, string tppId, string tppUserId);

        Task<string> CreateTppTicket(CreateTppTicketModel ticket);

        Task<IEnumerable<LookupModel>> GetTicketStatusCodes();

    }

}
