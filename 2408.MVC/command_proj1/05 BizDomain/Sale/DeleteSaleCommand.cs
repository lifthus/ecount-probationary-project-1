using System;

namespace command_proj1
{
    public class DeleteSaleCommandInput
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
                Key = value;
            }
        }
    }

    public class DeleteSaleCommand : Command<Sale>
    {
        public DeleteSaleCommandInput Input { get; set; }

        private PipeLine _pipeLine = new PipeLine();

        protected override void Init()
        {
        }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("판매 삭제 커맨드 입력 없음");
            }
            if (Input.Key.COM_CODE == null || Input.Key.IO_DATE == null ) {
                throw new InexecutableCommandError("삭제 대상 판매 지정 요망");
            }
        }
        protected override void OnExecuting()
        {
        }
        protected override void ExecuteCore()
        {
            Sale targetSale = null;
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
               .Mapping(cmd => {
                   cmd.Input = new GetSaleDACRequestDTO() {
                       COM_CODE = Input.Key.COM_CODE,
                       IO_DATE = Input.Key.IO_DATE,
                       IO_NO = Input.Key.IO_NO,
                   };
               })
               .Executed(res => {
                   if (res.HasError()) {
                       throw new InexecutableCommandError($"판매 등록 여부 검증 실패: {res.Errors[0].Message}");
                   }
                   if (res.Output == null) {
                       throw new InexecutableCommandError("대상 판매 없음");
                   }
                   targetSale = res.Output;
               });
            _pipeLine.Register<DeleteSaleDAC, int>(new DeleteSaleDAC())
                .AddFilter(cmd => {
                    if (targetSale == null) {
                        return false;
                    }
                    return true;
                })
                .Mapping(cmd => {
                    cmd.Input = new DeleteSaleDACRequestDTO(Input.Key.COM_CODE, Input.Key.IO_DATE, Input.Key.IO_NO);
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception(Errors[0].Message);
                    }
                    Output = targetSale;
                });

            _pipeLine.Execute();
        }
        protected override void Executed()
        {
        }

    }
}
