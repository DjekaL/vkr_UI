using System.Collections.Generic;

namespace WpfApp1.Confuguration.Models {
    public class Device {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Ip { get; set; }
        public int DeviceTypeId { get; set; }
        public DeviceType DeviceType { get; set; }
        public ICollection<StateLog> StateLogs { get; set; }
        public ICollection<Connection> HostConnections { get; set; }
        public ICollection<Connection> ClientConnections { get; set; }
    }
}
