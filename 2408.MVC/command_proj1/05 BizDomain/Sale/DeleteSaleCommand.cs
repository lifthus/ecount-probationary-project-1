﻿using System;

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
                throw new InexecutableCommandError("품목 삭제 커맨드 입력 없음");
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
               })
               .Executed(res => {
                   if (res.Output == null || res.Errors.Count > 0) {
                       throw new InexecutableCommandError($"품목 등록 여부 검증 실패");
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
