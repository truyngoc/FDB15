using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FDB.Common;

namespace FDB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // change ViewEngine location
            ViewEngines.Engines.Clear();
            var viewEngine = new FDB.Common.CustomViewLocationRazorViewEngine();

            ViewEngines.Engines.Add(viewEngine);

            // customs modelbinder for HTTP GET method (dateTime)
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());


            // customs model binder for (decimal)
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

        }
    }

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ImageActionLink(this HtmlHelper htmlHelper, string linkText, string action, string controller, object routeValues, object htmlAttributes, string imageSrc)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var img = new TagBuilder("img");

            img.Attributes.Add("src", VirtualPathUtility.ToAbsolute(imageSrc));
            var anchor = new TagBuilder("a") { InnerHtml = img.ToString(TagRenderMode.SelfClosing) };
            anchor.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(anchor.ToString());

        }
    }
}
