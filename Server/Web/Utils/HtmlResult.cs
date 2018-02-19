using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Utils
{
    internal sealed class HtmlResult : IActionResult
    {
        readonly string _htmlBodyCode;

        private readonly string _title;

        public HtmlResult(string htmlBodyCode, string title = "Web")
        {
            _htmlBodyCode = htmlBodyCode;
            _title = title;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            string fullHtmlCode = "<!DOCTYPE html><html><head>";
            fullHtmlCode += $"<title>{_title}</title>";
            fullHtmlCode += "<meta charset=utf-8 />";
            fullHtmlCode += "</head> <body>";
            fullHtmlCode += _htmlBodyCode;
            fullHtmlCode += "</body></html>";
            await context.HttpContext.Response.WriteAsync(fullHtmlCode);
        }
    }
}
