using System;
using System.ComponentModel.DataAnnotations;

namespace ErrorLog.Common
{
    public class LogContent
    {
        public string Message { get; set; }
        [StringLength(50)]
        public string ClientId { get; set; }
    }
}
