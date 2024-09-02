using command_proj1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace _2408.MVC.Services
{
    public class SaleService : Service
    {
        public SaleDTO Create(CreateSaleCommandInput inp)
        {
            return null;
        }

        public SaleDTO Get(GetSaleCommandInput inp)
        {
            SaleDTO prdDTO = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<GetProductCommand, Product>(new GetProductCommand())
                .Mapping(cmd => { 
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"품목 조회 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output == null)
                    {
                        return;
                    } 
                });

            pipeLine.Execute();
            return prdDTO;
        }

        public SaleSelectDTO Select(SelectSaleDACRequestDTO inp)
        {
            SaleSelectDTO resp = null;

            return resp;
        }
        public SaleDTO Put(UpdateSaleCommandInput inp)
        {
            SaleDTO prdDTO = null;
            return prdDTO;
        }

        public SaleDTO Delete(DeleteSaleCommandInput inp)
        {
            SaleDTO prdDTO = null;
            return prdDTO;
        }
    }
}