using Bab.Attachments.Models;
using BabCrm.Core;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace Bab.Attachments
{
    public class AttachmentsManager
    {
        private readonly string sharedFolderPath;

        public AttachmentsManager(IConfiguration configuration)
        {
            sharedFolderPath = configuration.GetValue<string>("SharedFolderPath");
        }

        public async Task<SubmissionResponse> UploadAttachment(Attachment attachment)
        {
            try
            {
                // Convert base64 string to byte array
                byte[] fileBytes = Convert.FromBase64String(attachment.Base64Data);

                // Save the attachment to the shared folder
                string fileName = attachment.FileName;
                string filePath = Path.Combine(attachment.FolderPath, attachment.RecordId, fileName);
                Directory.CreateDirectory($@"{attachment.FolderPath}\{attachment.RecordId}");
                await File.WriteAllBytesAsync(filePath, fileBytes);

                int lastIndex = filePath.LastIndexOf('\\');

                filePath = filePath.Substring(0, lastIndex);

                var result = new UploadAttachmentResponse
                {
                    AttachmentName = fileName.RemoveExtension(),
                    FileName = fileName,
                    FilePath = filePath
                };

                return SubmissionResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return SubmissionResponse.Error(ex.Message);
            }
        }

        public async Task<SubmissionResponse> DeleteAttachment(string filePath)
        {
            try
            {
                // Delete the file from the shared folder
                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));
                    return SubmissionResponse.Ok();
                }

                return SubmissionResponse.Error("Not found");
            }
            catch (Exception ex)
            {
                // Handle and log any errors
                return SubmissionResponse.Error(ex.Message);
            }
        }



        public async Task<SubmissionResponse> GetAttachment(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return SubmissionResponse.Error("File path cannot be null or empty.");
            }

            if (!File.Exists(filePath))
            {
                return SubmissionResponse.Error("Specified file does not exist.");
            }

            try
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    byte[] fileBytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(fileBytes, 0, (int)fileStream.Length);

                    string base64String = Convert.ToBase64String(fileBytes);

                    return SubmissionResponse.Ok(new { Base64String = base64String });
                }
            }
            catch (Exception ex)
            {
                return SubmissionResponse.Error($"An error occurred while converting the file to base64: {ex.Message}");
            }
        }

    }


}