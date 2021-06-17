using System.Web.Optimization;

namespace Save_Our_Pets
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css")
           .Include(
                "~/Content/style.css",
               "~/Content/css/navigation-menu.css",
               "~/Content/css/plugins.css",
               "~/Content/css/shortcodes.css",
               "~/Content/css/animate.min.css",
               "~/Content/css/alertify.css"));

            bundles.Add(new StyleBundle("~/bundles/libraries")
          .Include(
              "~/Content/libraries/animate/animate.min.css",
              "~/Content/libraries/bootstrap/bootstrap.min.css",
              "~/Content/libraries/loader/loaders.min.css",
              "~/Content/libraries/owl-carousel/owl.carousel.css",
              "~/Content/libraries/owl-carousel/owl.theme.css",
              "~/Content/libraries/price-filter/query-ui.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include(
                "~/Scripts/html5/respond.min.js",
                "~/Scripts/functions.js",
                "~/Scripts/jquery.easing.min.js",
                "~/Scripts/libraries/appear/jquery.appear.js",
                "~/Scripts/jquery.min.js",
                "~/Scripts/alertify.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jsval")
              .Include("~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/libraries_js")
                .Include(
                "~/Scripts/libraries/bootstrap/bootstrap.min.js",
                "~/Scripts/libraries/modernizr/modernizr.js",
                "~/Scripts/libraries/number/jquery.animateNumber.min.js",
                "~/Scripts/libraries/owl-carousel/owl.carousel.min.js",
                 "~/Scripts/libraries/price-filter/query-ui.min.js"
                ));

            

        }

    }
}