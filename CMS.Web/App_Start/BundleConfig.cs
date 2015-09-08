using System.Web;
using System.Web.Optimization;

namespace CMS.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/scripts/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/bs").Include(
                "~/Content/scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqui").Include(
                        "~/Content/plugins/ui/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/scripts/jquery.unobtrusive*",
                        "~/Content/scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/ueditor").Include(
                        "~/Content/plugins/ueditor/ueditor.config.js",
                        "~/Content/plugins/ueditor/ueditor.all.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqplugins").Include(
                        "~/Content/scripts/jquery.bganimate.js",
                        "~/Content/scripts/jquery.slimscroll.js",
                        "~/Content/scripts/jquery.powertip.js",
                        "~/Content/scripts/jquery.fancybox.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/Content/scripts/main.js"
                ));


            //// 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            //// 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/styles/css").Include(
                        "~/Content/styles/bootstrap.css",
                        "~/Content/styles/font-awesome.css"
                        ));
            //bundles.Add(new StyleBundle("~/Content/font-awesome/fa").Include(
            //            "~/Content/font-awesome/css/font-awesome.css"
            //            ));

            bundles.Add(new StyleBundle("~/Content/jqui").Include(
                            "~/Content/plugins/ui/jquery-ui.min.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/ueditor").Include(
                            "~/Content/plugins/ueditor/themes/default/css/ueditor.min.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/styles/st").Include(
                            "~/Content/styles/jquery.fancybox.css",
                            "~/Content/styles/jquery.powertip.css",
                            "~/Content/styles/style.css",
                            "~/Content/styles/bootcss.css",
                            "~/Content/styles/media.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/flags/banner").Include("~/Content/flags/famfamfam-flags.css"));

            //if (System.Configuration.ConfigurationManager.AppSettings["EnableOptimizations"].ToString().Equals("1"))
            //{
            //    BundleTable.EnableOptimizations = true;
            //}

            BundleTable.EnableOptimizations = false;
        }
    }
}