using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class InexecutableCommandError : Error
    {
        public InexecutableCommandError(string message) : base("INEXECUTABLE", message) { }
    }
}
