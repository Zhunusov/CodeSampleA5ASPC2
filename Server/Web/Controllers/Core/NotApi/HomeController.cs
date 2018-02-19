using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Core.NotApi
{
    /// <inheritdoc />
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
#if DEBUG
            return Redirect(Request.GetDisplayUrl() + "/swagger");
#else
            return new PhysicalFileResult(Path.Combine(_env.WebRootPath, "index.html"), "text/html");
#endif
        }
    }
}
