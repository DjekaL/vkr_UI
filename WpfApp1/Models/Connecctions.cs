using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfApp1.Models {
    public class Connecctions : INotifyPropertyChanged {

        private string _firstDevices;
        private SecDev _sec;
        public SecDev Sec {
            get {
                return _sec;
            }
            set {
                _sec = value;
                OnPropertyChanged("Sec");
            }
        }

        private  ObservableCollection<SecDev> _secDev = new ObservableCollection<SecDev>();
        public ObservableCollection<SecDev> SecDev {
            get {
                return _secDev;
            }
            set {
                _secDev = value;
                OnPropertyChanged("SecDev");
            }
        }

        private string _speed;

        public string FirstDevices { 
            get {
                return _firstDevices; 
            } 
            set { 
                _firstDevices = value;
                OnPropertyChanged("FirstDevices");
            }
        }
        private string _secDevice;
        public string SecDevice { 
            get {
                return _secDevice; 
            } 
            set {
                _secDevice = value;
                OnPropertyChanged("SecDevice");
            }
        }
        public string Speed {
            get {
                return _speed;
            }
            set {
                _speed = value;
                OnPropertyChanged("Speed");
            }
        }

        public bool IsVisible { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            IsVisible = true;
        }

    }
    public class SecDev : INotifyPropertyChanged {

        private string _secondDevices;
        public string SecondDevices {
            get {
                return _secondDevices;
            }
            set {
                _secondDevices = value;
                OnPropertyChanged("SecondDevices");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
