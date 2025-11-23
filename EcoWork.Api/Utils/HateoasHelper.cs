using Microsoft.AspNetCore.Mvc;

namespace EcoWork.Api.Utils
{
    public static class HateoasHelper
    {
        public static Dictionary<string, string> BuildLinks(
            IUrlHelper url,
            string controller,
            int id
        )
        {
            return new Dictionary<string, string>
            {
                { "self", url.Action("GetById", controller, new { id })! },
                { "update", url.Action("Update", controller, new { id })! },
                { "delete", url.Action("Delete", controller, new { id })! }
            };
        }
    }
}
