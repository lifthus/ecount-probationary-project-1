using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace _2408.MVC.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("product")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("popup/product/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("popup/product/update")]
        public ActionResult Update()
        {
            return View();
        }

        [HttpGet]
        [Route("popup/product/select")]
        public ActionResult Select()
        {
            return View();
        }
    }
}