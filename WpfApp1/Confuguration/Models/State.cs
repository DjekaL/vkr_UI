using System.Collections.Generic;

namespace WpfApp1.Confuguration.Models {
    public class State {
        public string? Type { get; set; }
        public int Id { get; set; }
        public ICollection<StateLog> StateLogs { get; set; }
    }
}
