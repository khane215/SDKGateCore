using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateSDKs.Models
{
    public class GateConfigModel
    {
        public string merchantid { get; set; } = "";
        public string secret { get; set; } = "";
        public string password { get; set; } = "";
        public string pathKeyFile { get; set; } = "";
    }
}
