using Bab.BatchData.Models;
using BabCrm.Logging;
using BabCrm.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;

namespace Bab.BatchData.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchDataController : ControllerBase
    {
        private readonly BatchDataManager _batchDataManager;
        private readonly IMapper _mapper;

        public BatchDataController(BatchDataManager batchDataManager, IMapper mapper)
        {
            this._batchDataManager = batchDataManager;
            this._mapper = mapper;
    }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(Contact))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Contact not found")]
        [HttpGet("contact")]
        public async Task<IActionResult> GetContactByCIF([FromQuery][Required] int cif)
        {
            try
            {
                var contact = await _batchDataManager.GetContactByCif(cif);

                if (contact == null)
                {
                    return NotFound("Contact not found");
                }

                return Ok(contact);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetContactByCIF), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<Contact>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Contact not found")]
        [HttpPost("contacts/query")]
        public async Task<IActionResult> GetContacts([FromBody] QueryRequest queryRequest)
        {
            try
            {
                var contacts = await _batchDataManager.GetContacts(queryRequest.QueryCondition, queryRequest.Parameters);

                if (contacts == null || !contacts.Any())
                {
                    return NotFound("Contact not found");
                }

                return Ok(contacts);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(queryRequest, ex, nameof(GetContacts), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<Contact>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Contact not found")]
        [HttpPost("contacts")]
        public async Task<IActionResult> GetContacts([FromBody] ContactsFilter contactsFilter)
        {
            try
            {
                var contacts = await _batchDataManager.GetContacts(contactsFilter);

                if (contacts == null || !contacts.Any())
                {
                    return NotFound("Contact not found");
                }

                return Ok(contacts);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(contactsFilter, ex, nameof(GetContacts), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<FinancialAccountModel>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Financial accounts not found")]
        [HttpGet("financialaccounts")]
        public async Task<IActionResult> GetFinancialAccountsByCif([FromQuery][Required] int cif, bool isDistinctByCif)
        {
            try
            {
                var result = await _batchDataManager.GetFinancialAccountsByCif(cif, isDistinctByCif);

                if (result == null || !result.Any())
                {
                    return NotFound("Financial accounts not found");
                }

                var financialAccounts = _mapper.Map<IEnumerable<FinancialAccountModel>>(result);

                return Ok(financialAccounts);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetFinancialAccountsByCif), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(FinancialAccountModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Financial account not found")]
        [HttpGet("financialaccount")]
        public async Task<IActionResult> GetFinancialAccountByAccountNumber([FromQuery][Required] string accountNumber)
        {
            try
            {
                var result = await _batchDataManager.GetFinancialAccountByAccountNumber(accountNumber);

                if (result == null)
                {
                    return NotFound("Financial account not found");
                }

                var financialAccount = _mapper.Map<FinancialAccountModel>(result);

                return Ok(financialAccount);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetFinancialAccountByAccountNumber), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomFinancialAccount>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Financial accounts not found")]
        [HttpPost("financialaccounts/query")]
        public async Task<IActionResult> GetFinancialAccounts([FromBody] QueryRequest queryRequest, bool isDistinctByCif)
        {
            try
            {
                var result = await _batchDataManager.GetFinancialAccounts(queryRequest.QueryCondition, isDistinctByCif, queryRequest.Parameters);

                if (result == null || !result.Any())
                {
                    return NotFound("Financial accounts not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(queryRequest, ex, nameof(GetFinancialAccounts), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomFinancePaymentHistory>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Finance payment history not found")]
        [HttpGet("financepaymenthistory")]
        public async Task<IActionResult> GetFinancePaymentHistoryByCIF([FromQuery][Required] int cif)
        {
            try
            {
                var result = await _batchDataManager.GetFinancePaymentHistoryByCIF(cif);

                if (result == null || !result.Any())
                {
                    return NotFound("Finance payment history not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetFinancePaymentHistoryByCIF), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomFinancePaymentHistory>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Finance payment history not found")]
        [HttpPost("financepaymenthistory/query")]
        public async Task<IActionResult> GetFinancePaymentHistory([FromBody] QueryRequest queryRequest)
        {
            try
            {
                var result = await _batchDataManager.GetFinancePaymentHistory(queryRequest.QueryCondition, queryRequest.Parameters);

                if (result == null || !result.Any())
                {
                    return NotFound("Finance payment history not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(queryRequest, ex, nameof(GetFinancePaymentHistory), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomLoan>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "loans not found")]
        [HttpGet("loans")]
        public async Task<IActionResult> GetLoansByCIF([FromQuery][Required] int cif, bool isDistinctByCif)
        {
            try
            {
                var result = await _batchDataManager.GetLoansByCIF(cif, isDistinctByCif);

                if (result == null || !result.Any())
                {
                    return NotFound("loans not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetLoansByCIF), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(CustomLoan))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "loan not found")]
        [HttpGet("loan")]
        public async Task<IActionResult> GetLoanById([FromQuery][Required] string loanId)
        {
            try
            {
                var result = await _batchDataManager.GetLoanById(loanId);

                if (result == null)
                {
                    return NotFound("loan not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetLoanById), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomLoan>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "loans not found")]
        [HttpPost("loans/query")]
        public async Task<IActionResult> GetLoans([FromBody] QueryRequest queryRequest, bool isDistinctByCif)
        {
            try
            {
                var result = await _batchDataManager.GetLoans(queryRequest.QueryCondition, isDistinctByCif, queryRequest.Parameters);

                if (result == null || !result.Any())
                {
                    return NotFound("loans not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(queryRequest, ex, nameof(GetLoans), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomCreditCard>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "credit cards not found")]
        [HttpGet("creditcards")]
        public async Task<IActionResult> GetCreditCardsByCIF([FromQuery][Required] int cif, bool isDistinctByCif)
        {
            try
            {
                var result = await _batchDataManager.GetCreditCardsByCIF(cif, isDistinctByCif);

                if (result == null || !result.Any())
                {
                    return NotFound("credit cards not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetCreditCardsByCIF), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(CustomCreditCard))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "credit cards not found")]
        [HttpGet("creditcard")]
        public async Task<IActionResult> GetCreditCardByCMS([FromQuery][Required] long cms)
        {
            try
            {
                var result = await _batchDataManager.GetCreditCardByCMS(cms);

                if (result == null)
                {
                    return NotFound("credit card not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetCreditCardByCMS), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomCreditCard>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "credit cards not found")]
        [HttpPost("creditcards/query")]
        public async Task<IActionResult> GetCreditCards([FromBody] QueryRequest queryRequest, bool isDistinctByCif)
        {
            try
            {
                var result = await _batchDataManager.GetCreditCards(queryRequest.QueryCondition, isDistinctByCif, queryRequest.Parameters);

                if (result == null || !result.Any())
                {
                    return NotFound("credit cards not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(queryRequest, ex, nameof(GetCreditCards), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomDebitCard>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Debit cards not found")]
        [HttpGet("debitcards")]
        public async Task<IActionResult> GetDebitCardsByCIF([FromQuery][Required] int cif, bool isDistinctByCif)
        {
            try
            {
                var result = await _batchDataManager.GetDebitCardsByCIF(cif, isDistinctByCif);

                if (result == null || !result.Any())
                {
                    return NotFound("Debit cards not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetDebitCardsByCIF), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(CustomDebitCard))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Debit card not found")]
        [HttpGet("debitcard")]
        public async Task<IActionResult> GetDebitCardByCMS([FromQuery][Required] long cms)
        {
            try
            {
                var result = await _batchDataManager.GetDebitCardByCMS(cms);

                if (result == null)
                {
                    return NotFound("Debit card not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetDebitCardByCMS), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<CustomDebitCard>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Debit cards not found")]
        [HttpPost("debitcards/query")]
        public async Task<IActionResult> GetDebitCards([FromBody] QueryRequest queryRequest, bool isDistinctByCif)
        {
            try
            {
                var result = await _batchDataManager.GetDebitCards(queryRequest.QueryCondition, isDistinctByCif, queryRequest.Parameters);

                if (result == null || !result.Any())
                {
                    return NotFound("Debit cards not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(queryRequest, ex, nameof(GetDebitCards), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<Branch>))]
        [HttpGet("lookups/branches")]
        public async Task<IActionResult> GetBranches()
        {
            try
            {
                var result = await _batchDataManager.GetBranches();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetBranches), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }

        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<object>))]
        [HttpGet("GetProperties")]
        public async Task<IActionResult> GetProperties(string entityName)
        {
            try
            {
                var result = await _batchDataManager.GetProperties(entityName);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetProperties), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }



        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<Atm>))]
        [HttpGet("lookups/atms")]
        public async Task<IActionResult> GetAtms()
        {
            try
            {
                var result = await _batchDataManager.GetAtms();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetAtms), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<Sector>))]
        [HttpGet("lookups/sectors")]
        public async Task<IActionResult> GetSectors()
        {
            try
            {
                var result = await _batchDataManager.GetSectors();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetSectors), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }


        [SwaggerResponse(StatusCodes.Status200OK, "", typeof(IEnumerable<Department>))]
        [HttpGet("lookups/departments")]
        public async Task<IActionResult> GetDepartmentsFromDb()
        {
            try
            {
                var result = await _batchDataManager.GetDepartmentsFromDb();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(GetDepartmentsFromDb), this.GetRoute());
                return BadRequest(ex.Message);
            }
        }
    }
}