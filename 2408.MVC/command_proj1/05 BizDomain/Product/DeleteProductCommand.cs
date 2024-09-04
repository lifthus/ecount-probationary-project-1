using System;
using System.Collections.Generic;

namespace command_proj1
{
    public class DeleteProductCommandInput
    {
        private ProductKey _key;
        public ProductKey Key {
            get {
                if (_key == null) {
                    _key = new ProductKey();
                }
                return _key;
            }
            set {
                Key = value;
            }
        }
    }

    public class DeleteProductCommand : Command<Product>
    {
        public DeleteProductCommandInput Input { get; set; }

        private PipeLine _pipeLine = new PipeLine();

        protected override void Init()
        {
        }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("품목 삭제 커맨드 입력 없음");
            }
        }
        protected override void OnExecuting()
        {
        }
        protected override void ExecuteCore()
        {
            Product targetProduct = null;
            _pipeLine.Register<GetProductDAC, Product>(new GetProductDAC())
               .Mapping(cmd => {
                   cmd.Input = new GetProductDACRequestDTO(Input.Key.COM_CODE, Input.Key.PROD_CD);
               })
               .Executed(res => {
                   if (res.HasError()) {
                       throw new InexecutableCommandError($"품목 등록 여부 검증 실패: {res.Errors[0].Message}");
                   }
                   if (res.Output == null) {
                       throw new InexecutableCommandError("해당 품목 없음");
                   }
                   targetProduct = res.Output;
               });
            // 기존 SelectSale로 원하는 목적을 달성할 수 있기 때문에 우선 재사용
            _pipeLine.Register<SelectSaleDAC, SelectSaleDACResponseDTO>(new SelectSaleDAC())
                .AddFilter(cmd => {
                    if (targetProduct == null) {
                        return false;
                    }
                    return true;
                })
                .Mapping(cmd => {
                    var req = new SelectSaleDACRequestDTO();
                    req.COM_CODE = "80000";
                    req.PROD_CD_list = new string[] { targetProduct.Key.PROD_CD };
                    req.REMARKS = "";
                    req.IO_DATE_start = "1900/01/01";
                    req.IO_DATE_end = "9999/12/31";
                    req.pageSize = 1;
                    req.pageNo = 1;
                    cmd.Input = req;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw res.Errors[0];
                    }
                    if (0 < res.Output.totalCount) {
                        throw new InexecutableCommandError("해당 품목의 판매 이력이 존재");
                    }
                });
            _pipeLine.Register<DeleteProductDAC, int>(new DeleteProductDAC())
                .AddFilter(cmd => {
                    if (targetProduct == null) {
                        return false;
                    }
                    return true;
                })
                .Mapping(cmd => {
                    cmd.Input = targetProduct;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception(Errors[0].Message);
                    }
                    Output = targetProduct;
                });

            _pipeLine.Execute();
        }
        protected override void Executed()
        {
        }

    }
}
