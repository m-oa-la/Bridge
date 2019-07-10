using System;
using System.Web.Optimization;

namespace BridgeMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Bundle for common functions.
            bundles.Add(new ScriptBundle("~/bundles/scripts/common").Include(
                "~/Scripts/Common/*.js"));

            // Module bundles. Each modules has two bundles; one for scripts and one for styles.
            bundles.Add(new ScriptBundle("~/bundles/scripts/m1").Include(
                "~/Scripts/M1/*.js"));

            // Enable optimizations, even when debug is disabled
            BundleTable.EnableOptimizations = true;
        }
    }
}