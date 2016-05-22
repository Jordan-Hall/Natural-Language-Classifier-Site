using System.Web;
using System.Web.Mvc;

namespace JordanHall.Ibm.Nlc.SiteApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
