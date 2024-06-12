using System;

namespace WpfApp1.Confuguration.Models {
    public class DataTransferLog {

        public int Id { get; set; }
        public DateTime? RecieveTime { get; set; }
        public DateTime SendingTime { get; set; }
        public int ConnectionId { get; set; }
        public double DataSize { get; set; }
        public Connection Connection { get; set; }
    }
}
