using System.Web;
using System.Web.Mvc;

namespace WebApplication_MVC_AAD_from_template
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
