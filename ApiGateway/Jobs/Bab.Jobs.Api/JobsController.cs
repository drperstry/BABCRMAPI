using Bab.Jobs.Models;
using BabCrm.Logging;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bab.Jobs.Api
{
    [Route("api/[controller]")]
    public class JobsController : Controller
    {
        private readonly JobsManager _jobsManager;
        private readonly ReportsManager _reportsManager;
        private readonly IConfiguration _configuration;
        public JobsController(JobsManager jobsManager, ReportsManager reportsManager, IConfiguration configuration)
        {
            this._jobsManager = jobsManager;
            _reportsManager = reportsManager;
            _configuration = configuration;

        }

        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping() => Ok("pong");

        [AllowAnonymous]
        [HttpPost("createescalationjob")]
        public async Task<IActionResult> CreateEscalationJob([FromBody] CreateEscalationJob createEscalationJob)
        {
            string dateTimeCronExpresion = $"{createEscalationJob.Time.Minute} {createEscalationJob.Time.Hour} ";
            string cronExpresion;

            RecurrenceFrequency recurrenceFrequency = (RecurrenceFrequency)createEscalationJob.RecurrenceFrequency;

            string JobIdSuffix = $"{Enum.GetName(typeof(RecurrenceFrequency), recurrenceFrequency)}-{createEscalationJob.Time.ToUniversalTime().Hour.ToString()}";
            
            switch (recurrenceFrequency)
            {
                case RecurrenceFrequency.FirstDayOfWeek:
                    cronExpresion = dateTimeCronExpresion + "* * 0";
                    break;
                case RecurrenceFrequency.FirstDayOfMonth:
                    cronExpresion = dateTimeCronExpresion + "1 * *";
                    break;
                case RecurrenceFrequency.FirstDayOfYear:
                    cronExpresion = dateTimeCronExpresion + "1 1 *";
                    break;
                case RecurrenceFrequency.EveryDay:
                    cronExpresion = dateTimeCronExpresion + "* * *";
                    break;
                default:
                    return BadRequest($"Not supported {nameof(recurrenceFrequency)} = {recurrenceFrequency} out of range");
            }

            string serverTimeZone = _configuration.GetValue<string>("ServerTimeZone");

            RecurringJob.AddOrUpdate($"EscalationJob-{JobIdSuffix}", () => _jobsManager.CreateEscalationJob(createEscalationJob.EntityId), cronExpresion, timeZone: TimeZoneInfo.FindSystemTimeZoneById(serverTimeZone));

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("UpsertScheduleTimerRunWorkflow")]
        public IActionResult CreateWorkflowJob([FromBody] WorkflowJobRequest workflowJobRequest)
        {
            string dateTimeCronExpresion = $"{workflowJobRequest.Time.Minute} {workflowJobRequest.Time.Hour} ";
            string cronExpresion;

            RecurrenceFrequency recurrenceFrequency = (RecurrenceFrequency)workflowJobRequest.RecurrenceFrequency;

            switch (recurrenceFrequency)
            {
                case RecurrenceFrequency.FirstDayOfWeek:
                    cronExpresion = dateTimeCronExpresion + "* * 0";
                    break;
                case RecurrenceFrequency.FirstDayOfMonth:
                    cronExpresion = dateTimeCronExpresion + "1 * *";
                    break;
                case RecurrenceFrequency.FirstDayOfYear:
                    cronExpresion = dateTimeCronExpresion + "1 1 *";
                    break;
                case RecurrenceFrequency.EveryDay:
                    cronExpresion = dateTimeCronExpresion + "* * *";
                    break;
                default:
                    return BadRequest($"Not supported {nameof(recurrenceFrequency)} = {recurrenceFrequency} out of range");
            }

            RecurringJob.AddOrUpdate(workflowJobRequest.JobId, () => _jobsManager.ExecuteWorflow(workflowJobRequest.WorkflowId, workflowJobRequest.EntityId), cronExpresion, timeZone: TimeZoneInfo.Utc);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<IActionResult> test()
        {
            var result = await _jobsManager.GetEscalationInfoByLevel(1);

            //RecurringJob.AddOrUpdate("EscalationJob", () => _jobsManager.CreateEscalationJob(), "00 10 * * 1", timeZone: TimeZoneInfo.Utc);
            //RecurringJob.AddOrUpdate("EscalationJob", () => _jobsManager.CreateEscalationJob(), "00 10 * * 1", timeZone: TimeZoneInfo.FindSystemTimeZoneById("Middle East Standard Time"));

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] string configurationGuid)
        {
            var result = await _jobsManager.SendEmailReport(configurationGuid);

            return Ok(result);
        }

        [HttpPost]
        [Route("timer")]
        public IActionResult UpdateCRM([FromBody] RequestData request)
        {
            var jobId = _jobsManager.SaveDueDate(request);

            return Ok(new { JobId = jobId });
        }

        [HttpDelete]
        [Route("{jobId}")]
        public IActionResult DeleteJob(string jobId, bool isRecurringJob)
        {
            try
            {
                var response = _jobsManager.DeleteJob(jobId, isRecurringJob);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("segment/{segmentCampaignId}/sendnotification")]
        public async Task<IActionResult> SendCustomNotification(string segmentCampaignId)
        {
            try
            {
                var response = await _jobsManager.SendNotificationBySegment(segmentCampaignId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(segmentCampaignId, ex, nameof(SendCustomNotification));
                return BadRequest(ex.Message);
            }
        }

    }
}
