﻿using command_proj1;
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
            SaleSelectDTO dto = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<SelectSaleCommand, SelectSaleDACResponseDTO>(new SelectSaleCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"판매 조회 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output == null) {
                        return;
                    }
                    dto = new SaleSelectDTO();
                    dto.totalCount = res.Output.totalCount;
                    dto.pageSize = res.Output.pageSize;
                    dto.pageNo = res.Output.pageNo;
                    dto.sales = res.Output.list.Select(sale => new SaleDTO(sale)).ToList();
                });

            pipeLine.Execute();
            return dto;
        }
        public SaleDTO Put(UpdateSaleCommandInput inp)
        {
            SaleDTO saleDTO = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<UpdateSaleCommand, Sale>(new UpdateSaleCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"판매 수정 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output == null) {
                        return;
                    }
                    saleDTO = new SaleDTO(res.Output);
                });

            pipeLine.Execute();
            return saleDTO;
        }

        public SaleDTO Delete(DeleteSaleCommandInput inp)
        {
            SaleDTO saleDTO = null;
            var pipeLine = new PipeLine();
            pipeLine.Register<DeleteSaleCommand, Sale>(new DeleteSaleCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"판매 삭제 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output == null) {
                        return;
                    }
                    saleDTO = new SaleDTO(res.Output);
                });

            pipeLine.Execute();

            return saleDTO;
        }
    }
}