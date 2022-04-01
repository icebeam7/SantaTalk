using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace SantaTalk.Functions
{
    public static class ScanSanta
    {
        static ComputerVisionClient visionClient;
        private const int numberOfCharsInOperationId = 36;

        static ScanSanta()
        {
            var keys = CognitiveClient.GetCredentials();
            visionClient = new ComputerVisionClient(keys) { Endpoint = CognitiveClient.GetEndpoint() };
        }

        [FunctionName("ScanSanta")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] Stream image,
            ILogger log)
        {
            string text;

            try
            {
                var result = await visionClient.ReadInStreamAsync(image);
                text = await GetTextAsync(result.OperationLocation);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(text);
        }

        private static async Task<string> GetTextAsync(string operationLocation)
        {
            var operationId = operationLocation.Substring(
                operationLocation.Length - numberOfCharsInOperationId);
            var guid = Guid.Parse(operationId);

            var result = await visionClient.GetReadResultAsync(guid);

            int i = 0;
            int maxRetries = 10;

            while ((result.Status == OperationStatusCodes.Running ||
                    result.Status == OperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
                await Task.Delay(1000);
                result = await visionClient.GetReadResultAsync(guid);
            }

            var sb = new StringBuilder();

            foreach (var line in result.AnalyzeResult.ReadResults)
            {
                foreach (var word in line.Lines)
                {
                    sb.Append(word.Text);
                    sb.Append("\r\n");
                }
            }

            return sb.ToString();
        }
    }
}
