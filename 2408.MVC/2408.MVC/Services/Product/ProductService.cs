using command_proj1;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace _2408.MVC.Services
{
    public class ProductService : Service
    {
        public ProductDTO Create(CreateProductCommandInput inp)
        {
            ProductDTO prdDTO = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<CreateProductCommand, Product>(new CreateProductCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"품목 생성 및 조회 실패:{res.Errors[0].Message}");
                    }
                    prdDTO = new ProductDTO(res.Output);
                });

            pipeLine.Execute();
            return prdDTO;
        }

        public ProductDTO Get(GetProductCommandInput inp)
        {
            ProductDTO prdDTO = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<GetProductCommand, Product>(new GetProductCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"품목 조회 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output == null)
                    {
                        return;
                    }
                    prdDTO = new ProductDTO(res.Output);
                });

            pipeLine.Execute();
            return prdDTO;
        }

        public ProductSelectDTO Select(SelectProductDACRequestDTO inp)
        {
            ProductSelectDTO resp = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<SelectProductCommand, SelectProductDACResponseDTO>(new SelectProductCommand())
                .Mapping(cmd =>
                {
                    cmd.Input = inp;
                })
                .Executed(res =>
                {
                    if (res.HasError())
                    {
                        throw new Exception($"품목 쿼리 실패: {res.Errors[0].Message}");
                    }

                    resp = new ProductSelectDTO()
                    {
                        totalCount = res.Output.totalCount,
                        pageSize = res.Output.pageSize,
                        pageNo = res.Output.pageNo,
                        products = res.Output.list.Select(prd => new ProductDTO(prd)).ToList(),
                    };
                });

            pipeLine.Execute();
            return resp;
        }
        public ProductDTO Put(UpdateProductCommandInput inp)
        {
            ProductDTO prdDTO = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<UpdateProductCommand, Product>(new UpdateProductCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"품목 생성 및 조회 실패:{res.Errors[0].Message}");
                    }
                    if (res.Output == null) {
                        return;
                    }
                    prdDTO = new ProductDTO(res.Output);
                });

            pipeLine.Execute();
            return prdDTO;
        }

        public ProductDTO Delete(DeleteProductCommandInput inp)
        {
            ProductDTO prdDTO = null;

            var pipeLine = new PipeLine();
            pipeLine.Register<DeleteProductCommand, Product>(new DeleteProductCommand())
                .Mapping(cmd => {
                    cmd.Input = inp;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"품목 삭제 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output == null) {
                        return;
                    }
                    prdDTO = new ProductDTO(res.Output);
                });

            pipeLine.Execute();
            return prdDTO;
        }
    }
}