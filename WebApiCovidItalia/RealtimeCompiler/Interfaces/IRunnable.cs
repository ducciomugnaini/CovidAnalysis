using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealtimeCompiler.Interfaces
{
    public interface IRunnable
    {
        JObject Elaborate(JObject data);
    }
}
