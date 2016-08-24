using System;
using System.Web.Mvc;
using MvcAuthorization;

[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof(FDB.App_Start.MvcAuthorization), "PreStart")]

namespace FDB.App_Start {
    public static class MvcAuthorization {
        public static void PreStart() {
            //GlobalFilters.Filters.Add(new AuthorizeFilter());

            GlobalFilters.Filters.Add(new MyCusAuthorizeFilter());
        }
    }


    /*
     Override lai method "HandleUnauthorizedRequest" cua class AuthorizeAttribute
     de chuyen huong den "Shared\Error.cshtml" -> Error view khi khong co quyen
     */
    public class MyCusAuthorizeFilter : AuthorizeFilter
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {            
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // user is logged-in, so redirecting to login page won't help, must be premium
                //filterContext.Result = new RedirectResult("redirect_url");                

                filterContext.Result = new ViewResult { ViewName = "Error", ViewBag = { message="Unauthorized" } };                
                //filterContext.HttpContext.Response.StatusCode = 403;
            }
            else
            {
                // let the base implementation redirect the user
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }


    
}