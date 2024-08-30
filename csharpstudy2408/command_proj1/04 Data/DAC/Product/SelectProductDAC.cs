﻿using command_proj1._04_Data.Db;
using Npgsql.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class SelectProductDACRequestDTO
    {
        /// <summary>
        /// EQUAL
        /// </summary>
        public string COM_CODE;

        /// <summary>
        /// LIKE
        /// </summary>
        public string PROD_CD;
        /// <summary>
        /// LIKE
        /// </summary>
        public string PROD_NM;

        /// <summary>
        /// -1 DESC / 0 NONE / 1 ASC
        /// </summary>
        public int ord_PROD_NM;

        public int pageSize;
        public int pageNo;
    }

    public class SelectProductDACResponseDTO
    {
        public List<Product> list;
        public int totalCount;
    }

    public class SelectProductDAC : Command<SelectProductDACResponseDTO>
    {
        public SelectProductDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("품목 조회 필터 정보 필요");
            }
            if (Input.COM_CODE == null) {
                throw new InexecutableCommandError("회사 코드 필요");
            }
        }
        protected override void OnExecuting()
        {
            if (Input.PROD_CD == null) {
                Input.PROD_CD = "";
            }
            if (Input.PROD_NM == null) {
                Input.PROD_NM = "";
            }
        }
        protected override void ExecuteCore()
        {
            var productQueryBuilder = new StringBuilder("SELECT *");

            var productCountBuilder = new StringBuilder("SELECT COUNT(*)");

            // 리터럴 문자열 연결은 컴파일 하면서 최적화된다.
            var filterSQL =
            "FROM flow.product_jhl" +
            "WHERE com_code = @com_code AND prod_cd LIKE @prod_cd AND prod_nm LIKE @prod_nm";
            productQueryBuilder.Append(filterSQL);
            productCountBuilder.Append(filterSQL);

            productQueryBuilder.AppendLine("ORDER BY");
            if (0 < Input.ord_PROD_NM) {
                productQueryBuilder.AppendLine("prod_nm, ");
            } else if (Input.ord_PROD_NM < 0) {
                productQueryBuilder.AppendLine("prod_nm DESC, ");
            }
            productQueryBuilder.AppendLine("write_dt");

            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", $"{Input.COM_CODE}" },
                {"@prod_cd",  $"%{Input.PROD_CD}%" },
                {"@prod_nm",  $"%{Input.PROD_NM}%" },

            };

            var dbManager = new DbManager();


            var totalCnt = dbManager.Scalar<int>(productCountBuilder.ToString(), parameters, (reader, data) => {

            });

            var prdList = dbManager.Query<Product>(productQueryBuilder.ToString(), parameters, (reader, data) => {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.PROD_CD = reader["prod_cd"].ToString();
                data.PRICE = (int)reader["price"];
                data.PROD_NM = reader["prod_nm"].ToString();
                data.WRITE_DT = (DateTime)reader["write_dt"];
            });

            Output.list = prdList;
            Output.totalCount = prdList.Count;
        }
        protected override void Executed() { }
    }
}