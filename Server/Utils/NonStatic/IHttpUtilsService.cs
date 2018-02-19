using System.Net;

namespace Utils.NonStatic
{
    public interface IHttpUtilsService
    {
        IPAddress GetClientIpAddress();
        string GetClientIpAddressString();
        string GetUserAgentString();
    }
}