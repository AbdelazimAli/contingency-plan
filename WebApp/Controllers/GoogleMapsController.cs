using Db.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class GoogleMapsController : BaseController
    {
        // GET: GoogleMaps
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult ShowMapPartial(int Height=700,string Lng="", string Lat="",string destLng="",string destLat = "",string txt_LatID="",string txt_LngID="")
        {
            
            ViewBag.Lng = Lng;
            ViewBag.Lat = Lat;
            ViewBag.Height = Height;
            ViewBag.destLng = destLng;
            ViewBag.destLat = destLat;
            ViewBag.Language = Language;
            ViewBag.txt_LatID = txt_LatID;
            ViewBag.txt_LngID = txt_LngID;
            return PartialView("_ShowMapPartial");

        }

        public ActionResult WebcameraTest()
        {
            return View();
        }

        public JsonResult Upload(byte[] image)
        {
            try
            {
                var test = HttpContext.Request.Files[0];
            }
            catch
            {

            }
            return Json(new { });
        }


    }
}