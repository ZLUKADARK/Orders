using Microsoft.AspNetCore.Http;
using System.Threading;

namespace Microsoft.AspNetCore.Http
{
    public class _HttpContextAccessor : IHttpContextAccessor
    {
        private static AsyncLocal<HttpContext> _httpContextCurrent = new AsyncLocal<HttpContext>();
        HttpContext IHttpContextAccessor.HttpContext { get => _httpContextCurrent.Value; set => _httpContextCurrent.Value = value; }
    }
}
