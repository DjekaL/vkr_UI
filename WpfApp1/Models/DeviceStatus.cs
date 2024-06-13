using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models {
    public class DeviceStatus {
        public string Name { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string LastCheck { get; set; } = string.Empty;
        public string PacketAmount { get; set; } = string.Empty;
    }
}

