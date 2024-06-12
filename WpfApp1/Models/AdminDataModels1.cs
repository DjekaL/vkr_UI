using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models {
    public class AdminDataModels1 : INotifyPropertyChanged {

        public ObservableCollection<Connecctions> CurrentDevicesConections { get; set; } = new ObservableCollection<Connecctions>();
        public ObservableCollection<SecDev> CurrentSecDev {  get; set; } = new ObservableCollection<SecDev>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
