using System.ComponentModel;

namespace WpfApp1.Models {
    public class Devices : INotifyPropertyChanged {

        private string _name = string.Empty;
        private string _ip = string.Empty;
        private string _type = string.Empty;
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Type {
            get {
                return _type;
            }
            set {
                _type = value;
                OnPropertyChanged("Type");
            }
        }
        public string IP {
                get {
                    return _ip;
                }
                set {
                    _ip = value;
                    OnPropertyChanged("IP");
                }
            }
        public bool IsChanged { get; set; } = false;
        public int Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            IsChanged = true;
        }
    }
}
