using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

          

            bundles.Add(new ScriptBundle("~/bundles/Kgrid").Include(
                       "~/Scripts/Kgrid.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                          //"~/Scripts/jquery.validate.js",
                          //"~/Scripts/jquery.validate.unobtrusive.min.js"
                          "~/Scripts/jquery.validate.js",
                         "~/Scripts/jquery.validate.unobtrusive.js",
                       "~/Scripts/jquery.unobtrusive-ajax.js"
                        //"~/Scripts/jquery.validate.unobtrusive.dynamic.js"

                        ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.6.2.js"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-2.2.4.js",
                       "~/Scripts/jszip.min.js", // used in export to excel
                       "~/Scripts/pako_deflate.min.js", // used to compress pdf
                      "~/Scripts/kendo.all.min.js",
                      "~/Scripts/bootbox.js",
                       "~/Scripts/app/grids.js",
                      "~/Scripts/app/forms.js",
                      "~/Scripts/app/formula.js",
                      "~/Scripts/toastr.js",
                      "~/Scripts/typeahead.bundle.min.js",
                      "~/Scripts/bloodhound.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            "~/Scripts/jquery-ui-1.11.4.min.js",
            "~/Scripts/bootstrap.js",
            "~/Scripts/bootstrap-select.js",
            "~/Scripts/jquery-migrate.min.js",
            "~/Scripts/jquery.slimscroll.min.js",
            "~/Scripts/metronic.js",
            "~/Scripts/app.min.js",
            "~/Scripts/demo.min.js",
            "~/Scripts/quick-sidebar.js",
            "~/Scripts/respond.min.js"
            ));



            bundles.Add(new StyleBundle("~/Content/cssmin").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/jquery.mCustomScrollbar.min.css",
                      //   "~/Content/components.css",
                      "~/Content/bootstrap-select.min.css",
                      "~/Content/uniform.default.css",
                      "~/Content/bootstrap-switch.min.css",
                      "~/Content/plugins.css",
                      "~/Content/custom.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/kendo.common.min.css",
                      "~/Content/kendo.default.min.css",
                      "~/Content/toastr.css",
                      "~/Content/typeahead.css",
                      "~/Content/Site.css",
                      "~/Content/forms.css"
                     //"~/Content/css/style.css"
                     ));



            bundles.Add(new StyleBundle("~/Content/jQuery-File-Upload").Include(
                  "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                  "~/Content/jQuery.FileUpload/css/jquery.fileupload-ui.css",
                  "~/Content/blueimp-gallery2/css/blueimp-gallery.css",
                  "~/Content/blueimp-gallery2/css/blueimp-gallery-video.css",
                  "~/Content/blueimp-gallery2/css/blueimp-gallery-indicator.css"
                  ));

            bundles.Add(new ScriptBundle("~/bundles/jQuery-File-Upload").Include(
                        //<!-- The Templates plugin is included to render the upload/download listings -->
                        "~/Scripts/jQuery.FileUpload/vendor/jquery.ui.widget.js",
                       "~/Scripts/jQuery.FileUpload/tmpl.min.js",
                        //<!-- The Load Image plugin is included for the preview images and image resizing functionality -->
                        "~/Scripts/jQuery.FileUpload/load-image.all.min.js",
                        //<!-- The Canvas to Blob plugin is included for image resizing functionality -->
                        "~/Scripts/jQuery.FileUpload/canvas-to-blob.min.js",
                        //"~/Scripts/file-upload/jquery.blueimp-gallery.min.js",
                        //<!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
                        "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                        //<!-- The basic File Upload plugin -->
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                        //<!-- The File Upload processing plugin -->
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload-process.js",
                        //<!-- The File Upload image preview & resize plugin -->
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload-image.js",
                        //<!-- The File Upload audio preview plugin -->
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload-audio.js",
                        //<!-- The File Upload video preview plugin -->
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload-video.js",
                        //<!-- The File Upload validation plugin -->
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload-validate.js",
                        //!-- The File Upload user interface plugin -->
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload-ui.js",
                        //Blueimp Gallery 2 
                        "~/Scripts/blueimp-gallery2/js/blueimp-gallery.js",
                        "~/Scripts/blueimp-gallery2/js/blueimp-gallery-video.js",
                        "~/Scripts/blueimp-gallery2/js/blueimp-gallery-indicator.js",
                        "~/Scripts/blueimp-gallery2/js/jquery.blueimp-gallery.js"

                       ));

            bundles.Add(new ScriptBundle("~/bundles/Blueimp-Gallerry2").Include(//Blueimp Gallery 2 
                                    "~/Scripts/blueimp-gallery2/js/blueimp-gallery.js",
                                    "~/Scripts/blueimp-gallery2/js/blueimp-gallery-video.js",
                                    "~/Scripts/blueimp-gallery2/js/blueimp-gallery-indicator.js",
                                    "~/Scripts/blueimp-gallery2/js/jquery.blueimp-gallery.js"));
        }
    }
}
