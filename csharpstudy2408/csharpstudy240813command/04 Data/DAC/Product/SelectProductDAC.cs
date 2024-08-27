using csharpstudy240813command._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class SelectProductDACRequestDTO
    {
        public string ComCode;
        public string ProdCd;
        public string ProdNm;
        public bool OrdProdNm;
        public SelectProductDACRequestDTO(string comCode, string prodCd, string prodNm, bool ordProdNm)
        {
            ComCode = comCode != null ? comCode : "";
            ProdCd = prodCd != null ? prodCd : "";
            ProdNm = prodNm != null ? prodNm : "";
            OrdProdNm = ordProdNm;
        }
    }

    public class SelectProductDAC : Command<List<Product>>
    {
        public SelectProductDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new Exception("품목 조회 필터 정보 필요");
            }
        }
        protected override void OnExecuting() { }
        protected override void ExecuteCore()
        {
            var sql = @"
                SELECT *
                FROM flow.product_jhl
                WHERE com_code LIKE @com_code AND prod_cd LIKE @prod_cd AND prod_nm LIKE @prod_nm
                ORDER BY write_dt
            ";
            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", $"%{Input.ComCode}%" },
                {"@prod_cd",  $"%{Input.ProdCd}%" },
                {"@prod_nm",  $"%{Input.ProdNm}%" }
            };

            var dbManager = new DbManager();
            var prdList = dbManager.Query<Product>(sql, parameters, (reader, data) =>
            {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.PROD_CD = reader["prod_cd"].ToString();
                data.PRICE = (int)reader["price"];
                data.PROD_NM = reader["prod_nm"].ToString();
                data.WRITE_DT = (DateTime)reader["write_dt"];
            });

            if (Input.OrdProdNm) {
                prdList.Sort((a, b) => {
                    return a.PROD_NM.CompareTo(b.PROD_NM);
                });
            }

            Output = prdList;
        }
        protected override void Executed() { }
    }
}
