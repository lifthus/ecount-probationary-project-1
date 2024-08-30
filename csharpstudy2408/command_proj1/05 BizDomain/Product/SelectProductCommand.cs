using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class SelectProductCommandInput : SelectProductDACRequestDTO
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
        /// -1 DESC
        /// 0 NONE
        /// 1 ASC
        /// </summary>
        public int ord_PROD_NM; // -1 DESC 1 ASC
    }

    public class SelectProductCommand
    {
    }
}
