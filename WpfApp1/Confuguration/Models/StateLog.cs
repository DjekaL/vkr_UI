using System;

namespace WpfApp1.Confuguration.Models {
    public class StateLog {

        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
