using Microsoft.AspNetCore.Http;

namespace EcoWork.Api.Utils
{
    public static class HateoasHelper
    {
        public static Dictionary<string, string> BuildLinks(HttpContext ctx, string baseRoute, int id)
        {
            string host = $"{ctx.Request.Scheme}://{ctx.Request.Host}";

            return new Dictionary<string, string>
            {
                { "self", $"{host}/{baseRoute}/{id}" },
                { "update", $"{host}/{baseRoute}/{id}" },
                { "delete", $"{host}/{baseRoute}/{id}" }
            };
        }
    }
}