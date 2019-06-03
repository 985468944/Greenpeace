using System.Web;
using System.Web.Optimization;

namespace CloudDataMar.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/script/bootstrapjquery").Include(
                 "~/Content/plugins/jquery-1.10.2.min.js",
                 "~/Content/plugins/jquery.cokie.min.js",
                 "~/Content/AdminStyle/js/app.min.js",
                 "~/Content/bootstrap-3.3.5/js/bootstrap.min.js",
                 "~/Content/plugins/jquery.form.js",
                 "~/Content/plugins/jquery.validate.js",
                 "~/Content/jquery-confirm/js/jquery-confirm.js",
                 "~/Content/Script/Common.js",
                 "~/Content/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js",
                 "~/Content/Script/jquery.liMarquee.js"
                 ));

            bundles.Add(new StyleBundle("~/css/bootstrapcss").Include(
                "~/Content/bootstrap-3.3.5/css/bootstrap.min.css",
                "~/Content/jquery-confirm/css/jquery-confirm.css",
                "~/Content/bootstrap-datetimepicker/css/datetimepicker.css",
                "~/Content/Script/base/jquery.ui.datepicker.css",
                "~/Content/Css/liMarquee.css"
                ));

            bundles.Add(new StyleBundle("~/css/style").Include(
                "~/Content/Css/style.css",
                "~/Content/css/common.css",
                "~/Content/AdminStyle/css/AdminLTE.min.css",
                "~/Content/AdminStyle/css/skins/skin-blue-light.css"));

            bundles.Add(new StyleBundle("~/css/datatables").Include(
                "~/Content/DataTables-1.10.8/css/jquery.dataTables.min.css",
                "~/Content/DataTables-1.10.8/css/dataTables.bootstrap.min.css",
                "~/Content/DataTables-1.10.8/css/dataTables.foundation.min.css",
                "~/Content/DataTables-1.10.8/css/dataTables.jqueryui.min.css"));

            bundles.Add(new StyleBundle("~/css/uploadify").Include(
                "~/Content/uploadify-3.2.1/uploadify.css"));

            bundles.Add(new ScriptBundle("~/script/datatables").Include(
                "~/Content/DataTables-1.10.8/js/jquery.dataTables.min.js",
                "~/Content/DataTables-1.10.8/js/dataTables.bootstrap.min.js",
                "~/Content/DataTables-1.10.8/js/dataTables.foundation.min.js",
                "~/Content/DataTables-1.10.8/js/dataTables.jqueryui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/uploadify").Include(
                "~/Content/uploadify-3.2.1/jquery.uploadify.js"));
        }
    }
}