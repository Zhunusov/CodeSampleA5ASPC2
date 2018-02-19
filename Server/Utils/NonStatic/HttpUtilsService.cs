using System.Net;
using Microsoft.AspNetCore.Http;

namespace Utils.NonStatic
{
    public class HttpUtilsService : IHttpUtilsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpUtilsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IPAddress GetClientIpAddress()
        {
            return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
        }

        public string GetClientIpAddressString()
        {
            return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public string GetUserAgentString()
        {
            var collection = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
            string result = string.Empty;
            foreach (var value in collection)
            {
                result += "/n" + value;
            }

            return result;
        }
    }
}
