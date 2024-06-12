using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1 {
    public class Perfomance {
        public int ProcessorTime { get; set; }
        public int MemoryInUse { get; set; }
        public int DiskTime { get; set; }
        public int PacketsPerSec { get; set; }
        public int BsentPerSec { get; set; }
        public int BRecievePerSec { get; set; }
    }
}
