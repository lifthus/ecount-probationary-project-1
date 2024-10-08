﻿using _2408.MVC.Services;
using command_proj1;
using System;
using System.Web.ApplicationServices;
using System.Web.Mvc;


namespace _2408.MVC.Controllers
{
    public class SaleController : Controller
    {
        [HttpGet]
        [Route("sale")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("popup/sale/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("popup/sale/update")]
        public ActionResult Update()
        {
            return View();
        }
    }
}