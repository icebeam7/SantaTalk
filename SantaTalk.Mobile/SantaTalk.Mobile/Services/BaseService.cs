using System;
using System.Net.Http;

using Xamarin.Essentials;

namespace SantaTalk.Services.Base
{
    public class BaseService
    {
        //string santaUrl = "{REPLACE WITH YOUR FUNCTION URL}/api/";

        protected string santaUrl = "http://192.168.0.107:7071/api/";
        protected static HttpClient httpClient = new HttpClient();

        public BaseService()
        {
            if (httpClient.BaseAddress == null)
                httpClient.BaseAddress = new Uri(santaUrl);
        }
    }
}