using System.Web;
using System.Web.Optimization;

namespace _2408.MVC
{
    public class BundleConfig
    {
        // 묶음에 대한 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=301862를 참조하세요.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/components").Include(
                      "~/Scripts/components/component.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/util.css",
                      "~/Content/common.css"
                      ));
           
        }
    }
}
