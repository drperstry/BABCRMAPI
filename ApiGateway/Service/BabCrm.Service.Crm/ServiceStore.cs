using BabCrm.Core;
using BabCrm.Crm;
using BabCrm.Crm.Configuration;
using BabCrm.ObjectModel;
using BabCrm.Service.Crm.Models;
using BabCrm.Service.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Constants = BabCrm.Core.Constants;

namespace BabCrm.Service.Crm
{
    public class ServiceStore : IServiceStore
    {
        private readonly CrmService _crmService;
        private readonly ICrmConfig _crmConfig;
        private string attachementFilePath;
        private string _sharedFolderPath;
        private string isEligibleForOfficialLetter;
        private string isNotEligibleForOfficialLetter;
        private string _OwnerId;

        private readonly IEnumerable<CallbackTimeslots> callbackTimeslots;

        public ServiceStore(CrmService crmService, ICrmConfig crmConfig, IConfiguration configuration)
        {
            this._crmService = crmService;
            this._crmConfig = crmConfig;

            var attachementSection = configuration.GetSection("AttachementConfig");

            attachementFilePath = attachementSection["FilePath"];
            _sharedFolderPath = configuration.GetValue<string>("SharedFolderPath");

            isEligibleForOfficialLetter = attachementSection["IsEligibleForOfficialLetter"];

            isNotEligibleForOfficialLetter = attachementSection["IsNotEligibleForOfficialLetter"];

            callbackTimeslots = configuration.GetSection("CallbackTimeslots").Get<List<CallbackTimeslots>>();
            this._OwnerId = configuration.GetValue<string>("OwnerId");
        }

        public async Task<string> WhoAmI() => await _crmService.WhoAmI();

        public async Task<IEnumerable<SamaLookupModel>> GetProducts()
        {
            var query = $@"<fetch>
                              <entity name='ntw_samaproduct'>
                                <attribute name='ntw_samaproductid' alias='samaid' />
                                <attribute name='ntw_name' alias='samaname' />
                                <attribute name='ntw_code' alias='samacode' />
                                <attribute name='ntw_arabicname' alias='samaarabicname' />
                                <filter>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                                <order attribute='ntw_name' descending='false' />
                                <link-entity name='product' from='productid' to='ntw_internalproductid' visible='false' link-type='outer' alias='a_7bcb4e76b52eee11bafa00155d0f022d'>
                                  <attribute name='productid' alias='internalid' />
                                  <attribute name='name' alias='internalname' />
                                  <attribute name='ntw_arabicname' alias='internalarabicname' />
                                  <filter>
                                    <condition attribute='statecode' operator='eq' value='0' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            var products = await _crmService.Get(Core.Constants.SamaProductEntityName, query);

            return FillSamaLookup(products);
        }

        public async Task<IEnumerable<BaseLookupModel>> GetInternalProducts()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='product'>
                                <attribute name='name'/>
                                <attribute name='ntw_arabicname'/>
                                <attribute name='productid'/>
                                <attribute name='productnumber'/>
                                <order attribute='productnumber' descending='false'/>
                                </entity>
                          </fetch>";

            var categories = await _crmService.Get(Constants.ProductEntityName, query);

            return FillInternalProducts(categories);
        }

        public async Task<IEnumerable<BaseLookupModel>> GetLeadProducts()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='ntw_leadproduct'>
                                <attribute name='ntw_leadproductid'/>
                                <attribute name='ntw_name'/>
                                <attribute name='createdon'/>
                                <attribute name='ntw_typeset'/>
                                <attribute name='ntw_code'/>
                                <attribute name='ntw_arabicname'/>
                                <order attribute='ntw_name' descending='false'/>
                                </entity>
                           </fetch>";

            var leadProducts = await _crmService.Get(Constants.LeadProductEntityName, query);

            return FillLeadProducts(leadProducts);
        }

        public async Task<IEnumerable<SamaLookupModel>> GetProducts(string channelCode)
        {
            var query = $@"<fetch>
                              <entity name='ntw_samaproduct'>
                                <attribute name='ntw_samaproductid' alias='samaid' />
                                <attribute name='ntw_name' alias='samaname' />
                                <attribute name='ntw_code' alias='samacode' />
                                <attribute name='ntw_arabicname' alias='samaarabicname' />
                                <filter>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                                <order attribute='ntw_name' descending='false' />
                                {(!string.IsNullOrEmpty(channelCode) ? $@" <link-entity name='ntw_ntw_samaproduct_ntw_channel' from='ntw_samaproductid' to='ntw_samaproductid' visible='false' intersect='true'>
                                <link-entity name='ntw_channel' from='ntw_channelid' to='ntw_channelid' alias='ab'>
                                   <filter type='and'>
                                   <condition attribute='ntw_code' operator='eq' value='{channelCode}' />
                                   </filter>
                                    </link-entity>
                                    </link-entity>" : "")}
                                <link-entity name='product' from='productid' to='ntw_internalproductid' visible='false' link-type='outer' alias='a_7bcb4e76b52eee11bafa00155d0f022d'>
                                  <attribute name='productid' alias='internalid' />
                                  <attribute name='name' alias='internalname' />
                                  <attribute name='ntw_arabicname' alias='internalarabicname' />
                                  <filter>
                                    <condition attribute='statecode' operator='eq' value='0' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";


            var products = await _crmService.Get(Core.Constants.SamaProductEntityName, query);

            return FillSamaLookup(products);
        }

        public async Task<IEnumerable<BaseLookupModel>> GetCategories()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_requestcategory'>
                                <attribute name='ntw_name' />
                                <attribute name='ntw_arabicname' />
                                <attribute name='ntw_requestcategoryid' />
                                <attribute name='ntw_code' />
                                <order attribute='ntw_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ntw_code' operator='ne' value='ONLINE_REQUEST' />
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";

            var categories = await _crmService.Get(Constants.CategoryEntityName, query);

            return FillCategories(categories);
        }

        public async Task<IEnumerable<BaseLookupModel>> GetCategories(string channelCode)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                            <entity name='ntw_requestcategory'>
                            <attribute name='ntw_name' />
                            <attribute name='ntw_arabicname' />
                            <attribute name='ntw_requestcategoryid' />
                            <attribute name='ntw_code' />
                            <order attribute='ntw_name' descending='false' />
                            <filter type='and'>
                            <condition attribute='ntw_code' operator='ne' value='ONLINE_REQUEST' />
                             <condition attribute='statecode' operator='eq' value='0' />
                            </filter>
                           {(!string.IsNullOrEmpty(channelCode) ? $@" <link-entity name='ntw_ntw_requestcategory_ntw_channel' from='ntw_requestcategoryid' to='ntw_requestcategoryid' visible='false' intersect='true'>
                            <link-entity name='ntw_channel' from='ntw_channelid' to='ntw_channelid' alias='aa'>
                            <filter type='and'>
                            <condition attribute='ntw_code' operator='eq' value='{channelCode}' />
                            </filter>
                            </link-entity>
                            </link-entity>" : "")}
                            </entity>
                            </fetch>";

            var categories = await _crmService.Get(Constants.CategoryEntityName, query);

            return FillCategories(categories);
        }



        public async Task<IEnumerable<Currency>> GetTransactionCurrencies()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='transactioncurrency'>
                                <attribute name='transactioncurrencyid' />
                                <attribute name='currencyname' />
                                <attribute name='isocurrencycode' />
                                <attribute name='currencysymbol' />
                                <attribute name='exchangerate' />
                                <attribute name='currencyprecision' />
                                <order attribute='currencyname' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";

            var transactionCurrencies = await _crmService.Get(Constants.TransactionCurrencyEntityName, query);

            return FillTransactionCurrency(transactionCurrencies);
        }

        public async Task<IEnumerable<SamaLookupModel>> GetTicketTypes(string samaProductCode)
        {
            string samaProductFilter = string.Empty;
            if (!samaProductCode.IsEmpty())
            {
                samaProductFilter = $@" <link-entity name='ntw_samaproduct' from='ntw_samaproductid' to='ntw_samaproductid' link-type='inner' alias='ab'>
                                    <filter type='and'>
                                    <condition attribute='ntw_code' operator='eq' value='{samaProductCode}'/>
                                    </filter>
                                </link-entity>";
            }

            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_samatype'>
                                <attribute name='ntw_samatypeid' alias='samaid' />
                                <attribute name='ntw_name' alias='samaname' />
                                <attribute name='ntw_arabicname' alias='samaarabicname' />
                                <attribute name='ntw_code' alias='samacode' />
                                <filter>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                                <order attribute='ntw_name' descending='false' />
                               {samaProductFilter}
                                <link-entity name='ntw_requesttype' from='ntw_requesttypeid' to='ntw_internaltypeid' link-type='inner' alias='aa'>
                                  <attribute name='ntw_requesttypeid' alias='internalid' />
                                  <attribute name='ntw_name' alias='internalname' />
                                  <attribute name='ntw_arabicname' alias='internalarabicname' />
                                  <attribute name='ntw_code' alias='internalcode' />
                                  <attribute name='ntw_categoryid' alias='relatedrecordid' />
                                  <filter>
                                    <condition attribute='statecode' operator='eq' value='0' />
                                  </filter>
                                 <link-entity name='ntw_requestcategory' from='ntw_requestcategoryid' to='ntw_categoryid' link-type='outer'>
                                   <attribute name='ntw_code' alias='relatedrecordcode' />
                                 </link-entity>
                                </link-entity>
                              </entity>
                            </fetch>";

            var ticketTypes = await _crmService.Get(Core.Constants.SamaTicketTypeEntityName, query);

            return FillSamaLookup(ticketTypes);
        }

        public async Task<IEnumerable<BaseLookupModel>> GetDepartments()
        {

            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='ntw_department'>
                            <attribute name='ntw_departmentid' />
                            <attribute name='ntw_name' />
                            <attribute name='ntw_arabicname' />
                            <attribute name='createdon' />
                            <attribute name='ntw_code' />
                            <order attribute='ntw_name' descending='false' />
                            <filter type='and'>
                              <condition attribute='statecode' operator='eq' value='0' />
                            </filter>
                          </entity>
                        </fetch>";

            var departments = await _crmService.Get(Core.Constants.DepartmentEntityName, query);

            return FillLookup(departments, requestIdAttribute: "ntw_departmentid");
        }

        public async Task<IEnumerable<SamaLookupModel>> GetSubTypes()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_samasubtype'>
                                <attribute name='ntw_samasubtypeid' alias='samaid' />
                                <attribute name='ntw_name' alias='samaname' />
                                <attribute name='ntw_arabicname' alias='samaarabicname' />
                                <attribute name='ntw_code' alias='samacode' />
                                <attribute name='ntw_samatypeid' alias='relatedrecordid' />
                                <filter>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                                <order attribute='ntw_name' descending='false' />
                                <link-entity name='ntw_requestsubtype' from='ntw_requestsubtypeid' to='ntw_internalsubtypeid' visible='false' link-type='outer' alias='a_92ca50ecb52eee11bafa00155d0f022d'>
                                  <attribute name='ntw_requestsubtypeid' alias='internalid' />
                                  <attribute name='ntw_name' alias='internalname' />
                                  <attribute name='ntw_arabicname' alias='internalarabicname' />
                                  <attribute name='ntw_code' alias='internalcode' />
                                  <filter>
                                    <condition attribute='statecode' operator='eq' value='0' />
                                  </filter>
                                </link-entity>
                                <link-entity name='ntw_samatype' from='ntw_samatypeid' to='ntw_samatypeid' link-type='outer'>
                                  <attribute name='ntw_code' alias='relatedrecordcode' />
                                </link-entity>
                              </entity>
                            </fetch>";

            var ticketSubTypes = await _crmService.Get(Core.Constants.SamaTicketSubTypeEntityName, query);

            return FillSamaLookup(ticketSubTypes);
        }

        public async Task<IEnumerable<BaseLookupModel>> GetChannels()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_channel'>
                                <attribute name='ntw_channelid' />
                                <attribute name='ntw_name' />
                                <attribute name='ntw_code' />
                                <attribute name='ntw_arabicname' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                                <order attribute='ntw_name' descending='false' />
                              </entity>
                            </fetch>";

            var channels = await _crmService.Get(Core.Constants.ChannelEntityName, query);

            return FillLookup(channels, requestIdAttribute: "ntw_channelid");
        }

        public async Task<IEnumerable<BaseLookupModel>> GetStatusCodes(string entityName)
        {
            var result = await _crmService.GetStatusCodes(entityName);

            return FillLocalizedOptionSet(result);
        }

        public async Task<IEnumerable<BaseLookupModel>> GetCountries()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                             <entity name='ntw_country'>
                               <attribute name='ntw_countryid' />
                               <attribute name='ntw_name' />
                               <attribute name='ntw_code' />
                               <attribute name='ntw_arabicname' />
                               <order attribute='ntw_name' descending='false' />
                               <filter type='and'>
                                 <condition attribute='statecode' operator='eq' value='0' />
                               </filter>
                             </entity>
                           </fetch>";

            var countries = await _crmService.Get(Core.Constants.CountryEntityName, query);

            return FillLookup(countries, requestIdAttribute: "ntw_countryid");
        }

        public async Task<IEnumerable<BaseLookupModel>> GetLanguages()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_language'>
                                <attribute name='ntw_languageid' />
                                <attribute name='ntw_name' />
                                <attribute name='ntw_code' />
                                <attribute name='ntw_arabicname' />
                                <order attribute='ntw_name' descending='false' />
                                <filter type='and'>
                                 <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";

            var languages = await _crmService.Get(Core.Constants.LanguageEntityName, query);

            return FillLookup(languages, requestIdAttribute: "ntw_languageid");
        }

        public async Task<IEnumerable<BaseLookupModel>> GetSegments()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='ntw_customersegment'>
                                <attribute name='ntw_customersegmentid'/>
                                <attribute name='ntw_name'/>
                                <attribute name='ntw_code'/>
                                </entity>
                           </fetch>";

            var languages = await _crmService.Get(Core.Constants.SegmentEntity, query);

            return FillLookup(languages, requestIdAttribute: "ntw_customersegmentid");
        }

        public async Task<IEnumerable<BaseLookupModel>> GetTeleSalesChannels()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_telesaleschannel'>
                                <attribute name='ntw_telesaleschannelid' />
                                <attribute name='ntw_name' />
                                <attribute name='ntw_code' />
                                <attribute name='ntw_arabicname' />
                                <order attribute='ntw_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";

            var teleSalesChannels = await _crmService.Get(Core.Constants.TeleSalesChannelsEntityName, query);

            return FillLookup(teleSalesChannels, requestIdAttribute: "ntw_telesaleschannelid");
        }

        public async Task<IEnumerable<BaseLookupModel>> GetCustomerMessageTypes()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                             <entity name='ntw_customermessagetype'>
                               <attribute name='ntw_customermessagetypeid' />
                               <attribute name='ntw_name' />
                               <attribute name='ntw_arabicname' />
                               <attribute name='ntw_code' />
                               <order attribute='ntw_order' descending='false' />
                               <filter type='and'>
                                 <condition attribute='statecode' operator='eq' value='0' />
                               </filter>
                             </entity>
                           </fetch>";

            var customerMessageTypes = await _crmService.Get(Core.Constants.CustomerMessageTypeEntityName, query);

            return FillLookup(customerMessageTypes, requestIdAttribute: "ntw_customermessagetypeid");
        }

        public async Task<IEnumerable<BaseLookupModel>> GetCustomerMessageSubjects()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='ntw_customermessagesubject'>
                            <attribute name='ntw_customermessagesubjectid' />
                            <attribute name='ntw_name' />
                            <attribute name='ntw_arabicname' />
                            <attribute name='ntw_code' />
                            <order attribute='ntw_order' descending='false' />
                            <filter type='and'>
                              <condition attribute='statecode' operator='eq' value='0' />
                            </filter>
                          </entity>
                        </fetch>";

            var customerMessageSubjects = await _crmService.Get(Core.Constants.CustomerMessageSubjectEntityName, query);

            return FillLookup(customerMessageSubjects, requestIdAttribute: "ntw_customermessagesubjectid");
        }

        public async Task<IEnumerable<LookupModel>> GetCardTypes()
        {
            var result = await _crmService.GetOptionSet(Constants.CardTypeOptionSet);
            return FillOptionSet(result);
        }

        public async Task<IEnumerable<LookupModel>> GetNotificationChannels()
        {
            var result = await _crmService.GetOptionSet(Constants.NotificationChannelOptionSet);
            return FillOptionSet(result);
        }

        public async Task<IEnumerable<LookupModel>> GetLegalIdTypes()
        {
            var result = await _crmService.GetOptionSet(Constants.LegalTypeOptionSet);
            return FillOptionSet(result);
        }

        public async Task<IEnumerable<LookupModel>> GetMonthlySalaryOptions()
        {
            var result = await _crmService.GetOptionSet(Constants.MonthlySalaryOptionSet);
            return FillOptionSet(result);
        }

        public async Task<IEnumerable<LookupModel>> GetDividions()
        {
            var result = await _crmService.GetOptionSet(Constants.DivisionOptionSet);
            return FillOptionSet(result);
        }

        public async Task<IEnumerable<LookupModel>> GetPreferedTimeOptions()
        {
            var result = await _crmService.GetOptionSet(Constants.PreferedTimeOptionSet);
            return FillOptionSet(result);
        }

        public async Task<string> GetContactIdByCif(string cif)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='contact'>
                                <attribute name='contactid' />
                                <order attribute='fullname' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ntw_cif' operator='eq' value='{cif}' />
                                </filter>
                              </entity>
                            </fetch>";

            var data = await _crmService.Get(Core.Constants.ContactEntityName, query);

            if (data == null || !data.Any())
            {
                return null;
            }

            var id = data[0]?.GetValue<string>("contactid");

            return id;
        }

        public async Task<ContactInfo> GetContactInfoByLegalId(string legalid)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='contact'>
                            <attribute name='fullname' />
                            <attribute name='contactid' />
                            <attribute name='ntw_cif' />
                            <order attribute='fullname' descending='false' />
                              <link-entity name='ntw_language' from='ntw_languageid' to='ntw_preferredlanguageid'>
                                <attribute name='ntw_code' alias='langcode' />
                              </link-entity>
                            <filter type='and'>
                            <condition attribute='governmentid' operator='eq' value='{legalid}' />
                            </filter>
                            </entity>
                            </fetch>";

            var data = await _crmService.Get(Core.Constants.ContactEntityName, query);

            if (data == null || !data.Any())
            {
                return null;
            }

            var result = new ContactInfo
            {
                Cif = data[0]?.GetValue<string>("ntw_cif"),
                PrefferedLanguage = data[0]?.GetValue<string>("langcode")
            };

            return result;
        }

        public async Task<ContactInfo> GetContactInfoByCif(string cif)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='contact'>
                            <attribute name='contactid' />
                            <order attribute='fullname' descending='false' />
                              <link-entity name='ntw_language' from='ntw_languageid' to='ntw_preferredlanguageid' link-type='outer'>
                                <attribute name='ntw_code' alias='langcode' />
                              </link-entity>
                            <filter type='and'>
                             <condition attribute='ntw_cif' operator='eq' value='{cif}' />
                            </filter>
                            </entity>
                            </fetch>";

            var data = await _crmService.Get(Core.Constants.ContactEntityName, query);

            if (data == null || !data.Any())
            {
                return null;
            }

            var result = new ContactInfo
            {
                ContactId = data[0]?.GetValue<string>("contactid"),
                PrefferedLanguage = data[0]?.GetValue<string>("langcode")
            };

            return result;
        }

        public async Task<string> GetContactIdByLegalId(string legalid)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='contact'>
                            <attribute name='fullname' />
                            <attribute name='contactid' />
                            <order attribute='fullname' descending='false' />
                            <filter type='and'>
                            <condition attribute='governmentid' operator='eq' value='{legalid}' />
                            </filter>
                            </entity>
                            </fetch>";

            var data = await _crmService.Get(Core.Constants.ContactEntityName, query);

            if (data == null || !data.Any())
            {
                return null;
            }

            var id = data[0]?.GetValue<string>("contactid");

            return id;
        }

        public async Task<string> GetRmByEmail(string email)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='systemuser'>
                                <attribute name='systemuserid'/>
                                <order attribute='fullname' descending='false'/>
                                <filter type='and'>
                                <condition attribute='internalemailaddress' operator='eq' value='{email}'/>
                                </filter>
                                </entity>
                            </fetch>";

            var data = await _crmService.Get(Core.Constants.SystemUserEntityName, query);

            if (data == null || !data.Any())
            {
                return null;
            }

            var id = data[0]?.GetValue<string>("systemuserid");

            return id;
        }

        //TODO need to add status filter, to check with Ali if it's statecode or statuscode || i think ali mentioned that the status
        //will appear to the client with different values

        private static readonly HashSet<int> OpenStatusCodes = new HashSet<int>
{
    1, 2, 3, 4, 961110000, 961110001, 961110005
};

        private static readonly HashSet<int> ClosedStatusCodes = new HashSet<int>
{
    5, 961110002, 961110003, 961110004, 6, 2000, 1000
};

        public async Task<IEnumerable<Case>> GetTickets(string contactId, string channelId, TicketFilter filter, string language = "Ar")
        {
            var contactCondition = contactId.IsEmpty() ? string.Empty : $@"<condition attribute='customerid' operator='eq'  value='{contactId}' />";
            var nationalIdCondition = filter.NationalId.IsEmpty() ? string.Empty : $@"<condition attribute='ntw_idnumber' operator='eq'  value='{filter.NationalId}' />";
            var mobileNumberCondition = filter.MobileNumber.IsEmpty() ? string.Empty : $@"<condition attribute='ntw_mobilenumber' operator='eq'  value='{filter.MobileNumber}' />";
            var typeCondition = filter.TypeCode.IsEmpty() ? string.Empty : $@"<condition attribute='ntw_samatypeid' operator='eq' value='{filter.TypeCode}' />";
            var subTypeCondition = filter.SubTypeCode.IsEmpty() ? string.Empty : $@"<condition attribute='ntw_samasubtypeid' operator='eq' value='{filter.SubTypeCode}' />";
            var requestCategoryCondition = filter.CategoryCode.IsEmpty() ? string.Empty : $@"<condition attribute='ntw_categoryid' operator='eq'  value='{filter.CategoryCode}' />";
            var onOrAfterCondition = !filter.StartDate.HasValue ? string.Empty : $@"<condition attribute='createdon' operator='on-or-after' value='{filter.StartDate.Value.ToString("yyyy-MM-dd")}' />";
            var onOrBeforeCondition = !filter.EndDate.HasValue ? string.Empty : $@"<condition attribute='createdon' operator='on-or-before' value='{filter.EndDate.Value.ToString("yyyy-MM-dd")}' />";
            var caseNumberCondition = filter.CaseNumber.IsEmpty() ? string.Empty : $@"<condition attribute='ticketnumber' operator='eq'  value='{filter.CaseNumber}' />";

            if (filter.IsOpen.HasValue && !filter.StatusCode.IsEmpty())
            {
                var statusCode = int.Parse(filter.StatusCode);

                if (filter.IsOpen.Value && !OpenStatusCodes.Contains(statusCode))
                    return Enumerable.Empty<Case>();

                if (!filter.IsOpen.Value && !ClosedStatusCodes.Contains(statusCode))
                    return Enumerable.Empty<Case>();
            }


            string statusCondition = string.Empty;

            if (!filter.StatusCode.IsEmpty())
            {
                statusCondition = $@"<condition attribute='statuscode' operator='eq' value='{filter.StatusCode}' />";
            }
            else if (filter.IsOpen.HasValue)
            {
                var codes = filter.IsOpen.Value ? OpenStatusCodes : ClosedStatusCodes;

                var conditions = string.Join("", codes.Select(c =>
                    $@"<condition attribute='statuscode' operator='eq' value='{c}' />"));

                statusCondition = $@"<filter type='or'>{conditions}</filter>";
            }


            //<condition attribute='primarycontactid' operator='eq' value='{contactId}' />
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='incident'>
                                <attribute name='title' />
                                <attribute name='ticketnumber' />
                                <attribute name='incidentid' />
                                <attribute name='description' />
                                <attribute name='ntw_accountnumber' />
                                <attribute name='statuscode' />
                                <attribute name='statecode' />
                                <attribute name='createdon' />
                                <attribute name='ntw_transactionreference' />
                                <attribute name='ntw_transactionmerchantid' />
                                <attribute name='ntw_transactiondate' />
                                <attribute name='ntw_transactioncardtype' />
                                <attribute name='ntw_transactionauthorizationcode' />
                                <attribute name='ntw_replymethodset' />
                                <attribute name='ntw_preferredcallbacktime' />
                                <attribute name='ntw_posterminalid' />
                                <attribute name='ntw_finalreplystatement' />
                                <attribute name='ntw_iseligibleforofficialletter' />
                                <attribute name='ntw_isofficiallettergenerated' />
                                <attribute name='ntw_officialletterurl' />
                                <attribute name='ntw_officialletterfilename' />
                                <attribute name='ntw_claimreason' />
                                <attribute name='ntw_claimamount' />
                                <attribute name='ntw_callbackreason' />
                                <attribute name='ntw_submittedon' />
                                <attribute name='ntw_qualitycheckdate' />
                                <attribute name='ntw_cmscardreplacementnumber' />
                                <attribute name='ntw_feesapplied' />
                                <attribute name='ntw_transactioncode' />
                                <attribute name='ntw_transactiontype' />
                                <attribute name='ntw_callednumber' />
                                <attribute name='ntw_callernumber' />
                                <attribute name='ntw_attachmentspath' />
                                <attribute name='ntw_samatypeid' alias='samatypeid' />
                                <attribute name='ntw_samasubtypeid' alias='samasubtypeid' />
                                <attribute name='ntw_samaproductid' alias='samaproductid' />
                                <attribute name='ntw_departmentid' alias='departmentid' />
                                <attribute name='ntw_categoryid' alias='requestcategoryid' />
                                <attribute name='transactioncurrencyid' alias='currencyid' />
                                <attribute name='ntw_preferredsmslanguageid' alias='preferredsmslanguageid' />
                                <order attribute='title' descending='false' />
                                <filter type='and'>
                                  {contactCondition}
                                  {nationalIdCondition}
                                  {mobileNumberCondition}
                                  {typeCondition}
                                  {subTypeCondition}
                                  {requestCategoryCondition}
                                  {onOrAfterCondition}
                                  {onOrBeforeCondition}
                                  {caseNumberCondition}
                                  {statusCondition}
                                </filter>
                               <link-entity name='ntw_channel' from='ntw_channelid' to='ntw_sourceid' visible='false' link-type='outer' alias='a_b728'>
<attribute name='ntw_name' alias='channelnameen' />
<attribute name='ntw_arabicname' alias='channelnamear' />
</link-entity>
<link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_b13'>
<attribute name='mobilephone' alias='cutomermobile' />
<attribute name='fullname' alias='customerfullname' />
 <attribute name='governmentid' alias='legalid' />
<attribute name='contactid' alias='contactguid' />
</link-entity>
<link-entity name='ntw_surveytemplate' from='ntw_surveytemplateid' to='ntw_surveytemplateid' link-type='outer'>
<attribute name='ntw_surveytemplateid' alias='templateid' />
<attribute name='ntw_name' alias='templatenameen' />
<attribute name='ntw_arabicname' alias='templatenamear' />
<attribute name='ntw_code' alias='templatecode' />
 <filter type='and'>
    <condition attribute='statecode' operator='eq' value='1' />
  </filter>
<link-entity name='ntw_surveyquestion' from='ntw_surveytemplateid' to='ntw_surveytemplateid' link-type='outer'>
<attribute name='ntw_weight' alias='questionweight' />
<attribute name='ntw_surveyquestionid' alias='questionid' />
<attribute name='ntw_name' alias='questionnameen' />
<attribute name='ntw_arabicname' alias='questionnamear' />
<attribute name='ntw_code' alias='questioncode' />
<attribute name='ntw_order' alias='qestionorder' />
 <filter type='and'>
    <condition attribute='statecode' operator='eq' value='0' />
  </filter>
<link-entity name='ntw_surveyanswer' from='ntw_surveyquestionid' to='ntw_surveyquestionid' link-type='outer'>
<attribute name='ntw_score' alias='answerscore' />
<attribute name='ntw_surveyanswerid' alias='answerid' />
<attribute name='ntw_name' alias='answernameen' />
<attribute name='ntw_arabicname' alias='answernamear' />
<attribute name='ntw_code' alias='answercode' />
<attribute name='ntw_order' alias='answerorder' />
<attribute name='ntw_isother' alias='isother' />
 <filter type='and'>
    <condition attribute='statecode' operator='eq' value='0' />
  </filter>
</link-entity>
</link-entity>
</link-entity>
<link-entity name='ntw_surveyresponse' from='ntw_casenumberid' to='incidentid' link-type='outer'>
<attribute name='ntw_submissiondate' alias='surveysumissiondate' />
<attribute name='ntw_referenceid' alias='surveyreferenceid' />
<attribute name='ntw_comments' alias='surveycomments' />
<attribute name='ntw_surveyresponseid' alias='surveyresponseid' />
<link-entity name='ntw_surveyresponsedetail' from='ntw_surveyresponseid' to='ntw_surveyresponseid' link-type='outer'>
<attribute name='ntw_textanswer' alias='surveytextanswer' />
<link-entity name='ntw_surveyquestion' from='ntw_surveyquestionid' to='ntw_questionid' link-type='outer'>
<attribute name='ntw_name' alias='surveyquestionnameen' />
<attribute name='ntw_arabicname' alias='surveyquestionnamear' />
<attribute name='ntw_surveyquestionid' alias='surveyquestionid' />
</link-entity>
<link-entity name='ntw_surveyanswer' from='ntw_surveyanswerid' to='ntw_answerid' link-type='outer'>
<attribute name='ntw_name' alias='surveyanswernameen' />
<attribute name='ntw_arabicname' alias='surveyanswernamear' />
</link-entity>
</link-entity>
</link-entity>
</entity>
</fetch>";

            var batchRequest = new BatchRequest(_crmConfig);
            var batchRequestItem = new BatchRequestItem { EntityPluralName = Constants.IncidentEntityName, FetchXml = query };
            batchRequest.AddRequest(batchRequestItem);

            var tickets = await _crmService.ExecuteBatchRequest(batchRequest);

            return FillTickets(tickets.FirstOrDefault()?.Data, language);
        }

        public async Task<IEnumerable<Case>> GetTicketByKey(string caseNumber, string cifLast4Digits, string guidLast4Digits, string language = "Ar")
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='incident'>
                                <attribute name='title' />
                                <attribute name='ticketnumber' />
                                <attribute name='incidentid' />
                                <attribute name='description' />
                                <attribute name='ntw_accountnumber' />
                                <attribute name='statuscode' />
                                <attribute name='createdon' />
                                <attribute name='statecode' />
                                <attribute name='ntw_transactionreference' />
                                <attribute name='ntw_transactionmerchantid' />
                                <attribute name='ntw_transactiondate' />
                                <attribute name='ntw_transactioncardtype' />
                                <attribute name='ntw_transactionauthorizationcode' />
                                <attribute name='ntw_replymethodset' />
                                <attribute name='ntw_preferredcallbacktime' />
                                <attribute name='ntw_posterminalid' />
                                <attribute name='ntw_finalreplystatement' />
                                <attribute name='ntw_iseligibleforofficialletter' />
                                <attribute name='ntw_claimreason' />
                                <attribute name='ntw_claimamount' />
                                <attribute name='ntw_callbackreason' />
                                <attribute name='ntw_submittedon' />
                                <attribute name='ntw_qualitycheckdate' />
                                <attribute name='ntw_cmscardreplacementnumber' />
                                <attribute name='ntw_feesapplied' />
                                <attribute name='ntw_transactioncode' />
                                <attribute name='ntw_transactiontype' />
                                <attribute name='ntw_callednumber' />
                                <attribute name='ntw_callernumber' />
                                <attribute name='ntw_attachmentspath' />
                                <attribute name='ntw_samatypeid' alias='samatypeid' />
                                <attribute name='ntw_samasubtypeid' alias='samasubtypeid' />
                                <attribute name='ntw_samaproductid' alias='samaproductid' />
                                <attribute name='ntw_departmentid' alias='departmentid' />
                                <attribute name='ntw_categoryid' alias='requestcategoryid' />
                                <attribute name='transactioncurrencyid' alias='currencyid' />
                                <attribute name='ntw_preferredsmslanguageid' alias='preferredsmslanguageid' />
                                <order attribute='title' descending='false' />
                                <filter type='and'>
                                 <condition attribute='ticketnumber' operator='eq' value='{caseNumber}' />
                                </filter>
                                <link-entity name='contact' from='contactid' to='customerid' link-type='inner' alias='al'>
                                  <attribute name='ntw_cif' alias='customercif' />
                                  <attribute name='governmentid' alias='legalid' />
                                </link-entity>
                                <link-entity name='ntw_channel' from='ntw_channelid' to='ntw_sourceid' visible='false' link-type='outer' alias='a_b728'>
                                  <attribute name='ntw_name' alias='channelnameen' />
                                  <attribute name='ntw_arabicname' alias='channelnamear' />
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_b13'>
                                  <attribute name='mobilephone' alias='cutomermobile' />
                                  <attribute name='fullname' alias='customerfullname' />
                                </link-entity>
                                <link-entity name='ntw_surveytemplate' from='ntw_surveytemplateid' to='ntw_surveytemplateid' link-type='outer'>
                                  <attribute name='ntw_surveytemplateid' alias='templateid' />
                                  <attribute name='ntw_name' alias='templatenameen' />
                                  <attribute name='ntw_arabicname' alias='templatenamear' />
                                  <attribute name='ntw_code' alias='templatecode' />
<filter type='and'>
        <condition attribute='statecode' operator='eq' value='0' />
      </filter>
                                  <link-entity name='ntw_surveyquestion' from='ntw_surveytemplateid' to='ntw_surveytemplateid' link-type='outer'>
                                    <attribute name='ntw_weight' alias='questionweight' />
                                    <attribute name='ntw_surveyquestionid' alias='questionid' />
                                    <attribute name='ntw_name' alias='questionnameen' />
                                    <attribute name='ntw_arabicname' alias='questionnamear' />
                                    <attribute name='ntw_code' alias='questioncode' />
                                    <attribute name='ntw_order' alias='qestionorder' />
<filter type='and'>
          <condition attribute='statecode' operator='eq' value='0' />
        </filter>
                                    <link-entity name='ntw_surveyanswer' from='ntw_surveyquestionid' to='ntw_surveyquestionid' link-type='outer'>
                                      <attribute name='ntw_score' alias='answerscore' />
                                      <attribute name='ntw_surveyanswerid' alias='answerid' />
                                      <attribute name='ntw_name' alias='answernameen' />
                                      <attribute name='ntw_arabicname' alias='answernamear' />
                                      <attribute name='ntw_code' alias='answercode' />
                                      <attribute name='ntw_order' alias='answerorder' />
                                     <attribute name='ntw_isother' alias='isother' />
<filter type='and'>
            <condition attribute='statecode' operator='eq' value='0' />
          </filter>
                                    </link-entity>
                                  </link-entity>
                                </link-entity>
                                <link-entity name='ntw_surveyresponse' from='ntw_casenumberid' to='incidentid' link-type='outer'>
                                  <attribute name='ntw_submissiondate' alias='surveysumissiondate' />
                                  <attribute name='ntw_referenceid' alias='surveyreferenceid' />
                                  <attribute name='ntw_comments' alias='surveycomments' />
                                  <attribute name='ntw_surveyresponseid' alias='surveyresponseid' />
                                  <link-entity name='ntw_surveyresponsedetail' from='ntw_surveyresponseid' to='ntw_surveyresponseid' link-type='outer'>
                                    <attribute name='ntw_textanswer' alias='surveytextanswer' />
                                    <link-entity name='ntw_surveyquestion' from='ntw_surveyquestionid' to='ntw_questionid' link-type='outer'>
                                      <attribute name='ntw_name' alias='surveyquestionnameen' />
                                      <attribute name='ntw_arabicname' alias='surveyquestionnamear' />
                                      <attribute name='ntw_surveyquestionid' alias='surveyquestionid' />
                                    </link-entity>
                                    <link-entity name='ntw_surveyanswer' from='ntw_surveyanswerid' to='ntw_answerid' link-type='outer'>
                                      <attribute name='ntw_name' alias='surveyanswernameen' />
                                      <attribute name='ntw_arabicname' alias='surveyanswernamear' />
                                    </link-entity>
                                  </link-entity>
                                </link-entity>
                              </entity>
                            </fetch>";

            var batchRequest = new BatchRequest(_crmConfig);
            var batchRequestItem = new BatchRequestItem { EntityPluralName = Constants.IncidentEntityName, FetchXml = query };
            batchRequest.AddRequest(batchRequestItem);

            var ticket = await _crmService.ExecuteBatchRequest(batchRequest);

            return FillTickets(ticket.FirstOrDefault()?.Data, language);
        }

        public async Task<IEnumerable<Case>> GetTicketByKey(string caseNumber, string caseGuid, string language = "Ar")
        {
            string caseGuidCondition = $@"<condition attribute='incidentid' operator='eq' value='{caseGuid}' />";

            string caseNumberCondition = $@"<condition attribute='ticketnumber' operator='eq' value='{caseNumber}' />";

            var condition = caseNumber == null ? caseGuidCondition : caseNumberCondition;

            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='incident'>
                                <attribute name='title' />
                                <attribute name='ticketnumber' />
                                <attribute name='incidentid' />
                                <attribute name='description' />
                                <attribute name='ntw_accountnumber' />
                                <attribute name='statuscode' />
                                <attribute name='createdon' />
                                <attribute name='statecode' />
                                <attribute name='ntw_transactionreference' />
                                <attribute name='ntw_transactionmerchantid' />
                                <attribute name='ntw_transactiondate' />
                                <attribute name='ntw_transactioncardtype' />
                                <attribute name='ntw_transactionauthorizationcode' />
                                <attribute name='ntw_replymethodset' />
                                <attribute name='ntw_preferredcallbacktime' />
                                <attribute name='ntw_posterminalid' />
                                <attribute name='ntw_finalreplystatement' />
                                <attribute name='ntw_isexistingcustomer' />
                                <attribute name = 'ntw_mobilenumber' />
                                <attribute name='ntw_idnumber' alias='caselegalid' />
                                <attribute name='ntw_firstname' />
                                <attribute name='ntw_lastname' />
                                <attribute name='ntw_iseligibleforofficialletter' />
                                <attribute name='ntw_claimreason' />
                                <attribute name='ntw_claimamount' />
                                <attribute name='ntw_callbackreason' />
                                <attribute name='ntw_submittedon' />
                                <attribute name='ntw_qualitycheckdate' />
                                <attribute name='ntw_cmscardreplacementnumber' />
                                <attribute name='ntw_feesapplied' />
                                <attribute name='ntw_transactioncode' />
                                <attribute name='ntw_transactiontype' />
                                <attribute name='ntw_callednumber' />
                                <attribute name='ntw_attachmentspath' />
                                <attribute name='ntw_samatypeid' alias='samatypeid' />
                                <attribute name='ntw_samasubtypeid' alias='samasubtypeid' />
                                <attribute name='ntw_samaproductid' alias='samaproductid' />
                                <attribute name='ntw_departmentid' alias='departmentid' />
                                <attribute name='ntw_categoryid' alias='requestcategoryid' />
                                <attribute name='transactioncurrencyid' alias='currencyid' />
                                <attribute name='ntw_preferredsmslanguageid' alias='preferredsmslanguageid' />
                                <order attribute='title' descending='false' />
                                <filter type='and'>
                                 {condition}
                                </filter>
                                <link-entity name='contact' from='contactid' to='customerid' link-type='inner' alias='al'>
                                  <attribute name='ntw_cif' alias='customercif' />
                                  <attribute name='contactid' alias='contactguid' />
                                  <attribute name='governmentid' alias='legalid' />
                                </link-entity>
                                <link-entity name='ntw_channel' from='ntw_channelid' to='ntw_sourceid' visible='false' link-type='outer' alias='a_b728'>
                                  <attribute name='ntw_name' alias='channelnameen' />
                                  <attribute name='ntw_arabicname' alias='channelnamear' />
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_b13'>
                                  <attribute name='mobilephone' alias='cutomermobile' />
                                  <attribute name='fullname' alias='customerfullname' />
                                </link-entity>
                                <link-entity name='ntw_surveytemplate' from='ntw_surveytemplateid' to='ntw_surveytemplateid' link-type='outer'>
                                  <attribute name='ntw_surveytemplateid' alias='templateid' />
                                  <attribute name='ntw_name' alias='templatenameen' />
                                  <attribute name='ntw_arabicname' alias='templatenamear' />
                                  <attribute name='ntw_code' alias='templatecode' />
<filter type='and'>
            <condition attribute='statecode' operator='eq' value='1' />
          </filter>
                                  <link-entity name='ntw_surveyquestion' from='ntw_surveytemplateid' to='ntw_surveytemplateid' link-type='outer'>
                                    <attribute name='ntw_weight' alias='questionweight' />
                                    <attribute name='ntw_surveyquestionid' alias='questionid' />
                                    <attribute name='ntw_name' alias='questionnameen' />
                                    <attribute name='ntw_arabicname' alias='questionnamear' />
                                    <attribute name='ntw_code' alias='questioncode' />
                                    <attribute name='ntw_order' alias='qestionorder' />
<filter type='and'>
            <condition attribute='statecode' operator='eq' value='0' />
          </filter>
                                    <link-entity name='ntw_surveyanswer' from='ntw_surveyquestionid' to='ntw_surveyquestionid' link-type='outer'>
                                      <attribute name='ntw_score' alias='answerscore' />
                                      <attribute name='ntw_surveyanswerid' alias='answerid' />
                                      <attribute name='ntw_name' alias='answernameen' />
                                      <attribute name='ntw_arabicname' alias='answernamear' />
                                      <attribute name='ntw_code' alias='answercode' />
                                      <attribute name='ntw_order' alias='answerorder' />
                                     <attribute name='ntw_isother' alias='isother' />
<filter type='and'>
            <condition attribute='statecode' operator='eq' value='0' />
          </filter>
                                    </link-entity>
                                  </link-entity>
                                </link-entity>
                                <link-entity name='ntw_surveyresponse' from='ntw_casenumberid' to='incidentid' link-type='outer'>
                                  <attribute name='ntw_submissiondate' alias='surveysumissiondate' />
                                  <attribute name='ntw_referenceid' alias='surveyreferenceid' />
                                  <attribute name='ntw_comments' alias='surveycomments' />
                                  <attribute name='ntw_surveyresponseid' alias='surveyresponseid' />
                                  <attribute name='statuscode' alias='surveyresponsestatuscode' />
                                  <link-entity name='ntw_surveyresponsedetail' from='ntw_surveyresponseid' to='ntw_surveyresponseid' link-type='outer'>
                                    <attribute name='ntw_textanswer' alias='surveytextanswer' />
                                    <link-entity name='ntw_surveyquestion' from='ntw_surveyquestionid' to='ntw_questionid' link-type='outer'>
                                      <attribute name='ntw_name' alias='surveyquestionnameen' />
                                      <attribute name='ntw_arabicname' alias='surveyquestionnamear' />
                                      <attribute name='ntw_surveyquestionid' alias='surveyquestionid' />
                                    </link-entity>
                                    <link-entity name='ntw_surveyanswer' from='ntw_surveyanswerid' to='ntw_answerid' link-type='outer'>
                                      <attribute name='ntw_name' alias='surveyanswernameen' />
                                      <attribute name='ntw_arabicname' alias='surveyanswernamear' />
                                    </link-entity>
                                  </link-entity>
                                </link-entity>
                              </entity>
                            </fetch>";

            var batchRequest = new BatchRequest(_crmConfig);
            var batchRequestItem = new BatchRequestItem { EntityPluralName = Constants.IncidentEntityName, FetchXml = query };
            batchRequest.AddRequest(batchRequestItem);

            var ticket = await _crmService.ExecuteBatchRequest(batchRequest);

            return FillTickets(ticket.FirstOrDefault()?.Data, language);
        }

        public async Task<string> CreateNewTicket(TicketRequest ticket, string contactId)
        {
            var newTicket = new JObject
            {
                ["description"] = ticket.Description,
                ["ntw_firstname"] = ticket.FirstName,
                ["ntw_lastname"] = ticket.LastName,
                ["ntw_mobilenumber"] = ticket.Mobile,
                ["ntw_idnumber"] = ticket.GovernmentId,
                ["ntw_email"] = ticket.Email,
                ["ntw_SourceID@odata.bind"] = $"/{Constants.ChannelEntityName}({ticket.ChannelCode})",
                ["productid@odata.bind"] = $"/{Constants.ProductEntityName}({ticket.InternalProductId})",
                ["ntw_SubTypeID@odata.bind"] = $"/{Constants.TicketSubTypeEntityName}({ticket.InternalTicketSubTypeId})",
                ["ntw_TypeID@odata.bind"] = $"/{Constants.TicketTypeEntityName}({ticket.InternalTicketTypeId})",
                ["ntw_CategoryID@odata.bind"] = $"/{Constants.CategoryEntityName}({ticket.CategoryCode})",
                ["customerid_contact@odata.bind"] = $"/{Constants.ContactEntityName}({contactId})",
                ["ntw_SamaProductId@odata.bind"] = $"/{Constants.SamaProductEntityName}({ticket.ProductServiceId})",
                ["ntw_SamaTypeId@odata.bind"] = $"/{Constants.SamaTicketTypeEntityName}({ticket.TypeId})",
                ["ntw_SamaSubTypeId@odata.bind"] = $"/{Constants.SamaTicketSubTypeEntityName}({ticket.SubTypeId})",
                ["ntw_accountnumber"] = ticket.AccountNumber,
                ["ntw_transactionreference"] = ticket.TransactionReference,
                ["ntw_claimamount"] = ticket.ClaimAmount,
                ["title"] = ticket.CaseTitle,
                ["ntw_transactiondate"] = ticket.TransactionDate,
                ["ntw_claimreason"] = ticket.ClaimReason,
                ["ntw_posterminalid"] = ticket.POSTerminalId,
                ["ntw_transactionmerchantid"] = ticket.TransactionMerchantId,
                ["ntw_transactionauthorizationcode"] = ticket.TransactionAuthorizationCode,
                ["ntw_transactioncardtype"] = ticket.TransactionCardType,
                ["ntw_preferredcallbacktime"] = ticket.PreferredCallBackTime.HasValue ? ticket.PreferredCallBackTime.Value.ToLocalTime() : null,
                ["ntw_callbackreason"] = ticket.CallBackReason,
                ["ntw_finalreplystatement"] = ticket.FinalReplyStatement,
                ["ntw_iseligibleforofficialletter"] = ticket.IsEligibileForOfficialLetter,
                ["ntw_replymethodset"] = ticket.ReplyMethod,
                ["ntw_receivemethodset"] = ticket.ReceiveMethod,
                ["ntw_callernumber"] = ticket.CallerNumber,
                ["ntw_callednumber"] = ticket.CalledNumber,
                ["ntw_transactiontype"] = ticket.TransactionType,
                ["ntw_transactioncode"] = ticket.TransactionCode,
                ["ntw_feesapplied"] = ticket.IsFeesApplied,
                ["ntw_cmscardreplacementnumber"] = ticket.CmsCardReplacementNumber,
                //["ntw_attachmentspath"] = ticket.Attachment == null ? "" : Path.Combine(_sharedFolderPath, ticket.Attachment?.FileName),
                //["ntw_spattachmenturl"] = ticket.CmsCardReplacementNumber,
            };

            if (!ticket.DepartmentId.IsEmpty())
            {
                newTicket["ntw_DepartmentID@odata.bind"] = $"/{Constants.DepartmentEntityName}({ticket.DepartmentId})";
            }

            if (!ticket.TransactionCurrencyId.IsEmpty())
            {
                newTicket["transactioncurrencyid@odata.bind"] = $"/{Constants.TransactionCurrencyEntityName}({ticket.TransactionCurrencyId})";
            }

            if (!ticket.PreferredSMSLanguage.IsEmpty())
            {
                newTicket["ntw_PreferredSMSLanguageID@odata.bind"] = $"/{Constants.LanguageEntityName}({ticket.PreferredSMSLanguage})";
            }

            var result = await _crmService.Save(Constants.IncidentEntityName, newTicket);

            return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
        }

        public async Task<string> UpdateTicketAttachmentPath(string attachmentPath, string requestId)
        {
            var ticket = new JObject
            {
                ["ntw_attachmentspath"] = attachmentPath,
                //["ntw_spattachmenturl"] = ticket.CmsCardReplacementNumber,
            };



            var result = await _crmService.Save(Constants.IncidentEntityName, ticket, requestId);

            return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
        }

        public async Task<string> GetTicketNumberById(string ticketId)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='incident'>
                                <attribute name='ticketnumber' />
                                <order attribute='title' descending='false' />
                                <filter type='and'>
                                  <condition attribute='incidentid' operator='eq' value='{ticketId}' />
                                </filter>
                              </entity>
                            </fetch>";

            var data = await _crmService.Get(Constants.IncidentEntityName, query);


            if (data == null || !data.Any())
            {
                return null;
            }

            var ticketNumber = data[0]?.GetValue<string>("ticketnumber");

            return ticketNumber;
        }

        public async Task<string> CreateProspect(ProspectRequestModel prospectRequest)
        {

            var prospect = new JObject
            {
                ["ntw_idnumber"] = prospectRequest.IdNumber,
                ["firstname"] = prospectRequest.FirstName,
                ["lastname"] = prospectRequest.LastName,
                ["ntw_employername"] = prospectRequest.EmployerName,
                ["ntw_age"] = prospectRequest.Age,
                ["address1_city"] = prospectRequest.City,
                ["ntw_monthlysalaryset"] = prospectRequest.MonthlySalary,
                ["ntw_nearestbranch"] = prospectRequest.NearestBranch,
                ["mobilephone"] = prospectRequest.MobilePhone,
                ["emailaddress1"] = prospectRequest.Email,
                ["ntw_preferredtimetocontactyouset"] = prospectRequest.PreferedTimeToContactYou,
                ["ntw_legalidtypeset"] = prospectRequest.LegalIDType,
                ["ntw_birthdate"] = prospectRequest.BirthDate,
                ["subject"] = prospectRequest.Topic,
                //["ntw_monthlysalaryset"] = prospectRequest.Salary,
                ["ntw_divisionset"] = prospectRequest.Division,
                ["address1_city"] = prospectRequest.RealEstateCity,
                ["address1_line2"] = prospectRequest.RealEstateDistrict,
                ["ntw_employername"] = prospectRequest.EmployerName,
                ["ntw_externalchannel"] = prospectRequest.ExternalChannel
            };

            if (!prospectRequest.PreferredProduct.IsEmpty())
            {
                prospect["ntw_PreferredLeadProductID@odata.bind"] = $"/{Constants.ProspectEntityName}({prospectRequest.PreferredProduct})";
            }

            if (!prospectRequest.Nationality.IsEmpty())
            {
                prospect["ntw_NationalityID@odata.bind"] = $"/{Constants.CountryEntityName}({prospectRequest.Nationality})";
            }

            if (!prospectRequest.Channel.IsEmpty())
            {
                prospect["ntw_TeleSalesChannelID@odata.bind"] = $"/{Constants.TeleSalesChannelsEntityName}({prospectRequest.Channel})";
            }

            var result = await _crmService.Save(Constants.ProspectEntityName, prospect);

            return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
        }

        public async Task<string> CreateCustomerMessage(CustomerMessageRequest customerMessage)
        {
            var newCustomerMessage = new JObject
            {
                ["ntw_subject"] = customerMessage.Subject,
                ["ntw_body"] = customerMessage.Body,
                ["ntw_CustomerID@odata.bind"] = $"/{Constants.ContactEntityName}({customerMessage.CustomerId})",
                ["ntw_direction"] = false
            };

            if (!customerMessage.CustomerMessageSubject.IsEmpty())
            {
                newCustomerMessage["ntw_CustomerMessageSubjectId@odata.bind"] = $"/{Constants.CustomerMessageSubjectEntityName}({customerMessage.CustomerMessageSubject})";
            }

            if (!customerMessage.CustomerMessageType.IsEmpty())
            {
                newCustomerMessage["ntw_CustomerMessageTypeId@odata.bind"] = $"/{Constants.CustomerMessageTypeEntityName}({customerMessage.CustomerMessageType})";
            }

            var result = await _crmService.Save(Constants.CustomerMessageEntityName, newCustomerMessage);

            return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
        }

        public async Task<string> CreateCustomerMessageReply(CustomerMessageReply customerMessageReply, CustomerMessageResponse relatedMessage)
        {
            var newCustomerMessage = new JObject
            {
                ["ntw_subject"] = relatedMessage.Subject,
                ["ntw_body"] = $"{relatedMessage.Body} \n--------------------------\n {customerMessageReply.ReplyMessage}",
                ["statecode"] = 0,
                ["statuscode"] = 1,
                ["ntw_direction"] = false,
                //["new_replymessage"] = customerMessageReply.ReplyMessage,
                ["ntw_CustomerID@odata.bind"] = $"/{Constants.ContactEntityName}({customerMessageReply.CustomerId})",
                ["ntw_RelatedMessageID@odata.bind"] = $"/{Constants.CustomerMessageEntityName}({customerMessageReply.RelatedMessageId})"
            };

            if (!relatedMessage.CustomerMessageSubject.IsEmpty())
            {
                newCustomerMessage["ntw_CustomerMessageSubjectId@odata.bind"] = $"/{Constants.CustomerMessageSubjectEntityName}({relatedMessage.CustomerMessageSubject})";
            }

            if (!relatedMessage.CustomerMessageType.IsEmpty())
            {
                newCustomerMessage["ntw_CustomerMessageTypeId@odata.bind"] = $"/{Constants.CustomerMessageTypeEntityName}({relatedMessage.CustomerMessageType})";
            }

            var result = await _crmService.Save(Constants.CustomerMessageEntityName, newCustomerMessage);

            return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;

        }

        public async Task<(IEnumerable<CustomerMessageModel>, int)> GetCustomerMessages(MessagType messagType, string customerId, int pageSize, int pageIndex)
        {
            var messageTypeCondition = "";

            if (messagType == MessagType.Received)
            {
                messageTypeCondition = $@"<condition attribute='ntw_direction' operator='eq' value='true' />";
            }

            if (messagType == MessagType.Sent)
            {
                messageTypeCondition = $@"<condition attribute='ntw_direction' operator='eq' value='false' />";
            }

            var customerMessagesQuery = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='{pageSize}' page='{pageIndex}'>
                              <entity name='ntw_customermessage'>
                                <attribute name='ntw_customermessageid' />
                                <attribute name='statuscode' />
                                <attribute name='createdon' />
                                <attribute name='ntw_referenceid' />
                                <attribute name='ntw_subject' />
                                <attribute name='new_replymessage' />
                                <attribute name='ntw_body' />
                                <attribute name='ntw_customerid' alias='customerid' />
                                <order attribute='ntw_referenceid' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ntw_customerid' operator='eq'  uitype='contact' value='{customerId}' />
                                  {messageTypeCondition}
                                </filter>
                                <link-entity name='contact' from='contactid' to='ntw_customerid' visible='false' link-type='outer' alias='cust'>
                                  <attribute name='fullname' alias='customerfullname' />
                                </link-entity>
                                <link-entity name='ntw_customermessagesubject' from='ntw_customermessagesubjectid' to='ntw_customermessagesubjectid' visible='false' link-type='outer' alias='a_c791'>
                                  <attribute name='ntw_name' alias='customermessagesubjecten' />
                                  <attribute name='ntw_arabicname' alias='customermessagesubjectar' />
                                </link-entity>
                                <link-entity name='ntw_customermessagetype' from='ntw_customermessagetypeid' to='ntw_customermessagetypeid' visible='false' link-type='outer' alias='a_1fb85'>
                                  <attribute name='ntw_name' alias='customermessagetypeen' />
                                  <attribute name='ntw_arabicname' alias='customermessagetypear' />
                                </link-entity>
                              </entity>
                            </fetch>";

            var customerMessagesCountQuery = $@"<fetch aggregate='true'>
                              <entity name='ntw_customermessage'>
                                <attribute name='ntw_customermessageid' alias='count' aggregate='countcolumn'  />
                                <filter type='and'>
                                  <condition attribute='ntw_customerid' operator='eq'  uitype='contact' value='{customerId}' />
                                  {messageTypeCondition}
                                </filter>
                              </entity>
                            </fetch>";


            BatchRequestItem customerMessagesBatchRequestItem = new BatchRequestItem { EntityPluralName = Constants.CustomerMessageEntityName, FetchXml = customerMessagesQuery };
            BatchRequestItem customerMessagesCountBatchRequestItem = new BatchRequestItem { EntityPluralName = Constants.CustomerMessageEntityName, FetchXml = customerMessagesCountQuery };
            BatchRequest customerMessagesBatchRequest = new BatchRequest(_crmConfig);
            customerMessagesBatchRequest.AddRequest(customerMessagesBatchRequestItem).AddRequest(customerMessagesCountBatchRequestItem);


            var batchResponse = await _crmService.ExecuteBatchRequest(customerMessagesBatchRequest);

            if (batchResponse != null && !batchResponse.IsEmpty())
            {
                var customerMessages = FillCustomerMessages(batchResponse.First().Data);
                var customerMessagesCount = batchResponse.ElementAt(1).Data[0].GetValue<int>("count");

                return (customerMessages, customerMessagesCount);
            }

            return (null, 0);
        }

        public async Task<CustomerMessageDetails> GetCustomerMessageDetails(string customerId, string customerMessageId)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_customermessage'>
                                <attribute name='ntw_customermessageid' />
                                <attribute name='ntw_referenceid' />
                                <attribute name='ntw_subject' />
                                <attribute name='new_replymessage' />
                                <attribute name='ntw_body' />
                                <attribute name='createdon' />
                                <attribute name='ntw_customerid' alias='customerid' />
                                <attribute name='statuscode' />
                                <attribute name='ntw_replydate' />
                                <attribute name='ntw_repliedbyid' alias='repliedbyid' />
                                <attribute name='ntw_readdate' />
                                <attribute name='ntw_readbyid' alias='readbyid' />
                                <attribute name='ntw_convertedtocasebyid' alias='convertedtocasebyid' />
                                <attribute name='ntw_converttocasedate' />
                                <order attribute='ntw_referenceid' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ntw_customerid' operator='eq' uitype='contact' value='{customerId}' />
                                  <condition attribute='ntw_customermessageid' operator='eq' uitype='contact' value='{customerMessageId}' />
                                 
                                </filter>
                                <link-entity name='contact' from='contactid' to='ntw_customerid' visible='false' link-type='outer' alias='a_2d2f8787c6c3ed11baef00155d0f022d'>
                                  <attribute name='fullname' alias='customerfullname' />
                                </link-entity>
                                <link-entity name='systemuser' from='systemuserid' to='ntw_convertedtocasebyid' visible='false' link-type='outer' alias='a_ff580cdcc6c3ed11baef00155d0f022d'>
                                  <attribute name='fullname' alias='convertedtocasebyfullname' />
                                </link-entity>
                                <link-entity name='systemuser' from='systemuserid' to='ntw_readbyid' visible='false' link-type='outer' alias='a_d1009bb6c6c3ed11baef00155d0f022d'>
                                  <attribute name='fullname' alias='readbyfullname' />
                                </link-entity>
                                <link-entity name='systemuser' from='systemuserid' to='ntw_repliedbyid' visible='false' link-type='outer' alias='a_882694c6c6c3ed11baef00155d0f022d'>
                                  <attribute name='fullname' alias='repliedfullname' />
                                </link-entity>
                                <link-entity name='ntw_customermessagesubject' from='ntw_customermessagesubjectid' to='ntw_customermessagesubjectid' visible='false' link-type='outer' alias='a_c791'>
                                  <attribute name='ntw_name' alias='customermessagesubjecten' />
                                  <attribute name='ntw_arabicname' alias='customermessagesubjectar' />
                                </link-entity>
                                <link-entity name='ntw_customermessagetype' from='ntw_customermessagetypeid' to='ntw_customermessagetypeid' visible='false' link-type='outer' alias='a_1fb85'>
                                  <attribute name='ntw_name' alias='customermessagetypeen' />
                                  <attribute name='ntw_arabicname' alias='customermessagetypear' />
                                </link-entity>
                              </entity>
                            </fetch>";

            var customerMessageDetails = await _crmService.Get(Constants.CustomerMessageEntityName, query);

            return FillCustomerMessageDetails(customerMessageDetails);
        }

        public async Task<CustomerMessageResponse> GetCustomerMessageById(string customerMessageId)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_customermessage'>
                                <attribute name='ntw_customermessageid' />
                                <attribute name='ntw_subject' />
                                <attribute name='ntw_body' />
                                <attribute name='ntw_customermessagetypeid' alias='customermessagetypeid' />
                                <attribute name='ntw_customermessagesubjectid' alias='customermessagesubjectid' />
                                <order attribute='ntw_referenceid' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ntw_customermessageid' operator='eq' uitype='contact' value='{customerMessageId}' />
                                </filter>
                              </entity>
                            </fetch>";

            var customerMessageDetails = await _crmService.Get(Constants.CustomerMessageEntityName, query);

            return FillCustomerMessage(customerMessageDetails);
        }

        public async Task<bool> DeleteCustomerMessage(DeleteCustomerMessageRequest request)
        {
            var batchRequest = new ChangeSetBatchRequest(_crmConfig);

            foreach (var id in request.CustomerMessageIds)
            {
                var batchRequestItem = new DeleteBatchRequestItem
                {
                    EntityPluralName = Constants.CustomerMessageEntityName,
                    RequestId = id
                };

                batchRequest.AddDeleteRequest(batchRequestItem);
            }

            var result = await _crmService.ExecuteChangeSetBatchRequest(batchRequest);

            return result != null;
        }

        public async Task<SurveyTemplateModel> GetSurveyTemplateByCode(string code)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_surveytemplate'>
                                <attribute name='ntw_surveytemplateid' />
                                <attribute name='ntw_name' />
                                <attribute name='ntw_arabicname' />
                                <attribute name='ntw_description' />
                                <attribute name='ntw_code' />
                                <order attribute='ntw_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ntw_code' operator='eq' value='{code}' />
                                </filter>
                              </entity>
                            </fetch>";

            var surveyTemplate = await _crmService.Get(Constants.SurveyTemplateEntityName, query);

            return FillSurveyTemplate(surveyTemplate);
        }

        public async Task<IEnumerable<SurveyQuestionModel>> GetSurveyQuestionsBySurveyCode(string surveyTemplateCode)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_surveyquestion'>
                                <attribute name='ntw_surveyquestionid' />
                                <attribute name='ntw_name' />
                                <attribute name='ntw_arabicname' />
                                <attribute name='ntw_weight' />
                                <attribute name='ntw_surveytemplateid' alias='templateid' />
                                <attribute name='ntw_code' />
                                <attribute name='ntw_order' />
                                <order attribute='ntw_order' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                                <link-entity name='ntw_surveytemplate' from='ntw_surveytemplateid' to='ntw_surveytemplateid' link-type='inner'>
                                  <filter>
                                    <condition attribute='ntw_code' operator='eq' value='{surveyTemplateCode}' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            var surveyQuestions = await _crmService.Get(Constants.SurveyQuestionEntityName, query);

            return FillSurveyQuestions(surveyQuestions);
        }

        public async Task<IEnumerable<SurveyAnswerModel>> GetSurveyAnswersByQuestionCode(string questionCode) //GetSurveyAnswersByQuestionId
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_surveyanswer'>
                                <attribute name='ntw_surveyanswerid' />
                                <attribute name='ntw_name' />
                                <attribute name='ntw_arabicname' />
                                <attribute name='ntw_surveyquestionid' alias='surveyquestionid' />
                                <attribute name='ntw_score' />
                                <attribute name='ntw_isother' />
                                <attribute name='ntw_order' />
                                <attribute name='ntw_code' />
                                <order attribute='ntw_order' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                                <link-entity name='ntw_surveyquestion' from='ntw_surveyquestionid' to='ntw_surveyquestionid' link-type='inner'>
                                  <filter>
                                    <condition attribute='ntw_code' operator='eq' value='{questionCode}' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            var surveyAnswers = await _crmService.Get(Constants.SurveyAnswerEntityName, query);

            return FillSurveyAnswers(surveyAnswers);
        }

        public async Task<List<SurveyQA>> GetSurveyTemplateQA(string templateId)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_surveyquestion'>
                                <attribute name='ntw_surveyquestionid' alias='questionid' />
                                <attribute name='ntw_name' alias='questionnameen' />
                                <attribute name='ntw_arabicname' alias='questionnamear' />
                                <attribute name='ntw_weight' alias='weight' />
                                <attribute name='ntw_surveytemplateid' alias='templateid' />
                                <attribute name='ntw_code' alias='qestioncode' />
                                <attribute name='ntw_order' alias='questionorder' />
                                <order attribute='ntw_order' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                  <condition attribute='ntw_surveytemplateid' operator='eq' uitype='ntw_surveytemplate' value='{templateId}' />
                                </filter>
                                <link-entity name='ntw_surveyanswer' from='ntw_surveyquestionid' to='ntw_surveyquestionid'>
                                  <attribute name='ntw_surveyanswerid' alias='answerid' />
                                  <attribute name='ntw_name' alias='answernameen' />
                                  <attribute name='ntw_arabicname' alias='answernamear' />
                                  <attribute name='ntw_surveyquestionid' alias='surveyquestionid' />
                                  <attribute name='ntw_score' alias='answerscore' />
                                  <attribute name='ntw_order' alias='answerorder' />
                                  <attribute name='ntw_code' alias='answercode' />
                                  <filter type='and'>
                                    <condition attribute='statecode' operator='eq' value='0' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            var surveyAnswers = await _crmService.Get(Constants.SurveyQuestionEntityName, query);

            return FillSurveyQA(surveyAnswers);
        }

        public async Task<string> CreateSurveyResponse(SurveyResponse surveyResponse)
        {
            var surveyTemplate = await GetSurveyTemplateByCode(surveyResponse.SurveyTemplateCode);
            var suveyTemplateId = surveyTemplate.Id;

            var newSurveyResponse = new JObject
            {
                ["ntw_comments"] = surveyResponse.Comments,
                ["ntw_CustomerID@odata.bind"] = $"/{Constants.ProductEntityName}({surveyResponse.CustomerId})",
                ["ntw_SurveyTemplateID@odata.bind"] = $"/{Constants.TicketSubTypeEntityName}({suveyTemplateId})"
            };

            if (!surveyResponse.CaseNumberId.IsEmpty())
            {
                newSurveyResponse["ntw_CaseNumberID@odata.bind"] = $"/{Constants.IncidentEntityName}({surveyResponse.CaseNumberId})";
            }

            var result = await _crmService.Save(Constants.SurveyResponseEntityName, newSurveyResponse);

            var surveyResponseId = result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;

            if (surveyResponseId.IsEmpty())
            {
                return null;
            }

            var batchRequest = new ChangeSetBatchRequest(_crmConfig);

            foreach (var item in surveyResponse.SurveyResponseDetail)
            {
                var surveyQuestions = await GetSurveyQuestionsBySurveyCode(surveyResponse.SurveyTemplateCode);
                var surveyQuestionId = surveyQuestions?.FirstOrDefault(x => x.Code == item.QuestionCode).Id;

                var obj = new JObject
                {
                    ["ntw_textanswer"] = item.OtherAnswer,
                    ["ntw_SurveyResponseID@odata.bind"] = $"/{Constants.SurveyResponseEntityName}({surveyResponseId})",
                    ["ntw_QuestionID@odata.bind"] = $"/{Constants.SurveyQuestionEntityName}({surveyQuestionId})",
                };

                if (!item.AnswerCode.IsEmpty())
                {
                    var surveyAnswers = await GetSurveyAnswersByQuestionCode(item.QuestionCode);
                    var surveyAnswerId = surveyAnswers?.FirstOrDefault(x => x.Code == item.AnswerCode).Id;

                    obj["ntw_AnswerID@odata.bind"] = $"/{Constants.SurveyAnswerEntityName}({surveyAnswerId})";
                }

                var batchRequestItem = new ChangeSetBatchRequestItem
                {
                    PostObject = obj,
                    EntityPluralName = Constants.SurveyResponseDetailsEntityName
                };

                batchRequest.AddRequest(batchRequestItem);
            }

            var response = await _crmService.ExecuteChangeSetBatchRequest(batchRequest);

            if (response != null && response.Any())
            {
                return surveyResponseId;
            }

            return null;
        }

        public async Task<string> CreateSurveyResponseV2(SurveyResponse surveyResponse)
        {
            IEnumerable<string> response = null;
            string surveyResponseId = null;
            double totalScore = 0;
            try
            {
                var surveyTemplate = await GetSurveyTemplateByCode(surveyResponse.SurveyTemplateCode);
                var suveyTemplateId = surveyTemplate.Id;

                var newSurveyResponse = new JObject
                {
                    ["ntw_comments"] = surveyResponse.Comments,
                    ["ntw_CustomerID@odata.bind"] = $"/{Constants.ProductEntityName}({surveyResponse.CustomerId})",
                    ["ntw_SurveyTemplateID@odata.bind"] = $"/{Constants.TicketSubTypeEntityName}({suveyTemplateId})",
                    ["statuscode"] = Constants.SubmittedSurveyResponseCode,
                    ["statecode"] = "1"
                };

                if (!surveyResponse.CaseNumberId.IsEmpty())
                {
                    newSurveyResponse["ntw_CaseNumberID@odata.bind"] = $"/{Constants.IncidentEntityName}({surveyResponse.CaseNumberId})";
                }

                var result = await _crmService.Save(Constants.SurveyResponseEntityName, newSurveyResponse);

                surveyResponseId = result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;

                if (surveyResponseId.IsEmpty())
                {
                    return null;
                }

                var batchRequest = new ChangeSetBatchRequest(_crmConfig);

                foreach (var item in surveyResponse.SurveyResponseDetail)
                {
                    var surveyQuestions = await GetSurveyQuestionsBySurveyCode(surveyResponse.SurveyTemplateCode);
                    var surveyQuestionId = surveyQuestions?.FirstOrDefault(x => x.Code == item.QuestionCode).Id;

                    var questionWeight = surveyQuestions?.FirstOrDefault(x => x.Code == item.QuestionCode).Weight;

                    var obj = new JObject
                    {
                        ["ntw_textanswer"] = item.OtherAnswer,
                        ["ntw_SurveyResponseID@odata.bind"] = $"/{Constants.SurveyResponseEntityName}({surveyResponseId})",
                        ["ntw_QuestionID@odata.bind"] = $"/{Constants.SurveyQuestionEntityName}({surveyQuestionId})",
                    };

                    var surveyAnswers = await GetSurveyAnswersByQuestionCode(item.QuestionCode);


                    if (item.AnswerCode.IsEmpty() && !item.OtherAnswer.IsEmpty())
                    {
                        var isOtherQuestion = surveyAnswers?.FirstOrDefault(x => x.IsOther == true);

                        double isOtherScore = SetResponseDetailScore(questionWeight, isOtherQuestion.Score);

                        obj["ntw_responsedetailscore"] = isOtherScore;

                        totalScore += isOtherScore;
                    }

                    if (!item.AnswerCode.IsEmpty())
                    {
                        var surveyAnswerId = surveyAnswers?.FirstOrDefault(x => x.Code == item.AnswerCode).Id;

                        var answerScore = surveyAnswers?.FirstOrDefault(x => x.Code == item.AnswerCode).Score;

                        double score = SetResponseDetailScore(questionWeight, answerScore);

                        totalScore += score;

                        obj["ntw_AnswerID@odata.bind"] = $"/{Constants.SurveyAnswerEntityName}({surveyAnswerId})";
                        obj["ntw_responsedetailscore"] = score;
                    }
                    var batchRequestItem = new ChangeSetBatchRequestItem
                    {
                        PostObject = obj,
                        EntityPluralName = Constants.SurveyResponseDetailsEntityName
                    };

                    batchRequest.AddRequest(batchRequestItem);
                }

                response = await _crmService.ExecuteChangeSetBatchRequest(batchRequest);

                if (response != null && response.Any())
                {
                    var updatedObj = new JObject
                    {
                        ["ntw_surveyresponsetotalscore"] = totalScore,
                    };

                    var updatedRes = await _crmService.Save(Constants.SurveyResponseEntityName, updatedObj, surveyResponseId);

                    return surveyResponseId;
                }

                if (surveyResponseId != null && response == null)
                {
                    await _crmService.Delete(Constants.SurveyResponseEntityName, surveyResponseId);
                }

                return null;
            }
            catch (Exception)
            {
                if (surveyResponseId != null && response == null)
                {
                    await _crmService.Delete(Constants.SurveyResponseEntityName, surveyResponseId);
                }

                throw;
            }
        }

        public async Task<string> GetSurveyResponseStatus(string caseId)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='ntw_surveyresponse'>
                                <attribute name='statuscode'/>
                                <filter type='and'>
                                <condition attribute='ntw_casenumberid' operator='eq' value='{caseId}'/>
                                </filter>
                                </entity>
                          </fetch>";

            var data = await _crmService.Get(Constants.SurveyResponseEntityName, query);

            if (data == null || !data.Any())
            {
                return null;
            }

            var status = data[0]?.GetValue<string>("statuscode");

            return status;

        }

        //NotImplementedException
        //public Task<IEnumerable<string>> AddAttachments(IEnumerable<Attachment> attachments)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<IEnumerable<AttachmentInfo>> GetAttachmentsInfo(RecordType recordType, IEnumerable<string> recordIds)
        {
            var recordIdsCondition =
            recordIds.IsEmpty() ? string.Empty :
            recordIds.Select(r => $@"<condition attribute='ntws_recordid' operator='eq' value='{r}' />")
                     .Aggregate((m1, m2) => m1 + m2);

            string entityLogicaleName = recordType.ToString().ToLower();

            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntws_file'>
                                <attribute name='ntws_fileid' />
                                <attribute name='ntws_titlear' />
                                <attribute name='createdon' />
                                <attribute name='ntws_recordid' />
                                <attribute name='ntws_url' />
                                <attribute name='ntws_titleen' />
                                <attribute name='ntws_filepath' />
                                <attribute name='ntws_filename' />
                                <order attribute='ntws_titlear' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ntws_isexternal' operator='eq' value='1' />
                                  <filter type='or'>
                                    {recordIdsCondition}
                                  </filter>
                                 <condition attribute='ntws_entitylogicalname' operator='eq' value='{entityLogicaleName}' />
                                </filter>
                              </entity>
                            </fetch>";

            var batchRequest = new BatchRequest(_crmConfig);
            var batchRequestItem = new BatchRequestItem { EntityPluralName = Constants.FileEntityName, FetchXml = query };
            batchRequest.AddRequest(batchRequestItem);

            var attachmentsInfo = await _crmService.ExecuteBatchRequest(batchRequest);

            return FillAttachmentsInfo(attachmentsInfo.FirstOrDefault()?.Data);
        }

        public async Task<IEnumerable<AttachmentInfo>> GetAttachmentsInfo(RecordType recordType, string recordId)
        {
            string entityLogicaleName = recordType.ToString().ToLower();

            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntws_file'>
                                <attribute name='ntws_fileid' />
                                <attribute name='ntws_titlear' />
                                <attribute name='createdon' />
                                <attribute name='ntws_recordid' />
                                <attribute name='ntws_url' />
                                <attribute name='ntws_titleen' />
                                <attribute name='ntws_filepath' />
                                <attribute name='ntws_filename' />
                                <order attribute='ntws_titlear' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ntws_isexternal' operator='eq' value='1' />
                                  <filter type='or'>
                                    <condition attribute='ntws_recordid' operator='eq' value='{recordId}' />
                                  </filter>
                                 <condition attribute='ntws_entitylogicalname' operator='eq' value='{entityLogicaleName}' />
                                </filter>
                              </entity>
                            </fetch>";

            var attachmentsInfo = await _crmService.Get(Constants.FileEntityName, query);

            return FillAttachmentsInfo(attachmentsInfo);
        }

        public async Task<IEnumerable<CallbackRequestResponse>> GetCallbackRequests(string cif, string callbackStatus)
        {
            var callbackCondition = @$"<filter type='and'>
                                    <condition attribute='statuscode' operator='eq' value='{callbackStatus}'/>
                                </filter>";

            var query = @$"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                        <entity name='ntw_callbackrequest'>
                            <attribute name='ntw_callbackrequestid'/>
                            <attribute name='ntw_referenceid'/>
                            <attribute name='ntw_timeintervalset'/>
                            <attribute name='statuscode'/>
                            <attribute name='statecode'/>
                            <attribute name='ntw_rmnameid'/>
                            <attribute name='ntw_rmclosedate'/>
                            <attribute name='ntw_chosendate'/>
                            <attribute name='ntw_rmmobilephone'/>
                            <order attribute='ntw_referenceid' descending='false'/>
                            {(string.IsNullOrEmpty(callbackStatus) ? string.Empty : callbackCondition)}
                            <link-entity name='contact' from='contactid' to='ntw_customerid' link-type='inner' alias='ab'>
                                <filter type='and'>
                                    <condition attribute='ntw_cif' operator='eq' value='{cif}'/>
                                </filter>
                            </link-entity>
                            <link-entity name='systemuser' from='systemuserid' to='ntw_rmnameid' visible='false' link-type='outer' alias='a_97fb6fbcc41def119e376045bd6aa06d'>
                                <attribute name='internalemailaddress' alias='rmemail'/>
                                <attribute name='address1_telephone1' alias='rmtelephone'/>
                                <attribute name='fullname' alias='rmfullname'/>
                            </link-entity>
                        </entity>
                    </fetch>";

            var callbacks = await _crmService.Get(Constants.CallbackRequestsEntityname, query);

            return FillCallbacks(callbacks);
        }

        public async Task<IEnumerable<CallbackRequestResponse>> GetCallbackRequests(string customerId, string callbackStatus, string rmId, string chosenDate, string timeInterval)
        {
            var query = @$"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='ntw_callbackrequest'>
                                <attribute name='ntw_callbackrequestid'/>
                                <attribute name='ntw_referenceid'/>
                                <attribute name='createdon'/>
                                <attribute name='ntw_chosendate'/>
                                <order attribute='ntw_referenceid' descending='false'/>
                                <filter type='and'>
                                <condition attribute='statuscode' operator='eq' value='{callbackStatus}'/>
                                <condition attribute='ntw_customerid' operator='eq' uitype='contact' value='{customerId}'/>
                                <condition attribute='ntw_rmnameid' operator='eq' uitype='systemuser' value='{rmId}'/>
                                <condition attribute='ntw_chosendate' operator='on' value='{chosenDate}'/>
                                <condition attribute='ntw_timeintervalset' operator='eq' value='{timeInterval}'/>
                                </filter>
                                </entity>
                           </fetch>";

            var callbacks = await _crmService.Get(Constants.CallbackRequestsEntityname, query);

            return FillCallbacks(callbacks);
        }

        public async Task<string> AssignCallbackRequest(CallbackRequest request)
        {
            var callback = new JObject
            {
                ["ntw_chosendate"] = request.CustomerChosenDate,
                ["ntw_timeintervalset"] = request.TimeInterval,
                ["ntw_rmmobilephone"] = request.RmMobile

            };

            callback["ntw_CustomerId@odata.bind"] = $"/{Constants.ContactEntityName}({request.InternalUserId})";
            callback["ntw_RMNameId@odata.bind"] = $"/{Constants.SystemUserEntityName}({request.InternalRmId})";

            var result = await _crmService.Save(Constants.CallbackRequestsEntityname, callback);

            return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
        }

        public async Task<bool> IsRmAssociatedWithTeam(string teamId, string rmId)
        {
            if (string.IsNullOrWhiteSpace(teamId) || string.IsNullOrWhiteSpace(rmId))
                return false;

            var query = $@"
        <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
          <entity name='systemuser'>
            <attribute name='fullname' />
            <order attribute='fullname' descending='false' />
            <filter type='and'>
              <condition attribute='systemuserid' operator='eq' value='{rmId}' />
            </filter>
            <link-entity name='teammembership' from='systemuserid' to='systemuserid' visible='false' intersect='true'>
              <link-entity name='team' from='teamid' to='teamid' alias='ah'>
                <filter type='and'>
                  <condition attribute='teamid' operator='eq' value='{teamId}' />
                </filter>
              </link-entity>
            </link-entity>
          </entity>
        </fetch>";

            var result = await _crmService.Get(Constants.SystemUserEntityName, query);

            return result != null && result.Any();
        }



        public async Task<string> CreateRmRecord(RmInternalDetails request)
        {
            var rm = new JObject
            {
                ["domainname"] = $@"{request.Domain}\{request.Name}",
                ["internalemailaddress"] = request.Email,
                ["mobilephone"] = request.Mobile,
                ["firstname"] = request.FirstName,
                ["lastname"] = request.LastName
            };

            rm["businessunitid@odata.bind"] = $"/{Constants.BusinessUnitEntityname}({request.BusinessUnitId})";

            var result = await _crmService.Save(Constants.SystemUserEntityName, rm);

            return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
        }

        public async Task<string> CreateAccount(AccountModel account)
        {
            var accountObj = new JObject
            {
                ["name"] = account.Name,
                ["telephone1"] = account.Mobile
            };

            var result = await _crmService.Save(Constants.AccountEntityname, accountObj);

            return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
        }

        public async Task<string> CreateAuthorizationRequest(AuthorizationRequest authorizationRequest)
        {
            try
            {
                var authRequest = new JObject
                {
                    ["ntw_cif"] = authorizationRequest.Cif,
                    ["ntw_applicationnumber"] = authorizationRequest.ApplicationNo,
                };

                authRequest["ntw_CustomerID@odata.bind"] = $"/{Constants.ContactEntityName}({authorizationRequest.ContactId})";


                var result = await _crmService.Save(Constants.AuthorizationRequestEntityName, authRequest);

                return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AssociateRmWithTeam(string teamId, string rmId)
        {
            var associateTeam = new JObject
            {
                ["systemuserid"] = rmId,
                ["teamid"] = teamId
            };

            var result = await _crmService.Associate(Constants.TeamEntityName, teamId, Constants.TeamMembershipAssociation, Constants.SystemUserEntityName, rmId);

            return result;
        }

        public async Task<string> CreateCustomer(CustomerModel customer)
        {
            try
            {
                var request = new JObject
                {
                    ["firstname"] = customer.FirstName,
                    ["lastname"] = customer.LastName,
                    ["ntw_cif"] = customer.CIF,
                    ["ntw_SegmentID@odata.bind"] = $"/{Constants.ContactEntityName}({customer.Segment})",
                    ["governmentid"] = customer.LegalId,
                    ["mobilephone"] = customer.MobileNumber,
                    ["ntw_PreferredLanguageID@odata.bind"] = $"/{Constants.ContactEntityName}({customer.PreferredLanguage})"
                };

                var result = await _crmService.Save(Constants.ContactEntityName, request, customer.CustomerGuid);

                return result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<EmployeeRMs> GetEmployeeRMs(string code)
        {
            try
            {
                string query = @$"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='ntx_employee'>
                                      <attribute name='ntx_employeeid' />
                                      <attribute name='ntx_name' />
                                      <attribute name='ntx_fullname' />
                                      <attribute name='ntx_code' />
                                      <attribute name='ntx_branchid' />
                                      <filter type='and'>
                                        <condition attribute='ntx_code' operator='eq' value='{code}' />
                                      </filter>
                                      <link-entity name='ntx_region' from='ntx_regionid' to='ntx_regionid' visible='false' link-type='outer'>
                                        <attribute name='ntx_name' alias='regionname' />
                                        <attribute name='ntx_code' alias='regioncode' />
                                      </link-entity>
                                      <link-entity name='ntx_region' from='ntx_regionid' to='ntx_regionid' visible='false' link-type='outer'>
                                        <link-entity name='ntx_employee' from='ntx_employeeid' to='ntx_managerid' visible='false' link-type='outer'>
                                          <attribute name='ntx_fullname' alias='managername' />
                                          <attribute name='ntx_name' alias='managernumber' />
                                          <attribute name='ntx_code' alias='managercode' />
                                        </link-entity>
                                      </link-entity>
                                      <link-entity name='ntw_branch' from='ntw_branchid' to='ntx_branchid' visible='false' link-type='outer'>
                                        <attribute name='ntw_name' alias='branchname' />
                                      </link-entity>
                                      <link-entity name='ntx_region' from='ntx_regionid' to='ntx_regionid'  link-type='outer'>
                                        <link-entity name='ntx_employee' from='ntx_employeeid' to='ntx_affluentareamanagerid'  link-type='outer'>
                                          <attribute name='ntx_code' alias='affluentareamanagercode' />
                                          <attribute name='ntx_name' alias='affluentareamanagernumber' />
                                          <attribute name='ntx_fullname' alias='affluentareamanagername' />
                                        </link-entity>
                                      </link-entity>
                                    </entity>
                                  </fetch>";

                var callbacks = await _crmService.Get(Constants.EmployeeEntityName, query);

                return FillEmployeeRMs(callbacks);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<EntitySpecificOptionSet>> GetCallbackTimeslots()
        {
            var timeSlotsArray = await _crmService.GetEntitySpecificOptionSet("ntw_callbackrequest", "ntw_timeintervalset");

            var timeSlots = FillTimeSlots(timeSlotsArray);

            return timeSlots;
        }

        public async Task<IEnumerable<EntitySpecificOptionSet>> GetSkillGroups()
        {
            var timeSlotsArray = await _crmService.GetEntitySpecificOptionSet("ntw_campaignlead", "ntw_skillgroupset");

            var timeSlots = FillTimeSlots(timeSlotsArray);

            return timeSlots;
        }

        public async Task<string> GetCaseResolutionTime(string caseId)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='incident'>
                                <attribute name='ticketnumber'/>
                                <attribute name='incidentid'/>
                                <attribute name='ntw_resolutiondate'/>
                                <order attribute='title' descending='false'/>
                                <filter type='and'>
                                <condition attribute='incidentid' operator='eq' uiname='' uitype='incident' value='{caseId}'/>
                                </filter>
                                </entity>
                                </fetch>";

            var data = await _crmService.Get(Core.Constants.IncidentEntityName, query);

            if (data == null || !data.Any())
            {
                return null;
            }

            var id = data[0]?.GetValue<string>("ntw_resolutiondate");

            return id;
        }

        public async Task<string> GetCustomerIdByCustomerMessageId(string customerMessageId)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='ntw_customermessage'>
                                <attribute name='ntw_customerid' />
                                <order attribute='ntw_referenceid' descending='false' />
                                <filter type='and'>
                                <condition attribute='ntw_customermessageid' operator='eq'  value='{customerMessageId}' />
                                </filter>
                                </entity>
                            </fetch>";

            var data = await _crmService.Get(Core.Constants.CustomerMessageEntityName, query);

            if (data == null || !data.Any())
            {
                return null;
            }

            var customerId = data[0]?.GetValue<string>("_ntw_customerid_value");

            return customerId;
        }

        public async Task<AutoDialerCampaignInfo> GetCampaignInfo(string campaignId, int pageIndex = 1)
        {

            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='5000' page='{pageIndex}'>
                          	<entity name='ntw_campaignleadrequestlead'>
                          		<link-entity name='lead' from='leadid' to='ntw_leadid'>
                          			<attribute name='mobilephone' alias='phone'/>
                          			<attribute name='ntx_cif' alias='cif'/>
                          			<attribute name='ntx_idnumber' alias='legid'/>
                          			<attribute name='fullname' alias='flname'/>
                          			<link-entity name='product' from='productid' to='ntx_producttype' link-type='outer'>
                          				<attribute name='name' alias='productname'/>
                          			</link-entity>
                          		</link-entity>
                          		<link-entity name='ntw_campaignlead' from='ntw_campaignleadid' to='ntw_campaignleadrequestid'>
                          			<attribute name='ntw_skillgroupset' alias='skilgroup'/>
                          			<attribute name='ntw_campaignleadrequestcode' alias='campcode'/>
                          			<filter>
                          				<condition attribute='ntw_campaignleadid' operator='eq' value='{campaignId}' />
                          			</filter>
                          		</link-entity>
                          	</entity>
                          </fetch>";

            var result = await _crmService.Get(Constants.CampaignLeadRequestLeadEntityName, query);

            return FillCampaignInfo(result);

        }

        public async Task<IEnumerable<LookupModel>> GetTppTicketCategories()
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='ntw_tppticketcategory'>
                        <attribute name='ntw_tppticketcategoryid'/>
                        <attribute name='ntw_name'/>
                        <attribute name='ntw_code'/>
                        <attribute name='createdon'/>
                        <order attribute='ntw_name' descending='false'/>
                      </entity>
                   </fetch>";

            var categories = await _crmService.Get(Constants.TppTicketCategory, query);

            return FillTppTicketCategories(categories);
        }

        public async Task<IEnumerable<LookupModel>> GetTppTicketTypes(string categoryCode)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='ntw_tpptickettype'>
                                <attribute name='ntw_tpptickettypeid'/>
                                <attribute name='ntw_name'/>
                                <attribute name='ntw_code'/>
                                <attribute name='createdon'/>
                                <order attribute='ntw_name' descending='false'/>
                                <link-entity name='ntw_tppticketcategory' from='ntw_tppticketcategoryid' to='ntw_tppcategory' link-type='inner' alias='ac'>
                                    <attribute name='ntw_tppticketcategoryid' alias='categoryid'/>
                                    <filter type='and'>
                                        <condition attribute='ntw_code' operator='eq' value='{categoryCode}' />
                                    </filter>
                                </link-entity>
                            </entity>
                        </fetch>";

            var types = await _crmService.Get(Constants.TppTicketType, query);

            return FillTppTicketTypes(types);
        }

        public async Task<IEnumerable<LookupModel>> GetBankEnvironments()
        {
            var result = await _crmService.GetOptionSet(Constants.BankEnvironmentOptionSet);
            return FillOptionSet(result);
        }

        public async Task<CollectionResult<TppTicketModel>> GetTppTickets(int pageSize, int pageIndex, string? tppId = null, string? tppUserId = null)
        {
            var filter = "";

            if (!string.IsNullOrWhiteSpace(tppId) || !string.IsNullOrWhiteSpace(tppUserId))
            {
                var conditions = "";

                if (!string.IsNullOrWhiteSpace(tppId))
                {
                    conditions += $"<condition attribute='ntw_ticketnumber' operator='eq' value='{tppId}' />";
                }

                if (!string.IsNullOrWhiteSpace(tppUserId))
                {
                    conditions += $"<condition attribute='ntw_userid' operator='eq' value='{tppUserId}' />";
                }

                filter = $"<filter type='and'>{conditions}</filter>";
            }

            var query = $@"<fetch count='{pageSize}' page='{pageIndex}'>
        <entity name='ntw_tppticket'>
            {filter}
            <attribute name='ntw_tppticketid'/>
            <attribute name='ntw_ticketnumber'/>
            <attribute name='createdon'/>
            <attribute name='ntw_userid'/>
            <attribute name='statuscode'/>
            <attribute name='ntw_severitytpp'/>
            <attribute name='ntw_severitypasp'/>
            <attribute name='ntw_reportingorganization'/>
            <attribute name='ntw_problemdetails'/>
            <attribute name='ntw_phone'/>
            <attribute name='ownerid'/>
            <attribute name='ntw_email'/>
            <attribute name='ntw_dateclosed'/>
            <attribute name='ntw_bankbrand'/>
            <attribute name='ntw_bankenvironment'/>
<attribute name='ntw_expectedresolutionstartdate'/>
    <attribute name='ntw_expectedresolutionenddate'/>
            <order attribute='ntw_ticketnumber' descending='false'/>
            <link-entity name='ntw_tppticketcategory' from='ntw_tppticketcategoryid' to='ntw_categoryid' link-type='inner' alias='ac'>
                <attribute name='ntw_tppticketcategoryid' alias='categoryid'/>
                <attribute name='ntw_name' alias='categoryname'/>
                <attribute name='ntw_code' alias='categorycode'/>
            </link-entity>
            <link-entity name='ntw_tpptickettype' from='ntw_tpptickettypeid' to='ntw_typeid' link-type='inner' alias='ad'>
                <attribute name='ntw_tpptickettypeid' alias='typeid'/>
                <attribute name='ntw_name' alias='typename'/>
                <attribute name='ntw_code' alias='typecode'/>
            </link-entity>
        </entity>
    </fetch>";

            var countQuery = $@"<fetch aggregate='true'>
        <entity name='ntw_tppticket'>
            {filter}
            <attribute name='ntw_tppticketid' alias='count' aggregate='countcolumn' />
        </entity>
    </fetch>";

            BatchRequestItem tppTicketsBatchRequestItem = new BatchRequestItem { EntityPluralName = Constants.TppTicket, FetchXml = query };
            BatchRequestItem tppTicketsCountBatchRequestItem = new BatchRequestItem { EntityPluralName = Constants.TppTicket, FetchXml = countQuery };
            BatchRequest tppTicketsBatchRequest = new BatchRequest(_crmConfig);
            tppTicketsBatchRequest.AddRequest(tppTicketsBatchRequestItem).AddRequest(tppTicketsCountBatchRequestItem);

            var batchResponse = await _crmService.ExecuteBatchRequest(tppTicketsBatchRequest);

            if (batchResponse != null && !batchResponse.IsEmpty())
            {
                var tppTickets = FillTppTickets(batchResponse.First().Data);
                var tppTicketsCount = batchResponse.ElementAt(1).Data[0].GetValue<int>("count");

                return new CollectionResult<TppTicketModel>(tppTickets, tppTicketsCount, pageSize, pageIndex);
            }

            return new CollectionResult<TppTicketModel>(null, 0, pageSize, pageIndex);
        }


        public async Task<string> CreateTppTicket(CreateTppTicketModel ticket)
        {
            var newTicket = new JObject
            {
                ["ntw_userid"] = ticket.UserId,
                ["ntw_reportingorganization"] = ticket.ReportingOrganization,
                ["ntw_email"] = ticket.Email,
                ["ntw_phone"] = ticket.Phone,
                ["ntw_problemdetails"] = ticket.ProblemDetails,
                ["ntw_bankbrand"] = ticket.BankBrand,
                ["ntw_bankenvironment"] = ticket.BankEnvironmentCode,
                ["ntw_severitytpp"] = ticket.SeverityTPP,
                ["ntw_severitypasp"] = ticket.SeverityPASP
            };

            if (!string.IsNullOrEmpty(_OwnerId))
            {
                newTicket["ownerid@odata.bind"] = $"/teams({_OwnerId})";
            }

            if (!string.IsNullOrEmpty(ticket.CategoryId))
            {
                newTicket["ntw_categoryid@odata.bind"] = $"/{Constants.TppTicketCategory}({ticket.CategoryId})";
            }

            if (!string.IsNullOrEmpty(ticket.TypeId))
            {
                newTicket["ntw_typeid@odata.bind"] = $"/{Constants.TppTicketType}({ticket.TypeId})";
            }

            var result = await _crmService.Save(Constants.TppTicket, newTicket);

            return result != null && !result["Id"].IsNullOrEmpty()
                ? result["Id"].ToString()
                : null;
        }

        public async Task<IEnumerable<LookupModel>> GetTicketStatusCodes()
        {
            return await GetOptionSetsByLanguage("statuscode", "incident");
        }




        #region PrivateMethods

        private async Task<IEnumerable<LookupModel>> GetOptionSetsByLanguage(string optionSetName, string relatedEntityName = null)
        {
            //var culture = _httpContextAccessor.GetCultureFromHeader();

            //var languageId = GetLanguageId(culture);

            // <condition attribute='langid' operator='eq' value='{languageId}' />

            string objectTypeCodeFilter = string.Empty;

            if (relatedEntityName != null)
            {
                var objectTypeCode = await _crmService.GetEntityObjectTypeCode(relatedEntityName);

                objectTypeCodeFilter = $@"<condition attribute='objecttypecode' operator='eq' value='{objectTypeCode}' />";
            }

            //  <condition attribute='objecttypecode' operator='eq' value='10490' />
            var query = $@"<fetch distinct='true'>
                             <entity name='stringmap'>
                               <attribute name='value' />
                               <attribute name='langid' />
                               <attribute name='attributevalue' />
                               <filter>
                                 {objectTypeCodeFilter}
                                 <condition attribute='attributename' operator='eq' value='{optionSetName}' />
                                <condition attribute='langid' operator='eq' value='1033' />
                               </filter>
                             </entity>
                           </fetch>";
            var Options = await _crmService.Get("stringmaps", query);
            return FillOptionsetByLanguage(Options);
        }


        private IEnumerable<LookupModel> FillOptionsetByLanguage(JArray data)
        {
            if (data == null || data.Count() == 0)
            {
                return null;
            }

            var result = new List<LookupModel>();

            foreach (var item in data)
            {
                var option = new LookupModel
                {
                    Code = item.GetValue<string>("attributevalue"),
                    Name = item.GetValue<string>("value"),
                };
                result.Add(option);
            }
            return result;
        }

        private IEnumerable<AttachmentInfo> FillAttachmentsInfo(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var attachmentsInfo = new List<AttachmentInfo>();

            foreach (var item in data)
            {
                var path = item.GetValue<string>("ntws_filepath");
                var recordId = item.GetValue<string>("ntws_recordid");
                string fullPath = "";
                if (!string.IsNullOrEmpty(path))
                {
                    path = attachementFilePath;
                    fullPath = path.Replace("{id}", recordId);
                }

                var attachmentInfo = new AttachmentInfo
                {
                    FileName = item.GetValue<string>("ntws_filename"),
                    AttachmentId = item.GetValue<string>("ntws_filename"),
                    RecordId = item.GetValue<string>("ntws_recordid"),
                    FilePath = fullPath,
                    TitleAr = item.GetValue<string>("ntws_titlear"),
                    TitleEn = item.GetValue<string>("ntws_titleen"),
                };
                attachmentsInfo.Add(attachmentInfo);
            }
            return attachmentsInfo;
        }

        private IEnumerable<BaseLookupModel> FillCategories(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var categories = new List<BaseLookupModel>();

            foreach (var item in data)
            {
                var category = new BaseLookupModel
                {
                    Id = item.GetValue<string>("ntw_requestcategoryid"),
                    Name = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("ntw_arabicname")).AddValueEn(item.GetValue<string>("ntw_name")),
                    Code = item.GetValue<string>("ntw_code")
                };
                categories.Add(category);
            }
            return categories;
        }

        private IEnumerable<BaseLookupModel> FillInternalProducts(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var products = new List<BaseLookupModel>();

            foreach (var item in data)
            {
                var product = new BaseLookupModel
                {
                    Id = item.GetValue<string>("productid"),
                    Name = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("ntw_arabicname")).AddValueEn(item.GetValue<string>("name")),
                    Code = item.GetValue<string>("productnumber")
                };
                products.Add(product);
            }
            return products;
        }

        private IEnumerable<BaseLookupModel> FillLeadProducts(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var leadProducts = new List<BaseLookupModel>();

            foreach (var item in data)
            {
                var product = new BaseLookupModel
                {
                    Id = item.GetValue<string>("ntw_leadproductid"),
                    Name = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("ntw_arabicname")).AddValueEn(item.GetValue<string>("ntw_name")),
                    Code = item.GetValue<string>("ntw_code")
                };
                leadProducts.Add(product);
            }
            return leadProducts;
        }

        private IEnumerable<Currency> FillTransactionCurrency(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var transactionCurrencies = new List<Currency>();

            foreach (var item in data)
            {
                var transactionCurrency = new Currency
                {
                    CurrencyId = item.GetValue<string>("transactioncurrencyid"),
                    CurrencyName = item.GetValue<string>("currencyname"),
                    CurrencyCode = item.GetValue<string>("isocurrencycode"),
                    ExchangeRate = item.GetValue<double>("exchangerate"),
                    CurrencySymbol = item.GetValue<string>("currencysymbol"),
                    CurrencyPrecision = item.GetValue<double>("currencyprecision"),
                };
                transactionCurrencies.Add(transactionCurrency);
            }
            return transactionCurrencies;
        }

        private IEnumerable<Case> FillTickets(JArray data, string language)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var tickets = new List<Case>();

            foreach (var item in data)
            {
                var isExisting = item.GetValue<bool>("ntw_isexistingcustomer");


                var contactMobile = item.GetValue<string>("cutomermobile");
                var incidentMobile = item.GetValue<string>("ntw_mobilenumber");


                var contactLegalId = item.GetValue<string>("legalid");
                var incidentLegalId = item.GetValue<string>("caselegalid");

                var firstName = item.GetValue<string>("ntw_firstname");
                var lastName = item.GetValue<string>("ntw_lastname");

                //space between first and last name
                var fullName = string.Join(" ", new[] { firstName, lastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

                var ticket = new Case
                {
                    CaseId = item.GetValue<string>("incidentid"),
                    AttachementPath = item.GetValue<string>("ntw_attachmentspath"),
                    Cif = item.GetValue<string>("customercif"),
                    CaseNumber = item.GetValue<string>("ticketnumber"),
                    LegalId = isExisting
                ? (string.IsNullOrWhiteSpace(contactLegalId) ? incidentLegalId : contactLegalId)
                : (string.IsNullOrWhiteSpace(incidentLegalId) ? contactLegalId : incidentLegalId),

                    CustomerId = item.GetValue<string>("contactguid"),
                    Description = item.GetValue<string>("description"),
                    CaseTitle = item.GetValue<string>("title"),
                    Status = item.GetValue<string>("statecode"),
                    AccountNumber = item.GetValue<string>("ntw_accountnumber"),
                    CaseStatus = item.GetValue<int>("statuscode"),
                    IsOpen = OpenStatusCodes.Contains(item.GetValue<int>("statuscode")),
                    ClaimAmount = item.GetValue<double>("ntw_claimamount"),
                    TransactionReference = item.GetValue<string>("ntw_transactionreference"),
                    TransactionMerchantId = item.GetValue<string>("ntw_transactionmerchantid"),
                    TransactionDate = item.GetValue<DateTime>("ntw_transactiondate").ToLocalTime(),
                    TransactionCardType = item.GetValue<string>("ntw_transactioncardtype"),
                    TransactionAuthorizationCode = item.GetValue<string>("ntw_transactionauthorizationcode"),
                    ReplyMethod = item.GetValue<string>("ntw_replymethodset"),
                    PreferredCallBackTime = item.GetValue<DateTime>("ntw_preferredcallbacktime").ToLocalTime(),
                    POSTerminalId = item.GetValue<string>("ntw_posterminalid"),
                    FinalReplyStatement = item.GetValue<string>("ntw_finalreplystatement"),
                    IsEligibleForOfficialLetter = item.GetValue<bool>("ntw_iseligibleforofficialletter") ? isEligibleForOfficialLetter : isNotEligibleForOfficialLetter,
                    ClaimReason = item.GetValue<string>("ntw_claimreason"),
                    CallBackReason = item.GetValue<string>("ntw_callbackreason"),
                    CaseCategory = item.GetValue<string>("requestcategoryid"),
                    Channel = language.EqualsIgnorCase("En") ? item.GetValue<string>("channelnameen") : item.GetValue<string>("channelnamear"),
                    Department = item.GetValue<string>("departmentid"),
                    CaseType = item.GetValue<string>("samatypeid"),
                    CaseSubType = item.GetValue<string>("samasubtypeid"),
                    Product = item.GetValue<string>("samaproductid"),
                    TransactionCurrency = item.GetValue<string>("currencyid"),
                    CustomerName = isExisting
                ? item.GetValue<string>("customerfullname")
                : (!string.IsNullOrWhiteSpace(fullName) ? fullName : item.GetValue<string>("customerfullname")),
                    CustomerMobile = !isExisting
                ? (!string.IsNullOrWhiteSpace(incidentMobile) ? incidentMobile : contactMobile)
                : (!string.IsNullOrWhiteSpace(contactMobile) ? contactMobile : incidentMobile),
                    OpenedDate = item.GetValue<DateTime>("createdon").ToLocalTime(),
                    ClosedDate = item.GetValue<DateTime>("ntw_qualitycheckdate").ToLocalTime(),
                    //SurveySubmissionDate = item.GetValue<DateTime>("ntw_submissiondate").ToLocalTime(),
                    SurveyReferenceId = item.GetValue<string>("surveyreferenceid"),
                    SurveyResponseStatus = item.GetValue<string>("surveyresponsestatuscode"),
                    SurveyComment = item.GetValue<string>("surveycomments"),
                    SurveyTextAnswer = item.GetValue<string>("surveytextanswer"),
                    SurveyQuestionName = language.EqualsIgnorCase("En") ? item.GetValue<string>("surveyquestionnameen") : item.GetValue<string>("surveyquestionnamear"),
                    SurveyQuestionId = item.GetValue<string>("surveyquestionid"),
                    SurveyAnswerName = language.EqualsIgnorCase("En") ? item.GetValue<string>("surveyanswernameen") : item.GetValue<string>("surveyanswernamear"),
                    SurveyResponseId = item.GetValue<string>("surveyresponseid"),
                    SurveyTemplateCode = item.GetValue<string>("templatecode"),
                    SurveyTemplateId = item.GetValue<string>("templateid"),
                    SurveyTemplateName = language.Equals("En", StringComparison.InvariantCultureIgnoreCase) ? item.GetValue<string>("templatenameen") : item.GetValue<string>("templatenamear"),
                    SurveyTemplateQuestionId = item.GetValue<string>("questionid"),
                    SurveyTemplateQuestionName = language.EqualsIgnorCase("En") ? item.GetValue<string>("questionnameen") : item.GetValue<string>("questionnamear"),
                    SurveyTemplateQuestionCode = item.GetValue<string>("questioncode"),
                    SurveyTemplateQuestionOrder = item.GetValue<int>("qestionorder"),
                    SurveyTemplateQuestionWeight = item.GetValue<string>("questionweight"),
                    IsOther = item.GetValue<bool>("isother"),
                    SurveyTemplateAnswerCode = item.GetValue<string>("answercode"),
                    SurveyTemplateAnswerId = item.GetValue<string>("answerid"),
                    SurveyTemplateAnswerName = language.EqualsIgnorCase("En") ? item.GetValue<string>("answernameen") : item.GetValue<string>("answernamear"),
                    SurveyTemplateAnswerOrder = item.GetValue<int>("answerorder"),
                    SurveyTemplateAnswerScore = item.GetValue<int>("answerscore"),
                    CallerNumber = item.GetValue<string>("ntw_callernumber"),
                    CalledNumber = item.GetValue<string>("ntw_callednumber"),
                    TransactionType = item.GetValue<string>("ntw_transactiontype"),
                    TransactionCode = item.GetValue<string>("ntw_transactioncode"),
                    IsFeesApplied = item.GetValue<bool>("ntw_feesapplied"),
                    CmsCardReplacementNumber = item.GetValue<string>("ntw_cmscardreplacementnumber"),
                    PreferredSMSLanguage = item.GetValue<string>("preferredsmslanguageid"),
                    IsOfficialLetterGenerated = item.GetValue<bool>("ntw_isofficiallettergenerated"),
                    OfficialLetterPath = item.GetValue<string>("ntw_officialletterurl"),
                    OfficialLetterFileName = item.GetValue<string>("ntw_officialletterfilename")
                };
                tickets.Add(ticket);
            }
            return tickets;
        }

        private IEnumerable<CustomerMessageModel> FillCustomerMessages(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var customerMessages = new List<CustomerMessageModel>();

            foreach (var item in data)
            {
                var customerMessage = new CustomerMessageModel
                {
                    ReferenceId = item.GetValue<string>("ntw_referenceid"),
                    CustomerMessageId = item.GetValue<string>("ntw_customermessageid"),
                    Status = item.GetValue<string>("statuscode"),
                    CreatedOn = item.GetValue<DateTime>("createdon").ToLocalTime(),
                    Subject = item.GetValue<string>("ntw_subject"),
                    Body = item.GetValue<string>("ntw_body"),
                    ReplyMessage = item.GetValue<string>("new_replymessage"),
                    CustomerId = item.GetValue<string>("customerid"),
                    CustomerFullName = item.GetValue<string>("customerfullname"),
                    CustomerMessageType = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("customermessagetypear")).AddValueEn(item.GetValue<string>("customermessagetypeen")),
                    CustomerMessageSubject = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("customermessagesubjectar")).AddValueEn(item.GetValue<string>("customermessagesubjecten")),
                };
                customerMessages.Add(customerMessage);
            }
            return customerMessages;
        }

        private CustomerMessageDetails FillCustomerMessageDetails(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var customerMessage = new CustomerMessageDetails
            {
                ReferenceId = data[0].GetValue<string>("ntw_referenceid"),
                Status = data[0].GetValue<string>("statuscode"),
                CustomerMessageId = data[0].GetValue<string>("ntw_customermessageid"),
                Subject = data[0].GetValue<string>("ntw_subject"),
                Body = data[0].GetValue<string>("ntw_body"),
                CreatedOn = data[0].GetValue<DateTime>("createdon").ToLocalTime(),
                ReplyMessage = data[0].GetValue<string>("new_replymessage"),
                CustomerId = data[0].GetValue<string>("customerid"),
                CustomerFullName = data[0].GetValue<string>("customerfullname"),
                ReadBy = data[0].GetValue<string>("readbyfullname"),
                ReadByUserId = data[0].GetValue<string>("readbyid"),
                ReadDate = data[0].GetValue<DateTime>("ntw_readdate").ToLocalTime(),
                RepliedBy = data[0].GetValue<string>("repliedfullname"),
                RepliedByUserId = data[0].GetValue<string>("repliedbyid"),
                ReplyDate = data[0].GetValue<DateTime>("ntw_replydate").ToLocalTime(),
                ConvertedToCaseBy = data[0].GetValue<string>("convertedtocasebyfullname"),
                ConvertedToCaseByUserId = data[0].GetValue<string>("convertedtocasebyid"),
                ConvertedToCaseDate = data[0].GetValue<DateTime>("ntw_converttocasedate").ToLocalTime(),
                CustomerMessageType = new LocalizedValue<string>().AddValueAr(data[0].GetValue<string>("customermessagetypear")).AddValueEn(data[0].GetValue<string>("customermessagetypeen")),
                CustomerMessageSubject = new LocalizedValue<string>().AddValueAr(data[0].GetValue<string>("customermessagesubjectar")).AddValueEn(data[0].GetValue<string>("customermessagesubjecten")),
            };

            return customerMessage;
        }

        private CustomerMessageResponse FillCustomerMessage(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var customerMessage = new CustomerMessageResponse
            {
                Id = data[0].GetValue<string>("ntw_customermessageid"),
                Subject = data[0].GetValue<string>("ntw_subject"),
                Body = data[0].GetValue<string>("ntw_body"),
                CustomerMessageType = data[0].GetValue<string>("customermessagetypeid"),
                CustomerMessageSubject = data[0].GetValue<string>("customermessagesubjectid")
            };

            return customerMessage;
        }

        private IEnumerable<BaseLookupModel> FillLookup(JArray data, string requestIdAttribute)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var result = new List<BaseLookupModel>();

            foreach (var item in data)
            {
                var lookupItem = new BaseLookupModel
                {
                    Id = item.GetValue<string>(requestIdAttribute),
                    Name = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("ntw_arabicname")).AddValueEn(item.GetValue<string>("ntw_name")),
                    Code = item.GetValue<string>("ntw_code")
                };
                result.Add(lookupItem);
            }
            return result;
        }

        private IEnumerable<SamaLookupModel> FillSamaLookup(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var result = new List<SamaLookupModel>();

            foreach (var item in data)
            {
                var lookupItem = new SamaLookupModel
                {
                    Id = item.GetValue<string>("internalid"),
                    Name = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("internalarabicname")).AddValueEn(item.GetValue<string>("internalname")),
                    Code = item.GetValue<string>("internalcode"),
                    RelatedRecordId = item.GetValue<string>("relatedrecordid"),
                    RelatedRecordCode = item.GetValue<string>("relatedrecordcode"),
                    SamaValues = new BaseLookupModel
                    {
                        Id = item.GetValue<string>("samaid"),
                        Name = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("samaarabicname")).AddValueEn(item.GetValue<string>("samaname")),
                        Code = item.GetValue<string>("samacode"),
                    }
                };
                result.Add(lookupItem);
            }
            return result;
        }

        private SurveyTemplateModel FillSurveyTemplate(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var template = new SurveyTemplateModel
            {
                Id = data[0].GetValue<string>("ntw_surveytemplateid"),
                Name = new LocalizedValue<string>().AddValueEn(data[0].GetValue<string>("ntw_name")).AddValueAr(data[0].GetValue<string>("ntw_arabicname")),
                Code = data[0].GetValue<string>("ntw_code"),
                Descripton = data[0].GetValue<string>("ntw_description"),
            };

            return template;
        }

        private IEnumerable<SurveyQuestionModel> FillSurveyQuestions(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var surveyQuestions = new List<SurveyQuestionModel>();

            foreach (var item in data)
            {
                var surveyQuestion = new SurveyQuestionModel
                {
                    Id = item.GetValue<string>("ntw_surveyquestionid"),
                    Name = new LocalizedValue<string>().AddValueEn(item.GetValue<string>("ntw_name")).AddValueAr(item.GetValue<string>("ntw_arabicname")),
                    Weight = item.GetValue<string>("ntw_weight"),
                    SurveyTemplateId = item.GetValue<string>("templateid"),
                    Order = item.GetValue<string>("ntw_order"),
                    Code = item.GetValue<string>("ntw_code"),
                };
                surveyQuestions.Add(surveyQuestion);
            }

            return surveyQuestions;
        }

        private IEnumerable<SurveyAnswerModel> FillSurveyAnswers(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var surveyAnswers = new List<SurveyAnswerModel>();

            foreach (var item in data)
            {
                var surveyAnswer = new SurveyAnswerModel
                {
                    Id = item.GetValue<string>("ntw_surveyanswerid"),
                    Name = new LocalizedValue<string>().AddValueEn(item.GetValue<string>("ntw_name")).AddValueAr(item.GetValue<string>("ntw_arabicname")),
                    Score = item.GetValue<string>("ntw_score"),
                    QuestionId = item.GetValue<string>("surveyquestionid"),
                    Order = item.GetValue<string>("ntw_order"),
                    Code = item.GetValue<string>("ntw_code"),
                    IsOther = item.GetValue<bool>("ntw_isother")
                };
                surveyAnswers.Add(surveyAnswer);
            }

            return surveyAnswers;
        }

        private List<SurveyQA> FillSurveyQA(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var surveyQAs = new List<SurveyQA>();

            foreach (var item in data)
            {
                var surveyQA = new SurveyQA
                {
                    QuestionId = item.GetValue<string>("questionid"),
                    QuestionCode = item.GetValue<string>("qestioncode"),
                    QuestionName = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("questionnamear")).AddValueEn(item.GetValue<string>("questionnameen")),
                    QuestionOrder = item.GetValue<string>("questionorder"),
                    QuestionWeight = item.GetValue<string>("weight"),
                    AnswerId = item.GetValue<string>("answerid"),
                    AnswerCode = item.GetValue<string>("answercode"),
                    AnswerName = new LocalizedValue<string>().AddValueAr(item.GetValue<string>("answernamear")).AddValueEn(item.GetValue<string>("answernameen")),
                    AnswerOrder = item.GetValue<string>("answerorder"),
                    AnswerScore = item.GetValue<string>("answerscore"),
                    SurveyTemplateId = item.GetValue<string>("templateid"),
                };
                surveyQAs.Add(surveyQA);
            }

            return surveyQAs;
        }

        private IEnumerable<LookupModel> FillOptionSet(JArray data)
        {
            if (data == null || data.Count() == 0)
            {
                return null;
            }

            var items = new List<LookupModel>();

            foreach (var item in data)
            {
                var label = JToken.Parse(item["Label"]["LocalizedLabels"].ToString());
                var option = new LookupModel
                {
                    Code = item.GetValue<string>("Value"),
                    Name = label.First.GetValue<string>("Label"),
                };
                items.Add(option);
            }
            return items;
        }

        private IEnumerable<BaseLookupModel> FillLocalizedOptionSet(JArray data)
        {
            if (data == null || data.Count() == 0)
            {
                return null;
            }

            var items = new List<BaseLookupModel>();

            foreach (var item in data)
            {
                var label = JToken.Parse(item["Label"]["LocalizedLabels"].ToString());

                var labels = JsonConvert.DeserializeObject<List<LabelInfo>>(item["Label"]["LocalizedLabels"].ToString());

                var option = new BaseLookupModel
                {
                    Code = item.GetValue<string>("Value"),
                    Name = new LocalizedValue<string>().AddValueAr(labels.FirstOrDefault(l => l.LanguageCode == 1025)?.Label).AddValueEn(labels.FirstOrDefault(l => l.LanguageCode == 1033).Label)
                };
                items.Add(option);
            }
            return items;
        }

        private IEnumerable<CallbackRequestResponse> FillCallbacks(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var callbacks = new List<CallbackRequestResponse>();

            foreach (var item in data)
            {
                var callback = new CallbackRequestResponse
                {
                    RmEmail = item.GetValue<string>("rmemail"),
                    RmName = item.GetValue<string>("rmfullname"),
                    RmPhoneNumber = item.GetValue<string>("ntw_rmmobilephone"),
                    RequestStatus = item.GetValue<string>("statecode"),
                    CallStatus = item.GetValue<string>("statuscode"),
                    CustomerRequestedDate = item.GetValue<DateTime>("ntw_chosendate"),
                    TimeSlotCode = item.GetValue<string>("ntw_timeintervalset"),
                    RmCloseDateTime = item.GetValue<DateTime>("ntw_rmclosedate"),
                };

                callbacks.Add(callback);
            }

            return callbacks;
        }

        private EmployeeRMs FillEmployeeRMs(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var record = data.FirstOrDefault();

            var employeeDetails = new EmployeeRMs
            {
                Name = record.GetValue<string>("ntx_fullname"),
                Number = record.GetValue<string>("ntx_name"),
                Branch = record.GetValue<string>("branchname"),
                Code = record.GetValue<string>("ntx_code"),
                RegionCode = record.GetValue<string>("regioncode"),
                RegionName = record.GetValue<string>("regionname"),
                RegionalManagerDetails = new AreaManager
                {
                    Name = record.GetValue<string>("managername"),
                    Number = record.GetValue<string>("managernumber"),
                    Code = record.GetValue<string>("managercode")
                },
                AffluentAreaManagerDetails = new AffluentAreaManager
                {
                    Name = record.GetValue<string>("affluentareamanagername"),
                    Number = record.GetValue<string>("affluentareamanagernumber"),
                    Code = record.GetValue<string>("affluentareamanagercode")
                }
            };

            return employeeDetails;
        }

        private AutoDialerCampaignInfo FillCampaignInfo(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var record = data.FirstOrDefault();

            var campaignInfo = new AutoDialerCampaignInfo();
            campaignInfo.CampaignCode = record.GetValue<string>("campcode");
            campaignInfo.SkillGroup = record.GetValue<string>("skilgroup");

            var leadsInfo = new List<AutoDialerLeadInfo>();

            foreach (var item in data)
            {
                var leadInfo = new AutoDialerLeadInfo
                {
                    CIF = item.GetValue<string>("cif"),
                    LegalId = item.GetValue<string>("legid"),
                    FullName = item.GetValue<string>("flname"),
                    PhoneNumber = item.GetValue<string>("phone"),
                    ProductTypeName = item.GetValue<string>("productname")
                };

                leadsInfo.Add(leadInfo);
            }

            campaignInfo.Leads = leadsInfo;

            return campaignInfo;
        }

        private string ExtractDomainName(string email)
        {
            Regex regex = new Regex(@"@([^.]+)");
            Match match = regex.Match(email);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }

        private IEnumerable<EntitySpecificOptionSet> FillTimeSlots(JArray timeSlots)
        {
            var optionSets = new List<EntitySpecificOptionSet>();

            foreach (JToken option in timeSlots)
            {
                int optionValue = option["Value"].Value<int>();

                JArray localizedLabels = option["Label"]["LocalizedLabels"].Value<JArray>();

                foreach (JToken localizedLabel in localizedLabels)
                {
                    string label = localizedLabel["Label"].Value<string>();

                    optionSets.Add(new EntitySpecificOptionSet
                    {
                        Code = optionValue.ToString(),
                        Value = label,
                    });
                }
            }

            return optionSets;
        }

        private double SetResponseDetailScore(string questionWeight, string answerScore)
        {
            double weight = double.Parse(questionWeight);
            double score = double.Parse(answerScore);

            double finalScore = (weight * score) / 100;

            return finalScore;
        }

        private IEnumerable<LookupModel> FillTppTicketCategories(JArray data)
        {
            if (data == null || !data.Any())
            {
                return Enumerable.Empty<LookupModel>();
            }

            var categories = new List<LookupModel>();

            foreach (var item in data)
            {
                var category = new LookupModel
                {
                    Id = item.GetValue<string>("ntw_tppticketcategoryid"),
                    Name = item.GetValue<string>("ntw_name"),
                    Code = item.GetValue<string>("ntw_code")
                };
                categories.Add(category);
            }
            return categories;
        }

        private IEnumerable<LookupModel> FillTppTicketTypes(JArray data)
        {
            if (data == null || !data.Any())
            {
                return Enumerable.Empty<LookupModel>();
            }

            var types = new List<LookupModel>();

            foreach (var item in data)
            {
                var type = new LookupModel
                {
                    Id = item.GetValue<string>("ntw_tpptickettypeid"),
                    Name = item.GetValue<string>("ntw_name"),
                    Code = item.GetValue<string>("ntw_code"),

                };
                types.Add(type);
            }

            return types;
        }

        private IEnumerable<TppTicketModel> FillTppTickets(JArray data)
        {
            if (data == null || !data.Any())
                return Enumerable.Empty<TppTicketModel>();

            var tickets = new List<TppTicketModel>();

            foreach (var item in data)
            {
                var ticket = new TppTicketModel
                {
                    Id = item.GetValue<string>("ntw_tppticketid"),
                    TicketNumber = item.GetValue<string>("ntw_ticketnumber"),
                    CreatedOn = item.GetValue<DateTime>("createdon"),
                    UserId = item.GetValue<string>("ntw_userid"),
                    StatusCode = item.GetValue<string>("statuscode"),
                    SeverityTPP = item.GetValue<string>("ntw_severitytpp"),
                    SeverityPASP = item.GetValue<string>("ntw_severitypasp"),
                    ReportingOrganization = item.GetValue<string>("ntw_reportingorganization"),
                    ProblemDetails = item.GetValue<string>("ntw_problemdetails"),
                    Phone = item.GetValue<string>("ntw_phone"),
                    OwnerId = item.GetValue<string>("ownerid"),
                    Email = item.GetValue<string>("ntw_email"),
                    DateClosed = item.GetValue<DateTime?>("ntw_dateclosed"),
                    BankBrand = item.GetValue<string>("ntw_bankbrand"),
                    BankEnvironment = item.GetValue<string>("ntw_bankenvironment"),
                    ExpectedResolutionStartDate = item.GetValue<DateTime?>("ntw_expectedresolutionstartdate"),
                    ExpectedResolutionEndDate = item.GetValue<DateTime?>("ntw_expectedresolutionenddate"),
                    Category = new LookupModel
                    {
                        Id = item.GetValue<string>("categoryid"),
                        Name = item.GetValue<string>("categoryname"),
                        Code = item.GetValue<string>("categorycode")
                    },
                    Type = new LookupModel
                    {
                        Id = item.GetValue<string>("typeid"),
                        Name = item.GetValue<string>("typename"),
                        Code = item.GetValue<string>("typecode")
                    }
                };
                tickets.Add(ticket);
            }
            return tickets;
        }
        #endregion        
    }
}