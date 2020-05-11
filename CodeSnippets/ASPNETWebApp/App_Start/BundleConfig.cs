using System.Web;
using System.Web.Optimization;

//If you determine that your application will benefit from bundling, you can create bundles in the 
//BundleConfig.cs file with the following code:

//bundles.Add(new ScriptBundle("~/bundles/myBundle").Include("~/Scripts/myScript1.js",
//"~/Scripts/myScript2.js",
//"~/Scripts/myScript3.js"));

//You are telling the server to create a new script, myBundle, made up of myScript1.js, myScript2.js, 
//and myScript3.js; and add the new script to the bundle collection.The bundle collection is a set of 
//the bundles that are available to your application.Although you can refer to the new script in a 
//direct script link, just as you would one of the scripts being bundled, the bundle functionality 
//gives you another path to put this script into your page: //@BundleTable.Bundles.ResolveBundleUrl(("~/bundles/myBundle")
//This code not only has the benefit of creating the script link for you but it also has the added 
//benefit of generating the hashtag for the script. This means the browser will store the script 
//longer and the client will have to download it fewer times. With the hashtag, browsers get the 
//new script only if the hashtag is different or if it hits the internal expiration date, which is generally one year.

//Enabling minification is simple; you can enable it in your configuration file by setting the 
//compilation elements debug attribute to false: <compilation debug = "false" />
//You can also do it in code by adding BundleTable.EnableOptimizations = true; at 
//the bottom of the RegisterBundles method in your App_Start/BundleConfig.cs file.

namespace ASPNETWebApp
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
        }
    }
}
