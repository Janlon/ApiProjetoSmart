using System.Web.Mvc;

namespace WEBAPI_VOPAK
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add((object)new HandleErrorAttribute());
        }
    }
}
