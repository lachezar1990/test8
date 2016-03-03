using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace WebApplicationppp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/i18n/angular-locale_bg-bg.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-r").Include(
                "~/Scripts/angular-ui-router.js"));

            bundles.Add(new ScriptBundle("~/bundles/modules").Include(
                "~/Scripts/angular-animate.min.js",
                "~/Scripts/angular-local-storage.min.js",
                "~/Scripts/smart-table.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                "~/Scripts/ngProgress.min.js",
                "~/Scripts/angular-file-upload.min.js",
                "~/Scripts/Directives/Uploader.js",
                "~/Scripts/ng-currency.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/moment-locale.js",
                "~/Scripts/angular-bootstrap-calendar-tpls.min.js",
                "~/Scripts/angular-sanitize.js",
                "~/Scripts/select.js"));

            bundles.Add(new ScriptBundle("~/bundles/services").IncludeDirectory("~/Scripts/Factories", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                      "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/controllers").IncludeDirectory("~/Scripts/Controllers", "*.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.min.css",
                 "~/Content/Site.css",
                 "~/Content/font-awesome.min.css",
                 "~/Content/awesome-bootstrap-checkbox.css",
                 "~/Content/ngProgress.css",
                 "~/Content/spinner.css",
                 "~/Content/angular-bootstrap-calendar.css",
            "~/Content/select.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
