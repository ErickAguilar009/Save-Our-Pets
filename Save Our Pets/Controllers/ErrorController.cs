using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Save_Our_Pets.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string Url)
        {
            ViewBag.Title = "Error";

            if(!String.IsNullOrEmpty(Url))
            {
                string path = Url;
                ViewBag.url = path;
            }

            return View();
        }
    }
}