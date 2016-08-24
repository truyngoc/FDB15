using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace FDB.Helpers
{
    public static class MyHelpers
    {
        // As the text the: "<span class='glyphicon glyphicon-plus'></span>" can be entered
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                             string text, string title, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            
            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = text;
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }
    }

    public static class AuthorizeActionLinkExtention
    {
        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            if (HasActionPermission(helper, actionName, controllerName))
                return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);

            return MvcHtmlString.Empty;
        }
        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            if (HasActionPermission(helper, actionName, controllerName))
                return helper.ActionLink(linkText, actionName, controllerName);

            return MvcHtmlString.Empty;
        }
        public static MvcHtmlString AuthorizeActionLink(this System.Web.Mvc.HtmlHelper helper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (HasActionPermission(helper, actionName, controllerName))
                return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);            
            
            return MvcHtmlString.Empty;
        }
        static bool HasActionPermission(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            ControllerBase controllerToLinkTo = string.IsNullOrEmpty(controllerName)
                ? htmlHelper.ViewContext.Controller
                : GetControllerByName(htmlHelper, controllerName);

            ControllerContext controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerToLinkTo);

            ReflectedControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(controllerToLinkTo.GetType());
            ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);
            
            return ActionIsAuthorized(controllerContext, actionDescriptor);
        }

        static bool ActionIsAuthorized(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
                return false;

            AuthorizationContext authContext = new AuthorizationContext(controllerContext, actionDescriptor);
            foreach (Filter authFilter in FilterProviders.Providers.GetFilters(authContext, actionDescriptor))
            {
                if (authFilter.Instance is System.Web.Mvc.AuthorizeAttribute)
                {                  

                    ((IAuthorizationFilter)authFilter.Instance).OnAuthorization(authContext);

                    if (authContext.Result != null)
                        return false;
                }
            }

            return true;
        }

        static ControllerBase GetControllerByName(HtmlHelper helper, string controllerName)
        {
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();

            IController controller = factory.CreateController(helper.ViewContext.RequestContext, controllerName);

            if (controller == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        System.Globalization.CultureInfo.CurrentUICulture,
                        "Controller factory {0} controller {1} returned null",
                        factory.GetType(),
                        controllerName));
            }

            return (ControllerBase)controller;
        }

    }
    public static class MyExtensions
    {
        public static MvcHtmlString EncodedActionLink(this UrlHelper Helper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            string htmlAttributesString = string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "?";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            //<a href="/Answer?questionId=14">What is Entity Framework??</a>
            StringBuilder ancor = new StringBuilder();
            ancor.Append("<a ");
            if (htmlAttributesString != string.Empty)
            {
                ancor.Append(htmlAttributesString);
            }
            ancor.Append(" href='");
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
            }

            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                ancor.Append("?q=" + Encrypt(queryString));
            }
            ancor.Append("'");
            ancor.Append(">");
            ancor.Append(linkText);
            ancor.Append("</a>");
            return new MvcHtmlString(ancor.ToString());
        }
        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string linkText,string imgURL, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            string htmlAttributesString = string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "?";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=\"" + d.Values.ElementAt(i)+"\"";
                }
            }

            //<a href="/Answer?questionId=14">What is Entity Framework??</a>
            StringBuilder ancor = new StringBuilder();
            ancor.Append("<a ");
            if (htmlAttributesString != string.Empty)
            {
                ancor.Append(htmlAttributesString);
            }
            ancor.Append(" href='");
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
            }

            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                ancor.Append("?q=" + Encrypt(queryString));
            }
            ancor.Append("'");
            ancor.Append(">");
            ancor.Append(linkText);
            ancor.Append(imgURL);
            ancor.Append("</a>");
            return new MvcHtmlString(ancor.ToString());
        }

        private static string Encrypt(string plainText)
        {
            string key = "jdsg432387#";
            byte[] EncryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            EncryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
            if (HttpContext.Current.Request.QueryString.Get("q") != null)
            {
                string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("q");
                string decrptedString = Decrypt(encryptedQueryString.ToString());
                if (!String.IsNullOrEmpty(decrptedString))
                {
                    string[] paramsArrs = decrptedString.Split('?');

                    for (int i = 0; i < paramsArrs.Length; i++)
                    {
                        string[] paramArr = paramsArrs[i].Split('=');
                        if (paramArr.Length > 1)
                            decryptedParameters.Add(paramArr[0], String.IsNullOrEmpty(paramArr[1]) ? (object)"0" : (object)paramArr[1]);
                    }
                }

            }

            if (decryptedParameters.Count > 0)
            {
                for (int i = 0; i < decryptedParameters.Count; i++)
                {
                    filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
                }
                base.OnActionExecuting(filterContext);
            }


        }

        private string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText) || encryptedText == null)
                return string.Empty;
            encryptedText = encryptedText.Replace(" ", "+");
            int mod4 = encryptedText.Length % 4;
            if (mod4 > 0)
            {
                encryptedText += new string('=', 4 - mod4);
                return string.Empty;
            }
            string key = "jdsg432387#";
            byte[] DecryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            byte[] inputByte = new byte[encryptedText.Length];

            DecryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                inputByte = Convert.FromBase64String(encryptedText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DecryptKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByte, 0, inputByte.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (System.FormatException fex)
            {
                return string.Empty;
            }



        }
    }
}