using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class CommandResult<TOut> where TOut : new()
    {
        public TOut Output
        {
            get
            {
                if (Output == null)
                {
                    Output = new TOut();
                }
                return Output;
            }
            set
            {
                Output = value;
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
