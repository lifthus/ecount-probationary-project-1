using _2408.MVC.Services;
using command_proj1;
using System;
using System.Web.Mvc;


namespace _2408.MVC.Controllers
{
    public class ProductAPIController : Controller
    {
        ProductService productService;

        public ProductAPIController()
        {
            productService = ServiceModule.GetService<ProductService>();
        }

        [HttpPost]
        public ActionResult Create(CreateProductCommandInput inp)
        {
            return Json(productService.Create(inp));
        }

        [HttpGet]
        public ActionResult Get()
        {
            var comCode = Request.QueryString["COM_CODE"];
            var prodCd = Request.QueryString["PROD_CD"];
            if (comCode == null || prodCd == null) {
                throw new Exception("쿼리스트링에 COM_CODE와 PROD_CD 필요");
            }
            var inp = new GetProductCommandInput();
            inp.Key.COM_CODE = comCode;
            inp.Key.PROD_CD = prodCd;

            return Json(productService.Get(inp), JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult Put(UpdateProductCommandInput inp)
        {
            return Json(productService.Put(inp));
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            var comCode = Request.QueryString["COM_CODE"];
            var prodCd = Request.QueryString["PROD_CD"];
            if (comCode == null || prodCd == null) {
                throw new Exception("쿼리스트링에 COM_CODE와 PROD_CD 필요");
            }
            var inp = new DeleteProductCommandInput();
            inp.Key.COM_CODE = comCode;
            inp.Key.PROD_CD = prodCd;

            return Json(productService.Delete(inp));
        }
    }
}