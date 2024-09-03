using _2408.MVC.Services;
using command_proj1;
using System;
using System.Web.ApplicationServices;
using System.Web.Mvc;


namespace _2408.MVC.Controllers
{
    public class SaleAPIController : Controller
    {
        SaleService saleService;

        public SaleAPIController()
        {
            saleService = ServiceModule.GetService<SaleService>();
        }

        [HttpPost]
        public ActionResult Create(CreateSaleCommandInput inp)
        {
            return Json(saleService.Create(inp));
        }

        [HttpGet]
        public ActionResult Get()
        {
            var comCode = Request.QueryString["COM_CODE"];
            var ioDate = Request.QueryString["IO_DATE"];
            var ioNo = Int32.Parse(Request.QueryString["IO_NO"]);

            if (comCode == null || ioDate == null) {
                throw new Exception("쿼리스트링에 COM_CODE와 IO_DATE 필요");
            }
            var inp = new GetSaleCommandInput();
            inp.Key.COM_CODE = comCode;
            inp.Key.IO_DATE = ioDate;
            inp.Key.IO_NO = ioNo;

            return Json(saleService.Get(inp), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Select()
        {
            var req = new SelectSaleDACRequestDTO();
            //req.COM_CODE = Request.QueryString["COM_CODE"];
            //req.PROD_CD = Request.QueryString["PROD_CD"];
            //req.PROD_NM = Request.QueryString["PROD_NM"];
            //Int32.TryParse(Request.QueryString["ord_PROD_NM"], out req.ord_PROD_NM);
            //Int32.TryParse(Request.QueryString["pageSize"], out req.pageSize);
            //Int32.TryParse(Request.QueryString["pageNo"], out req.pageNo);

            return Json(saleService.Select(req), JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult Put(UpdateSaleCommandInput inp)
        {
            return Json(saleService.Put(inp));
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            var comCode = Request.QueryString["COM_CODE"];
            var ioDate = Request.QueryString["IO_DATE"];
            int ioNo;
            var parsed = Int32.TryParse(Request.QueryString["IO_NO"], out ioNo);

            if (comCode == null || ioDate == null || !parsed) {
                throw new Exception("쿼리스트링에 COM_CODE, IO_DATE와 IO_NO 필요");
            }

            var inp = new DeleteSaleCommandInput();

            inp.Key.COM_CODE = comCode;
            inp.Key.IO_DATE = ioDate;
            inp.Key.IO_NO = ioNo;

            return Json(saleService.Delete(inp));
        }
    }
}