using System.Web.Optimization;

namespace AzureIoT.FrontEnd
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js"
                   , "~/Scripts/twitter-bootstrap-hover-dropdown.js"
                    ));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Theme/base/css").Include(
                        "~/Content/Theme/base/jquery.ui.core.css",
                        "~/Content/Theme/base/jquery.ui.resizable.css",
                        "~/Content/Theme/base/jquery.ui.selectable.css",
                        "~/Content/Theme/base/jquery.ui.accordion.css",
                        "~/Content/Theme/base/jquery.ui.autocomplete.css",
                        "~/Content/Theme/base/jquery.ui.button.css",
                        "~/Content/Theme/base/jquery.ui.dialog.css",
                        "~/Content/Theme/base/jquery.ui.slider.css",
                        "~/Content/Theme/base/jquery.ui.tabs.css",
                        "~/Content/Theme/base/jquery.ui.datepicker.css",
                        "~/Content/Theme/base/jquery.ui.progressbar.css",
                        "~/Content/Theme/base/jquery.ui.theme.css"));



            bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
  "~/Content/Theme/vendors/uniform/jquery.uniform.js"
  , "~/Content/Theme/vendors/chosen.jquery.js"
  , "~/Content/Theme/vendors/bootstrap-datepicker/js/bootstrap-datepicker.js"
  , "~/Content/Theme/vendors/bootstrap-wysihtml5-rails-b3/vendor/assets/javascripts/bootstrap-wysihtml5/wysihtml5.js"
  , "~/Content/Theme/vendors/bootstrap-wysihtml5-rails-b3/vendor/assets/javascripts/bootstrap-wysihtml5/core-b3.js"
  , "~/Content/Theme/vendors/twitter-bootstrap-wizard/jquery.bootstrap.wizard-for.bootstrap3.js"
  , "~/Content/Theme/vendors/boostrap3-typeahead/bootstrap3-typeahead.js"
  , "~/Content/Theme/vendors/easypiechart/jquery.easy-pie-chart.js"
  , "~/Content/Theme/vendors/ckeditor/ckeditor.js"
  , "~/Content/Theme/vendors/tinymce/js/tinymce/tinymce.js"
  , "~/Content/Theme/vendors/bootstrap-wysihtml5-rails-b3/vendor/assets/javascripts/bootstrap-wysihtml5/wysihtml5.js"
  , "~/Content/Theme/vendors/bootstrap-wysihtml5-rails-b3/vendor/assets/javascripts/bootstrap-wysihtml5/core-b3.js"
  , "~/Content/Theme/vendors/jGrowl/jquery.jgrowl.js"
  , "~/Content/Theme/vendors/bootstrap-datepicker/js/bootstrap-datepicker.js"
  , "~/Content/Theme/vendors/sparkline/jquery.sparkline.js"
  , "~/Content/Theme/vendors/tablesorter/js/jquery.tablesorter.js"
  , "~/Content/Theme/vendors/flot/jquery.flot.js"
  , "~/Content/Theme/vendors/flot/jquery.flot.selection.js"
  , "~/Content/Theme/vendors/flot/jquery.flot.resize.js"
  , "~/Content/Theme/vendors/fullcalendar/fullcalendar.js"
  , "~/Scripts/plugins/jquery/jquery.min.js"
  , "~/Scripts/plugins/bootstrap/js/bootstrap.js"
  , "~/Scripts/plugins/bootstrap-select/js/bootstrap-select.js"
  , "~/Scripts/plugins/jquery-slimscroll/jquery.slimscroll.js"
  , "~/Scripts/plugins/node-waves/waves.js"
  , "~/Scripts/plugins/jquery-countto/jquery.countTo.js"
  , "~/Scripts/plugins/raphael/raphael.min.js"
  , "~/Scripts/plugins/morrisjs/morris.js"
  , "~/Scripts/plugins/chartjs/Chart.bundle.js"
  , "~/Scripts/plugins/flot-charts/jquery.flot.js"
  , "~/Scripts/plugins/flot-charts/jquery.flot.resize.js"
  , "~/Scripts/plugins/flot-charts/jquery.flot.pie.js"
  , "~/Scripts/plugins/flot-charts/jquery.flot.categories.js"
  , "~/Scripts/plugins/flot-charts/jquery.flot.time.js"
  , "~/Scripts/plugins/jquery-sparkline/jquery.sparkline.js"
  , "~/Scripts/admin.js"
  , "~/Scripts/pages/index.js"
  , "~/Scripts/demo.js"
  , "~/Scripts/ui/modal.js"
  , "~/Scripts/forms/basic-form-elements.js"
                 ));



            bundles.Add(new StyleBundle("~/Content/Theme").Include(
                         "~/Scripts/plugins/bootstrap/css/bootstrap.css"
                        , "~/Scripts/plugins/node-waves/waves.css"
                        , "~/Scripts/plugins/animate-css/animate.css"
                        , "~/Scripts/plugins/morrisjs/morris.css"
                        , "~/Content/style.css"
                        , "~/Content/themes/all-themes.css"));


        }
    }
}