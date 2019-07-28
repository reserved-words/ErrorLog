
using System;
using System.Collections.Generic;

namespace ErrorLog.Data
{
    public class App
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Log> Logs { get; set; }
    }
}