using Node.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.SDK
{
    public class ShutdownEventArgs: EventArgs
    {
        public string Message { get; set; }
    }
}
