using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeiPo.Services;

namespace WeiPo.Common
{
    public class WeiboHttpClientHandler : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Cookie", string.Join(";",
                Singleton<Api>.Instance.GetCookies().Select(it => $"{it.Key}={it.Value}")));
            return base.SendAsync(request, cancellationToken);
        }
    }
}