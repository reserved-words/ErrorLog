using ErrorLog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLog.Models
{
    public class AppModel
    {
        public string Name { get; set; }
        public string Moniker { get; set; }

        public ICollection<AppLogModel> Logs { get; set; }
    }
}
