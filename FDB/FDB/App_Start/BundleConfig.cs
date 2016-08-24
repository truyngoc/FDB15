using System.Web;
using System.Web.Optimization;

namespace FDB
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/datepicker.js",
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/jquery.validate.mvc.js",
                      "~/Scripts/bootstrap-hover-dropdown.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/PagedList.css",
                      "~/Content/toastr.css",
                      "~/Content/jquery.validate.mvc.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/site.css",
                      "~/Content/my-themes/jquery-ui.min.css"));

            // add jqueryUI
            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/core.css",
            //            "~/Content/themes/base/datepicker.css",
            //            "~/Content/themes/base/resizable.css",
            //            "~/Content/themes/base/selectable.css",
            //            "~/Content/themes/base/accordion.css",
            //            "~/Content/themes/base/autocomplete.css",
            //            "~/Content/themes/base/button.css",
            //            "~/Content/themes/base/dialog.css",
            //            "~/Content/themes/base/slider.css",
            //            "~/Content/themes/base/tabs.css",
            //            "~/Content/themes/base/progressbar.css",
            //            "~/Content/themes/base/theme.css")); 

            // use customs themes jqueryUI (thay cho cai default o tren)
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}-custom.js"));

            //bundles.Add(new StyleBundle("~/Content/my-themes").Include(
            //            "~/Content/my-themes/jquery-ui.css",
            //            "~/Content/my-themes/jquery-ui.min.css")); 



            bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                        "~/Scripts/modalform.js"));

            // angularJS
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js"));
        }
    }
}
