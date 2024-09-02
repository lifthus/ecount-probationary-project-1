using command_proj1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public abstract class Command<TOut> : ICommand
    {
        private CommandResult<TOut> _result;
        public CommandResult<TOut> Result
        {
            get {
                if (_result == null)
                {
                    _result = new CommandResult<TOut>(default(TOut), Errors);
                }
                return _result; 
            }
            protected set { _result = value; }
        }

        public List<Error> Errors
        {
            get;
        } = new List<Error>();

        protected TOut Output
        {
            get {
                if (_result == null) {
                    _result = new CommandResult<TOut>(default(TOut), Errors);
                }
                return _result.Output;
            }
            set { _result = new CommandResult<TOut>(value, Errors); }
        }


        /* 커맨드 실행부 */
        public void Execute()
        {
            try
            {
                Init();
                CanExecute();
                OnExecuting();
                _result = new CommandResult<TOut>(default(TOut), Errors);
                ExecuteCore();
                Executed();
            } catch (Error err) {
                Console.WriteLine($"커맨드 수행 실패 {err.StackTrace}");
                Errors.Add(err);
            } catch (Exception ex) {
                Console.WriteLine($"커맨드 수행 실패 {ex.StackTrace}");
                Errors.Add(new UnknownError(ex.Message));
            }
        }

        /* 커맨드 실행에 필요한 요소들 초기화 */
        protected abstract void Init();
        /* 입력값이나 실행 가능 상태 검증 : 예외 던지기 */
        protected abstract void CanExecute();
        /* 입력값 등 전처리 */
        protected abstract void OnExecuting();
        /* 전처리 거친 입력값 활용해 비즈니스 로직 실행 */
        protected abstract void ExecuteCore();
        /* 비즈니스 로직 실행 후 응답 콘텐츠 가공 */
        protected abstract void Executed();
    }
}
