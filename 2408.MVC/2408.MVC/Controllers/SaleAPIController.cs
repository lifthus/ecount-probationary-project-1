﻿using _2408.MVC.Services;
using command_proj1;
using System;
using System.Linq;
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
        [Route("api/sale")]
        public ActionResult Create(CreateSaleRequestDTO dto)
        {
            var inp = new CreateSaleCommandInput();
            inp.Key.COM_CODE = dto.Key.COM_CODE;
            inp.Key.IO_DATE = dto.Key.IO_DATE;
            inp.PROD_CD = dto.PROD_CD;
            inp.UNIT_PRICE = decimal.Parse(dto.UNIT_PRICE);
            inp.QTY = decimal.Parse(dto.QTY);
            inp.REMARKS = dto.REMARKS ?? "";

            return Json(saleService.Create(inp));
        }

        [HttpGet]
        [Route("api/sale")]
        public ActionResult Get()
        {
            var getType = Request.QueryString["type"];
            if (getType != null && getType.ToLower() == "get") {
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
            var req = new SelectSaleDACRequestDTO();
            req.COM_CODE = Request.QueryString["COM_CODE"];
            var prodCdList = Request.QueryString["PROD_CD_list"];
            req.PROD_CD_list = prodCdList == null || prodCdList.Length == 0 ? new string[] { } : prodCdList.Split(',');
            req.REMARKS = Request.QueryString["REMARKS"];
            req.IO_DATE_start = Request.QueryString["IO_DATE_start"];
            req.IO_DATE_end = Request.QueryString["IO_DATE_end"];
            int ioDateNoOrd;
            Int32.TryParse(Request.QueryString["IO_DATE_NO_ord"], out ioDateNoOrd);
            req.IO_DATE_NO_ord = ioDateNoOrd;
            int PRODcDoRD;
            Int32.TryParse(Request.QueryString["PROD_CD_ord"], out PRODcDoRD);
            req.PROD_CD_ord = PRODcDoRD;
            int pageSize;
            Int32.TryParse(Request.QueryString["pageSize"], out pageSize);
            req.pageSize = pageSize;
            int pageNo;
            Int32.TryParse(Request.QueryString["pageNo"], out pageNo);
            req.pageNo = pageNo;

            return Json(saleService.Select(req), JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [Route("api/sale")]
        public ActionResult Put(UpdateSaleCommandInput inp)
        {
            return Json(saleService.Put(inp));
        }

        [HttpDelete]
        [Route("api/sale")]
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

    public class CreateSaleRequestDTO
    {
        public CreateSaleKeyRequestDTO Key { get; set; }
        public string PROD_CD { get; set; }
        public string UNIT_PRICE { get; set; }
        public string QTY { get; set; }
        public string REMARKS { get; set; }
    }

    public class CreateSaleKeyRequestDTO
    {
        public string COM_CODE { get; set; }
        public string IO_DATE { get; set; }
    }

    public class UpdateSaleRequestDTO
    {
        public SaleKey Key { get; set; }
        public string PROD_NM { get; set; }
        public string PRICE { get; set; }
        public string UNIT_PRICE { get; set; }
        public string QTY { get; set; }
        public string REMARKS { get; set; }
    }
}