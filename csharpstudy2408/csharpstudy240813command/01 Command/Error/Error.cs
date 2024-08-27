using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class Error : Exception
    {
        public readonly string Code;
        public Error (string code, string message) : base (message)
        {
            Code = code;
        }
    }
}
