using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1 {
    public class Statistics {
        public DateTime time { get; set; }
        public int size { get; set; }
        public string sourceAddress { get; set; } = string.Empty;
        public string destinationAddress { get; set; } = string.Empty;
        public string? ipSender { get; set; }
    }
}
