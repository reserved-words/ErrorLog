using System;
using System.ComponentModel.DataAnnotations;

namespace ErrorLog.Common
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        [StringLength(50)]
        public string AppName { get; set; }
    }
}
