using System.Web;
using System.Web.Mvc;

namespace EuroManager.WorldSimulator.Presentation.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}