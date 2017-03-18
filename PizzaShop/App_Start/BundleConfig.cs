using System.Web;
using System.Web.Optimization;

namespace PizzaShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUI").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/adminCss").Include(
                       "~/Content/bootstrap.css",
                       "~/Content/AdminSite.css"));

            bundles.Add(new ScriptBundle("~/bundles/basic").Include(
                       "~/Scripts/OwnScripts/navigation.js",
                       "~/Scripts/OwnScripts/basicScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                     "~/Scripts/OwnScripts/navigation.js",
                     "~/Scripts/OwnScripts/modalForm.js",
                     "~/Scripts/OwnScripts/adminScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/customValidation").Include(
                      "~/Scripts/OwnScripts/customValidation.js"));

            bundles.Add(new ScriptBundle("~/bundles/dragAndDropVal").Include(
                     "~/Scripts/OwnScripts/dragAndDrop.js",
                     "~/Scripts/OwnScripts/customValidation.js"));
        }
    }
}
