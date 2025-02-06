using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_managment_app.Models
{
    internal class FunctionSettings
    {
        public FunctionKey FunctionKeys { get; set; }
    }

    public class FunctionKey
    {
        public string TaskFunctionKey { get; set; }
    }
}
