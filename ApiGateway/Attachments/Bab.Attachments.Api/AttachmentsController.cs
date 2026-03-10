using Bab.Attachments.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Bab.Attachments.Api
{
    [Route("api/[controller]")]
    public class AttachmentsController : Controller
    {
        private readonly AttachmentsManager _attachmentsManager;
       
        public AttachmentsController(AttachmentsManager attachmentsManager)
        {
            this._attachmentsManager = attachmentsManager;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAttachment([FromBody] Attachment attachment)
        {
            try
            {
                var result = await _attachmentsManager.UploadAttachment(attachment);
                
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                // Handle and log any errors
                return StatusCode(500, new { ErrorMessage = ex.Message });
            }
        }

        [HttpDelete("{filePath}")]
        public async Task<IActionResult> DeleteFile(string filePath)
        {
            try
            {
                var result = await _attachmentsManager.DeleteAttachment(filePath);

                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                // Handle and log any errors
                return StatusCode(500, new { ErrorMessage = ex.Message });
            }
        }

        [HttpGet("{filePath}")]
        public async Task<IActionResult> GetAttachment(string filePath)
        {
            try
            {
                var result = await _attachmentsManager.GetAttachment(filePath);

                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                // Handle and log any errors
                return StatusCode(500, new { ErrorMessage = ex.Message });
            }
        }
    }
}
