using System;

namespace command_proj1
{
    public class CreateSaleCommandInput
    {
        public CreateSaleKeyDTO Key { get; set; }
        public string PROD_CD { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public decimal QTY { get; set; }
        public string REMARKS { get; set; }
    }

    public class CreateSaleKeyDTO
    {
        public string COM_CODE { get; set; }
        public string IO_DATE { get; set; }
    }

    public class CreateSaleCommand : Command<Sale>
    {
        public CreateSaleCommandInput Input { get; set; }

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
            Product targetProd = null;
            int newIONo = 0;
            _pipeLine.Register<GetProductDAC, Product>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(Input.Key.COM_CODE, Input.PROD_CD);
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new InexecutableCommandError("대상 품목 조회 실패");
                    }
                    targetProd = res.Output;
                    if (targetProd == null) {
                        throw new InexecutableCommandError("대상 품목 없음");
                    }
                    if (!targetProd.ACTIVE) {
                        throw new InexecutableCommandError("대상 품목 사용중단");
                    }
                });
            _pipeLine.Register<PickSaleNumberingDAC, SaleNumbering>(new PickSaleNumberingDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetSaleNumberingDACRequestDTO() {
                        COM_CODE = Input.Key.COM_CODE,
                        IO_DATE = Input.Key.IO_DATE
                    };
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new InexecutableCommandError($"채번 실패: {res.Errors[0].Message}");
                    }
                    newIONo = res.Output.IO_NO;
                    if (newIONo < 1) {
                        throw new InexecutableCommandError($"채번 실패: {res.Errors[0].Message}");
                    }
                });
            _pipeLine.Register<InsertSaleDAC, int>(new InsertSaleDAC())
                .Mapping(cmd => {
                    var newSale = new Sale();
                    newSale.Key.COM_CODE = Input.Key.COM_CODE;
                    newSale.Key.IO_DATE = Input.Key.IO_DATE;
                    newSale.Key.IO_NO = newIONo;
                    newSale.PROD_CD = Input.PROD_CD;
                    newSale.PROD_NM = targetProd.PROD_NM; // 조회된 품목의 현재 이름 넣어주기
                    newSale.UNIT_PRICE = Input.UNIT_PRICE;
                    newSale.QTY = Input.QTY;
                    newSale.REMARKS = Input.REMARKS;
                    cmd.Input = newSale;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception($"판매 생성 실패: {res.Errors[0].Message}");
                    }
                    if (res.Output != 1) {
                        throw new Exception("판매 생성 이상");
                    }
                });
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
               .Mapping(cmd => {
                   cmd.Input = new GetSaleDACRequestDTO() {
                       COM_CODE = Input.Key.COM_CODE,
                       IO_DATE = Input.Key.IO_DATE,
                       IO_NO = newIONo
                   };
               })
               .Executed(res => {
                   if (res.HasError()) {
                       throw new Exception($"판매 생성 실패: {res.Errors[0].Message}");
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
