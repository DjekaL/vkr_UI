using System.Collections.Generic;

namespace WpfApp1.Confuguration.Models {
    public class Connection {
        public int Id { get; set; }
        public int FirstDeviceId { get; set; }
        public int SecondDeviceId { get; set; }
        public double DataTransferingSpeed { get; set; }
        public bool IsVisible { get; set; }
        public Device FirstDevice { get; set; }
        public Device SecondDevice { get; set; }
        public ICollection<DataTransferLog> DataTransferLogs { get; set; }
    }
}
