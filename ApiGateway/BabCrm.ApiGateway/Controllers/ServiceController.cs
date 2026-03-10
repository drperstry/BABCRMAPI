using Asp.Versioning;
using AutoMapper;
using BabCrm.Core;
using BabCrm.Logging;
using BabCrm.Service;
using BabCrm.Service.ArchiveDataModels;
using BabCrm.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BabCrm.ApiGateway.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2)]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        private readonly ServiceManager _serviceManager;

        private readonly IMapper _mapper;

        public ServiceController(ServiceManager _serviceManager, IMapper mapper)
        {
            this._serviceManager = _serviceManager;
            this._mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping() => Ok($" hello the language is: {this.GetCurrentCulture().Name}");

        [HttpGet("whoAmI")]
        public async Task<IActionResult> WhoAmI() => Ok(await _serviceManager.WhoAmI());


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts([FromQuery] string? channelCode)
        {
            var currentCulture = this.GetCurrentCulture();

            var result = await _serviceManager.GetChannelProducts(channelCode);
            var products = _mapper.Map<IEnumerable<BaseLookupModel>>(result)?.Select(l => l.ToModel(currentCulture));

            return Ok(products);
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] string? channelCode)
        {
            var currentCulture = this.GetCurrentCulture();

            var categories = await _serviceManager.GetChannelCategories(channelCode);

            var result = categories?.Select(l => l.ToModel(currentCulture));
            return Ok(result);
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("tickettypes")]
        public async Task<IActionResult> GetTicketTypes(string categoryCode = "", string? productCode = "")
        {
            var currentCulture = this.GetCurrentCulture();

            var result = await _serviceManager.GetTicketTypes(categoryCode, productCode);

            var ticketTypes = _mapper.Map<IEnumerable<BaseLookupModel>>(result).Select(l => l.ToModel(currentCulture));

            return Ok(ticketTypes);
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("ticketsubtypes")]
        public async Task<IActionResult> GetSubTypes(string ticketTypeCode)
        {
            var currentCulture = this.GetCurrentCulture();

            var result = await _serviceManager.GetSubTypes(ticketTypeCode);
            var ticketSubTypes = _mapper.Map<IEnumerable<BaseLookupModel>>(result).Select(l => l.ToModel(currentCulture));

            return Ok(ticketSubTypes);
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("channels")]
        public async Task<IActionResult> GetChannels()
        {
            var currentCulture = this.GetCurrentCulture();

            var channels = await _serviceManager.GetChannels();

            return Ok(channels.Select(l => l.ToModel(currentCulture)));
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var currentCulture = this.GetCurrentCulture();

            var countries = await _serviceManager.GetCountries();

            return Ok(countries.Select(l => l.ToModel(currentCulture)));
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguages()
        {
            var currentCulture = this.GetCurrentCulture();

            var languages = await _serviceManager.GetLanguages();

            return Ok(languages.Select(l => l.ToModel(currentCulture)));
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("telesaleschannels")]
        public async Task<IActionResult> GetTeleSalesChannels()
        {
            var currentCulture = this.GetCurrentCulture();

            var teleSalesChannels = await _serviceManager.GetTeleSalesChannels();

            return Ok(teleSalesChannels.Select(l => l.ToModel(currentCulture)));
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("cardtypes")]
        public async Task<IActionResult> GetCardTypes() => Ok(await _serviceManager.GetCardTypes());


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("notificationchannels")]
        public async Task<IActionResult> GetNotificationChannels() => Ok(await _serviceManager.GetNotificationChannels());


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("legalidtypes")]
        public async Task<IActionResult> GetLegalIdTypes() => Ok(await _serviceManager.GetLegalIdTypes());


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("monnthlysalaryoptions")]
        public async Task<IActionResult> GetMonthlySalaryOptions() => Ok(await _serviceManager.GetMonthlySalaryOptions());


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<BaseLookupModel>))]
        [HttpGet("divisions")]
        public async Task<IActionResult> GetDividions() => Ok(await _serviceManager.GetDividions());


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("preferedtimeoptions")]
        public async Task<IActionResult> GetPreferedTimeOptions() => Ok(await _serviceManager.GetPreferedTimeOptions());


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var currentCulture = this.GetCurrentCulture();

            var depatments = await _serviceManager.GetDepartments();

            return Ok(depatments.Select(l => l.ToModel(currentCulture)));
        }

        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("tppticketcategories")]
        public async Task<IActionResult> GetTppTicketCategories()
        {
            try
            {
                var categories = await _serviceManager.GetTppTicketCategories();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetTppTicketCategories));
                return StatusCode(500, "An error occurred while fetching ticket categories.");
            }
        }

        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("tpptickettypes")]
        public async Task<IActionResult> GetTppTicketTypes([FromQuery] string categoryCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryCode))
                {
                    return BadRequest("categoryCode is required");
                }

                var types = await _serviceManager.GetTppTicketTypes(categoryCode);

                return Ok(types);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetTppTicketTypes));
                return StatusCode(500, "An error occurred while fetching ticket types.");
            }
        }

        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("bankenvironments")]
        public async Task<IActionResult> GetBankEnvironments()
        {
            try
            {
                var result = await _serviceManager.GetBankEnvironments();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetBankEnvironments));
                return StatusCode(500, "An error occurred while fetching bank environments.");
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("customermessagetypes")]
        public async Task<IActionResult> GetCustomerMessageTypes()
        {
            var currentCulture = this.GetCurrentCulture();

            var customerMessageTypes = await _serviceManager.GetCustomerMessageTypes();

            return Ok(customerMessageTypes.Select(l => l.ToModel(currentCulture)));
        }

        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("ticketstatusoptions")]
        public async Task<IActionResult> GetTicketStatusOptions()
        {
            //var currentCulture = this.GetCurrentCulture();

            var result = await _serviceManager.GetTicketStatusOptions();

            return Ok(result);
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<LookupModel>))]
        [HttpGet("customermessagesubjects")]
        public async Task<IActionResult> GetCustomerMessageSubjects()
        {
            var currentCulture = this.GetCurrentCulture();

            var customerMessageSubjects = await _serviceManager.GetCustomerMessageSubjects();

            return Ok(customerMessageSubjects.Select(l => l.ToModel(currentCulture)));
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<Currency>))]
        [HttpGet("transactioncurrencies")]
        public async Task<IActionResult> GetTransactionCurrencies() => Ok(await _serviceManager.GetTransactionCurrencies());

        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CaseViewModel>))]
        [HttpPost("tickets")]
        public async Task<IActionResult> GetTickets(string? cif, string? channelCode, TicketFilter filter) =>
            Ok(await _serviceManager.GetTickets(cif, channelCode, filter, this.GetCurrentCulture()));
        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CaseViewModel>))]
        [HttpGet("gettickets")]
        public async Task<IActionResult> GetTicketsByQuery(
    [FromQuery] string? cif,
    [FromQuery] string? channelCode,
    [FromQuery] string? nationalId,
    [FromQuery] string? mobileNumber,
    [FromQuery] string? categoryCode,
    [FromQuery] string? typeCode,
    [FromQuery] string? subTypeCode,
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate,
    [FromQuery] bool? isInternal,
    [FromQuery] bool? IsOpen,
    [FromQuery] string? statusCode
)
        {
            try
            {
                var filter = new TicketFilter
                {
                    NationalId = nationalId,
                    MobileNumber = mobileNumber,
                    CategoryCode = categoryCode,
                    TypeCode = typeCode,
                    SubTypeCode = subTypeCode,
                    StartDate = startDate,
                    EndDate = endDate,
                    IsInternal = isInternal ?? false,
                    IsOpen = IsOpen ?? false,
                    StatusCode = statusCode
                };

                var tickets = await _serviceManager.GetTickets(cif, channelCode, filter, this.GetCurrentCulture());

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetTicketsByQuery));
                return StatusCode(500, "An error occurred while fetching tickets.");
            }
        }

        [MapToApiVersion(1)]
        [HttpGet("ticketbykey")]
        public async Task<IActionResult> GetTicket(string key) => Ok(await _serviceManager.GetTicketByKey(key, this.GetCurrentCulture()));

        [MapToApiVersion(2)]
        [HttpGet("ticketbykey")]
        public async Task<IActionResult> GetTicketV2(string key) => Ok(await _serviceManager.GetTicketByKeyV2(key, this.GetCurrentCulture()));

        [HttpPost("ticket")]
        public async Task<IActionResult> CreateNewTicket([FromBody] TicketRequestModel request)
        {
            try
            {
                //request.PreferredSMSLanguage = this.GetCurrentCulture().Name.Substring(0, 1).ToUpper();
                var ticket = _mapper.Map<TicketRequest>(request);
                var result = await _serviceManager.CreateNewTicket(ticket);

                if (!result.Success)
                {
                    Logger.ApiLog(request, null, nameof(CreateNewTicket));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(request, ex, nameof(CreateNewTicket));
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AuthorizationRequest")]
        public async Task<IActionResult> CreateAuthorizationRequest([FromBody] AuthorizationRequest request)
        {
            try
            {
                var result = await _serviceManager.CreateAuthorizationRequest(request);

                if (!result.Success)
                {
                    Logger.ApiLog(request, null, nameof(CreateAuthorizationRequest));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(request, ex, nameof(CreateAuthorizationRequest));
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("archiveData")]
        public async Task<IActionResult> GetArchivedData([FromBody] ArchiveDataRequest request)
        {
            try
            {
                var result = await _serviceManager.GetArchivedData(request.TableName, request.Filter, this.GetCurrentCulture());

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(request, ex, nameof(GetArchivedData));
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("prospect")]
        public async Task<IActionResult> CreateProspect([FromBody] ProspectRequestModel prospectRequest)
        {
            try
            {
                var result = await _serviceManager.CreateProspect(prospectRequest);

                if (!result.Success)
                {
                    Logger.ApiLog(prospectRequest, null, nameof(CreateProspect));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(prospectRequest, ex, nameof(CreateProspect));
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("customermessages/{messageType}")]
        public async Task<IActionResult> GetCustomerMessagesByCustomerId(MessagType messageType, string customerId, int pageSize, int pageIndex) =>
            Ok(await _serviceManager.GetCustomerMessagesByCustomerId(messageType, customerId, pageSize, pageIndex, this.GetCurrentCulture()));


        [HttpGet("customermessagedetails")]
        public async Task<IActionResult> GetCustomerMessagesDetails(string customerId, string customerMessageId) =>
            Ok(await _serviceManager.GetCustomerMessagesDetails(customerId, customerMessageId, this.GetCurrentCulture()));


        [HttpDelete("customermessage")]
        public async Task<IActionResult> DeleteCustomerMessage([FromBody] DeleteCustomerMessageRequest request) =>
            Ok(await _serviceManager.DeleteCustomerMessage(request));


        [HttpPost("customermessage")]
        public async Task<IActionResult> CreateCustomerMessage([FromBody] CustomerMessageRequest customerMessage)
        {
            try
            {
                var result = await _serviceManager.CreateCustomerMessage(customerMessage);

                if (!result.Success)
                {
                    Logger.ApiLog(customerMessage, null, nameof(CreateCustomerMessage));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(customerMessage, ex, nameof(CreateCustomerMessage));
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("customermessage/reply")]
        public async Task<IActionResult> CreateCustomerMessageReply([FromBody] CustomerMessageReply customerMessageReply)
        {
            try
            {
                var result = await _serviceManager.CreateCustomerMessageReply(customerMessageReply);

                if (!result.Success)
                {
                    Logger.ApiLog(customerMessageReply, null, nameof(CreateCustomerMessageReply));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(customerMessageReply, ex, nameof(CreateCustomerMessageReply));
                return BadRequest(ex.Message);
            }
        }


        //what is the usage of cusotomer id
        [HttpGet("surveytemplate")]
        public async Task<IActionResult> GetSurveyTemplateByCode(string code, string customerId) =>
           Ok(await _serviceManager.GetSurveyTemplateByCode(code, this.GetCurrentCulture()));


        [HttpGet("surveyquestions")]
        public async Task<IActionResult> GetSurveyQuestionsBySurveyCode(string surveyTemplateCode) =>
           Ok(await _serviceManager.GetSurveyQuestionsBySurveyCode(surveyTemplateCode, this.GetCurrentCulture()));


        [HttpGet("surveyanswers")]
        public async Task<IActionResult> GetSurveyAnswersByQuestionCode(string questionCode) =>
           Ok(await _serviceManager.GetSurveyAnswersByQuestionCode(questionCode, this.GetCurrentCulture()));


        [HttpGet("surveytemplateqa")]
        public async Task<IActionResult> GetSurveyTemplateQA(string templateId) =>
          Ok(await _serviceManager.GetSurveyTemplateQA(templateId));

        [MapToApiVersion(1)]
        [HttpPost("surveyresponse")]
        public async Task<IActionResult> CreateSurveyResponse([FromBody] SurveyResponse surveyResponse)
        {
            try
            {
                var result = await _serviceManager.CreateSurveyResponse(surveyResponse);

                if (!result.Success)
                {
                    Logger.ApiLog(surveyResponse, null, nameof(CreateSurveyResponse));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(surveyResponse, ex, nameof(CreateSurveyResponse));
                return BadRequest(ex.Message);
            }
        }

        [MapToApiVersion(2)]
        [HttpPost("surveyresponse")]
        public async Task<IActionResult> CreateSurveyResponseV2([FromBody] SurveyResponse surveyResponse)
        {
            try
            {
                var result = await _serviceManager.CreateSurveyResponseV2(surveyResponse);

                if (!result.Success)
                {
                    Logger.ApiLog(surveyResponse, null, nameof(CreateSurveyResponse));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(surveyResponse, ex, nameof(CreateSurveyResponse));
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CallBackRequests")]
        public async Task<IActionResult> GetCallbackRequests(string cif, string? callbackStatus) => Ok(await _serviceManager.GetCallbackRequests(cif, callbackStatus));

        [HttpPost("AssignCallback")]
        public async Task<IActionResult> AssignCallbackRequest(CallbackRequest request)
        {
            var result = await _serviceManager.AssignCallbackRequest(request);

            return Ok(result);
        }

        [HttpGet("TimeSlots")]
        public async Task<IActionResult> GetCallbackTimeslots()
        {
            var timeSlots = await _serviceManager.GetCallbackTimeslots();

            return Ok(timeSlots);
        }

        [HttpGet("skillgroups")]
        public async Task<IActionResult> GetSkillGroups()
        {
            var skillGroups = await _serviceManager.GetSkillGroups();

            return Ok(skillGroups);
        }

        [HttpGet("DoesCifExist")]
        public async Task<IActionResult> CheckCifExistance(string cif)
        {
            var result = await _serviceManager.CheckCifExistance(cif);

            return Ok(result);
        }

        [HttpPost("CreateContact")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerModel customer)
        {
            var result = await _serviceManager.CreateCustomer(customer);

            return Ok(result);
        }

        [HttpGet("EmployeeRMs")]
        public async Task<IActionResult> GetEmployeeRMs(string code)
        {
            try
            {
                var result = await _serviceManager.GetEmployeeRMs(code);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(code, ex, nameof(GetEmployeeRMs));
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("autodialerfile/{campaignId}")]
        public async Task<IActionResult> SaveCampaignInfoToFile(string campaignId)
        {
            try
            {
                var result = await _serviceManager.SaveCampaignInfoToFile(campaignId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(campaignId, ex, nameof(SaveCampaignInfoToFile));
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("tpptickets")]
        public async Task<IActionResult> GetTppTickets(int pageSize = 10, int pageIndex = 1, string? tppId = null, string? tppUserId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tppId) && string.IsNullOrWhiteSpace(tppUserId))
                {
                    return BadRequest("tppID or tppUserId should contain value");
                }

                if (pageIndex < 1)
                {
                    return BadRequest("Page index should be greater than 0");
                }

                if (pageSize < 1)
                {
                    return BadRequest("Page size should be greater than 0");
                }

                var tickets = await _serviceManager.GetTppTickets(pageSize, pageIndex, tppId, tppUserId);

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetTppTickets));

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("tpptickets")]
        public async Task<IActionResult> CreateTppTicket([FromBody] CreateTppTicketModel ticket)
        {
            if (ticket == null)
                return BadRequest(SubmissionResponse.Error("Ticket data is required."));

            try
            {
                var response = await _serviceManager.CreateTppTicket(ticket);

                if (!response.Success)
                    return StatusCode(500, response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(this, ex, nameof(CreateTppTicket), ex.Message);
                return StatusCode(500, SubmissionResponse.Error("An error occurred while saving the TPP Ticket."));
            }
        }







    }
}

