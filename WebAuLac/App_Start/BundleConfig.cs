using System.Web;
using System.Web.Optimization;

namespace WebAuLac
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
  .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/jqueryui")
               .Include("~/Content/themes/base/all.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/myStyle.css"));
            //thêm jquery plugin datatables
            bundles.Add(new ScriptBundle("~/vendors/DataTables/js").Include("~/vendors/DataTables/datatables.js"));
            bundles.Add(new StyleBundle("~/vendors/DataTables/css").Include("~/vendors/DataTables/datatables.css"));

            bundles.Add(new ScriptBundle("~/vendors/DataTables/Buttons/js").Include(
                "~/vendors/DataTables/Buttons/js/buttons.flash.min.js",
                "~/vendors/DataTables/Buttons/js/buttons.html5.min.js",
                "~/vendors/DataTables/Buttons/js/buttons.print.min.js",
                "~/vendors/DataTables/Buttons/js/dataTables.buttons.min.js",
                "~/vendors/DataTables/Buttons/js/jszip.min.js",
                "~/vendors/DataTables/Buttons/js/pdfmake.min.js",
                "~/vendors/DataTables/Buttons/js/vfs_fonts.js"
                ));
            bundles.Add(new StyleBundle("~/vendors/DataTables/Buttons/css").Include("~/vendors/DataTables/Buttons/css/buttons.dataTables.min.css"));

            //thêm plugin checkbox
            bundles.Add(new ScriptBundle("~/vendors/DataTables/checkbox/js").Include("~/vendors/DataTables/checkbox/js/dataTables.checkboxes.min.js"));
            bundles.Add(new StyleBundle("~/vendors/DataTables/checkbox/css").Include("~/vendors/DataTables/checkbox/css/dataTables.checkboxes.css"));

            //thêm font-awesome
            bundles.Add(new StyleBundle("~/vendors/fontawesome/css").Include("~/vendors/fontawesome/css/font-awesome.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/modalform").Include("~/Scripts/modalform.js"));

            //jsTree
            bundles.Add(new ScriptBundle("~/vendors/jsTree/js").Include(
                        "~/vendors/jsTree/jstree.min.js"));

            bundles.Add(new StyleBundle("~/vendors/jsTree/css").Include(
                               "~/vendors/jsTree/themes/default/style.min.css"));
        }
    }
}
