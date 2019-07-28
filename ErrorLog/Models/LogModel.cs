﻿using ErrorLog.Data;
using ErrorLog.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLog.Models
{
    public class LogModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Level { get; set; }
        public string AppName { get; set; }
        public string AppMoniker { get; set; }
    }
}