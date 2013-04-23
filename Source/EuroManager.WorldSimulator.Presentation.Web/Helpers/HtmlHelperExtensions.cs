using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EuroManager.WorldSimulator.Presentation.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName,
            object routeValues = null, bool? isActive = null)
        {
            var tag = new TagBuilder("li");
            tag.InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, null).ToHtmlString();

            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            if (isActive == null)
            {
                if (string.Equals(actionName, currentAction, StringComparison.InvariantCultureIgnoreCase) &&
                    string.Equals(controllerName, currentController, StringComparison.InvariantCultureIgnoreCase))
                {
                    tag.AddCssClass("active");
                }
            }
            else if (isActive.Value)
            {
                tag.AddCssClass("active");
            }

            string html = tag.ToString();
            return new MvcHtmlString(html);
        }

        public static MvcHtmlString TeamLink(this HtmlHelper htmlHelper, int teamId, string teamName)
        {
            return htmlHelper.ActionLink(teamName, "Team", "World", new { id = teamId }, new { @class = "team-link" });
        }
    }
}