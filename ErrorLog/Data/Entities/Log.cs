using ErrorLog.Data.Enums;
using System;

namespace ErrorLog.Data
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public App App { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public Level Level { get; set; }
    }
}