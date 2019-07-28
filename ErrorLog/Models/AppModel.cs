using ErrorLog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLog.Models
{
    public class AppModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Moniker { get; set; }

        public ICollection<AppLogModel> Logs { get; set; }
    }
}
