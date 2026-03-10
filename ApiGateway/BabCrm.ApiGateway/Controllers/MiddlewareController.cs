using BabCrm.Service.Models;
using BabCrm.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BabCrm.Core;
using BabCrm.Logging;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Hangfire;

namespace BabCrm.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiddlewareController : ControllerBase
    {
        private MWManager _manager;

        public MiddlewareController(MWManager manager)
        {
            _manager = manager;
        }

        [HttpPost("NotificationRequest")]
        public async Task<IActionResult> SendNotification([FromBody] SendInstantNotification notification)
        {
            try
            {
                var jobId = BackgroundJob.Enqueue(() => _manager.CallSendInstantNotification(notification));
                //var result = await _manager.CallSendInstantNotification(notification);

                return Ok(jobId);
            }
            catch (Exception ex)
            {
                Logger.ApiLog("error while calling the notification service", ex, nameof(SendNotification));
                return StatusCode(500,ex.Message);
            }
        }
    }
}
