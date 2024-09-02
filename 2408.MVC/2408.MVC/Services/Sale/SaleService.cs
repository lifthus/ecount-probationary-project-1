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
            SaleDTO saleDTO = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<CreateSaleCommand, Sale>(new CreateSaleCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"판매 생성 실패: {res.Errors[0].Message}");
                    }
                    saleDTO = new SaleDTO(res.Output);
                });

            pipeLine.Execute();

            return saleDTO;
        }

        public SaleDTO Get(GetSaleCommandInput inp)
        {
            SaleDTO saleDTO = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<GetSaleCommand, Sale>(new GetSaleCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"판매 조회 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output == null)
                    {
                        return;
                    }
                    saleDTO = new SaleDTO(res.Output);
                });

            pipeLine.Execute();
            return saleDTO;
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