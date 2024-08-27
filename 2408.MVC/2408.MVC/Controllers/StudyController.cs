using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2408.MVC.Controllers
{
    public class StudyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        { // Test HTML 띄우는 메소드 
            return View();
        }

        [HttpPost]
        public ActionResult Select(Person person)
        {
            var nameIsJeonghun = person.Name == "정훈";

            var result = new { isSuccess = nameIsJeonghun };
            // 원래 JSONResult 해야하지만 번거로워서 프레임워크에서 그냥 Json 메소드로 만들어 놓은 것.
            return Json(result);
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}