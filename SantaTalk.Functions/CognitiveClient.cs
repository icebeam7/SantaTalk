using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace SantaTalk.Functions
{
    public static class CognitiveClient
    {
        public static ApiKeyServiceClientCredentials GetCredentials() =>
            new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("APIKey"));

        public static string GetEndpoint() => Environment.GetEnvironmentVariable("APIEndpoint");
    }
}
