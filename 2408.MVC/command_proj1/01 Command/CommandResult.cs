using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class CommandResult<TOut> where TOut : new()
    {
        private TOut _output;
        public TOut Output
        {
            get
            {
                if (_output == null)
                {
                    _output = new TOut();
                }
                return _output;
            }
            set
            {
                if (_output == null)
                {
                    _output = new TOut();
                }
                _output = value;
            }
        }
        public readonly List<Error> Errors;

        public CommandResult(TOut output, List<Error> errors) 
        {
            this.Output = output;
            this.Errors = errors;
        }
        
        public bool HasError ()
        {
            return Errors.Count > 0;
        }
    }
}
