using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorLog.Common
{
    public interface ILogAccess
    {
        IEnumerable<Log> GetLogs(string appName);
    }
}
