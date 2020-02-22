using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorLog.Common
{
    public interface ILogger
    {
        void Log(Log log);
    }
}
