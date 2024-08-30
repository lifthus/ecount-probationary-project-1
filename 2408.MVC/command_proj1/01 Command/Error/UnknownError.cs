using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class UnknownError : Error
    {
        public UnknownError(string message) : base("UNKNOWN", message) { }
    }
}
