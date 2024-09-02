using System;
using System.Security.Cryptography;

namespace command_proj1
{
    public class UpdateSaleCommandInput
    {
        private SaleKey _key;
        public SaleKey Key {
            get {
                if (_key == null) {
                    _key = new SaleKey();
                }
                return _key;
            }
            set {
                _key = value;
            }
        }
        public string PROD_CD { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public decimal QTY { get; set; }
        public string REMARKS { get; set; }
    }

    public class UpdateSaleCommand : Command<Sale>
    {
        public UpdateSaleCommandInput Input { get; set; }

        private PipeLine _pipeLine = new PipeLine();

        protected override void Init()
        {
        }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("품목 생성 커맨드 입력 없음");
            }
        }
        protected override void OnExecuting()
        {
        }
        protected override void ExecuteCore()
        {
            Sale targetSale = null;
            Product newProd = null;
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
               .Mapping(cmd => {
                   cmd.Input = new GetSaleDACRequestDTO() {
                       COM_CODE = Input.Key.COM_CODE,
                       IO_DATE = Input.Key.IO_DATE,
                       IO_NO = Input.Key.IO_NO
                   };
               })
               .Executed(res => {
                   if (res.HasError()) {
                       throw new InexecutableCommandError($"품목 등록 여부 검증 실패: {res.Errors[0].Message}");
                   }
                   if (res.Output == null) {
                       throw new InexecutableCommandError("대상 판매 없음");
                   }
                   targetSale = res.Output;
               });
            _pipeLine.Register<GetProductDAC, Product>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(Input.Key.COM_CODE, Input.PROD_CD);
                })
                .AddFilter(_ => {
                    // 수정 대상 판매의 품목 코드가 수정된 경우에만 해당 품목을 조회한다.
                    if (targetSale.PROD_CD != Input.PROD_CD) {
                        return true;
                    }
                    return false;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new InexecutableCommandError($"판매 대상 품목 조회 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output == null) {
                        throw new InexecutableCommandError("판매 대상 품목 없음");
                    }
                    newProd = res.Output;
                });
            _pipeLine.Register<UpdateSaleDAC, int>(new UpdateSaleDAC())
                .Mapping(cmd => {
                    targetSale.UNIT_PRICE = Input.UNIT_PRICE;
                    targetSale.QTY = Input.QTY;
                    targetSale.REMARKS = Input.REMARKS;
                    if (targetSale.PROD_CD != Input.PROD_CD) {
                        targetSale.PROD_CD = newProd.Key.PROD_CD;
                        targetSale.PROD_NM = newProd.PROD_NM;
                    }
                    cmd.Input = targetSale;
                })
                .AddFilter(_ => {
                    if (targetSale == null) {
                        return false;
                    }
                    return true;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"판매 수정 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output != 1) {
                        throw new Exception("판매 수정 이상");
                    }
                });
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetSaleDACRequestDTO() {
                        COM_CODE = Input.Key.COM_CODE,
                        IO_DATE = Input.Key.IO_DATE,
                        IO_NO = Input.Key.IO_NO,
                    };
                })
                .AddFilter(_ => {
                    if (targetSale == null) {
                        return false;
                    }
                    return true;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"수정된 판매 조회 실패: {res.Errors[0].Message}");
                    }
                    Output = res.Output;
                });

            _pipeLine.Execute();
        }
        protected override void Executed()
        {
        }

    }
}
