using command_proj1;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    if (res.Output == null || res.HasError()) {
                        throw new Exception($"품목 조회 실패");
                    }
                    prdDTO = new ProductDTO(res.Output);
                });

            pipeLine.Execute();
            return prdDTO;
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
                        throw new Exception($"품목 삭제 실패");
                    }
                    prdDTO = new ProductDTO(res.Output);
                });

            pipeLine.Execute();
            return prdDTO;
        }
    }
}