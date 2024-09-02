using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class SelectSaleDACRequestDTO
    {
        public string ComCode;
        public string[] ProdCds;
        public string RemarksLike;
        public string StartIODate;
        public string EndIODate;
        public int OrdProdCd; // -1, 0, 1 
        public int OrdQty; // -1, 0, 1
        public SelectSaleDACRequestDTO( )
        { 
        }
    }

    public class SelectSaleDACResponseDTO
    {
        public List<Sale> list;
        public int totalCount;
        public int pageSize;
        public int pageNo;
    }

    public class SelectSaleDAC : Command<SelectSaleDACResponseDTO>
    {
        public SelectSaleDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new Exception("판매 조회 조건 명세 필요");
            }
        }
        protected override void OnExecuting() { }
        protected override void ExecuteCore()
        {
            var sql = @"
                SELECT *
                FROM flow.sale_jhl
                WHERE 
                    com_code = @com_code
                    AND remarks LIKE @remarks_like
            ";
            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", Input.ComCode },
                {"@remarks_like", $"%{Input.RemarksLike}%" },
            };

            if (Input.ProdCds != null && Input.ProdCds.Length > 0) {
                sql += @"
                    AND prod_cd = ANY(@prod_cds)
                ";
                parameters.Add("@prod_cds", Input.ProdCds);
            }

            var sDate = Input.StartIODate;
            var eDate = Input.EndIODate;
            if (sDate != null && sDate.Length != 0) {
                sql += @"
                        AND @s_date <= io_date
                ";
                parameters.Add("@s_date", sDate);
            }
            if (eDate != null && eDate.Length != 0) {
                sql += @"
                        AND io_date <= @e_date
                ";
                parameters.Add("@e_date", eDate);
            }

            sql += @"
                ORDER BY io_date, io_no
            ";

            var dbManager = new DbManager();
            var saleList = dbManager.Query<Sale>(sql, parameters, (reader, data) =>
            {
                data.Key.COM_CODE = Input.ComCode;
                data.Key.IO_DATE = reader["io_date"].ToString();
                data.Key.IO_NO = (int)reader["io_no"];
                data.PROD_CD = reader["prod_cd"].ToString();
                data.QTY = (int)reader["qty"];
                data.REMARKS = reader["remarks"].ToString();
            });

            IOrderedEnumerable<Sale> tmpEnum = null;
            if (Input.OrdProdCd < 0) {
                tmpEnum = saleList.OrderByDescending(x => x.PROD_CD);
            } else if (Input.OrdProdCd >0) {
                tmpEnum = saleList.OrderBy(x => x.PROD_CD);
            }
            if (Input.OrdQty < 0) {
                if (tmpEnum == null) {
                    tmpEnum = saleList.OrderByDescending(x => x.QTY);
                } else {
                    tmpEnum = tmpEnum.ThenByDescending(x => x.QTY);
                }
            } else if (Input.OrdQty > 0) {
                if( tmpEnum == null) {
                    tmpEnum = saleList.OrderBy(x => x.QTY);
                } else {
                    tmpEnum = tmpEnum.ThenBy(x => x.QTY);
                }
            }
             
        }
        protected override void Executed() { }
    }
}