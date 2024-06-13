using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Confuguration;
using WpfApp1.Confuguration.Models;
using WpfApp1.Models;
using ApplicationContext = WpfApp1.Confuguration.ApplicationContext;

namespace WpfApp1 {
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window {

        ApplicationContext _context = new ApplicationContext();
        SQLiteHeper _db;
        private ObservableCollection<Devices> NewDevices { get; set; }
        private ObservableCollection<Devices> CurrentDevices { get; set; }
        private ObservableCollection<Connecctions> CurrentDevicesConections { get; set; }


        List<string> Type;

        public AdminWindow() {
            InitializeComponent();
            _db = new SQLiteHeper(_context);
            NewDevices = new ObservableCollection<Devices>();
            //CurrentDevices = new ObservableCollection<Devices>();
            //CurrentDevices = new ObservableCollection<Devices>();
            DataContext = this;
        }

        private void DevicesSettings_Loaded(object sender, RoutedEventArgs e) {
            NewDeviceTable.ItemsSource = NewDevices;
            Type = _db.GetDevicesTypes();
            DeviceTypes.ItemsSource = Type;

            CurrentDevices = _db.GetDevices();
            CurrentDevicesTable.ItemsSource = CurrentDevices;
            CurrentDeviceType.ItemsSource = Type;
            foreach (var device in CurrentDevices) {
                device.IsChanged = false;
            }
        }

        private void AddDeviceType_Click(object sender, RoutedEventArgs e) {
            _db.AddNewDeviceType(DeviceTypeToAdd.Text);
            ComboBox_Reload(sender, e);
            DeviceTypeToAdd.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            //_db.DBLoad();
        }

        private void DeviceTypesToUpdate_Loaded(object sender, RoutedEventArgs e) {
            DeviceTypesToUpdate.ItemsSource = _db.GetDevicesTypes();
        }

        private void DeviceTypeToDelete_Loaded(object sender, RoutedEventArgs e) {
            DeviceTypeToDelete.ItemsSource = _db.GetDevicesTypes();
        }

        private void DeviceTypesToUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DeviceTypesToUpdate.SelectedItem != null) {
                NewDeviceTypeTitle.Text = DeviceTypesToUpdate.SelectedValue.ToString();
            } else {
                NewDeviceTypeTitle.Clear();
            }
        }

        private void UpdateDeviceType_Click(object sender, RoutedEventArgs e) {
            _db.UpdateDeviceType(NewDeviceTypeTitle.Text, DeviceTypesToUpdate.SelectedValue.ToString());
            ComboBox_Reload(sender, e);
        }

        private void DeleteDeviceType_Click(object sender, RoutedEventArgs e) {
            _db.DeleteDeviceType(DeviceTypeToDelete.SelectedItem.ToString());
            ComboBox_Reload(sender, e);

        }

        private void ComboBox_Reload(object sender, RoutedEventArgs e) {
            DeviceTypesToUpdate_Loaded(sender, e);
            DeviceTypeToDelete_Loaded(sender, e);
            DevicesSettings_Loaded(sender, e);
        }

        private void AddDevice_Click(object sender, RoutedEventArgs e) {
            foreach (var ab in NewDevices) {
                var device = new Device { Name = ab.Name, Ip = ab.IP, DeviceTypeId = _db.GetDeviceTypeByName(ab.Type) };
                _db.AddNewDevice(device);
            }
            NewDevices.Clear();
            DevicesSettings_Reloaded(sender, e);
        }

        private void DeviceToDelet_Loaded(object sender, RoutedEventArgs e) {
            DeviceToDelet.ItemsSource = _db.GetDevicesNames();
        }

        private void UpdateDevice_Click(object sender, RoutedEventArgs e) {
            foreach (var device in CurrentDevices) {
                if (device.IsChanged) {
                    _db.UpdateDevice(device);
                }
            }
            DevicesSettings_Reloaded(sender, e);
        }

        private void DevicesSettings_Reloaded(object sender, RoutedEventArgs e) {
            DeviceToDelet_Loaded(sender, e);
            CurrentDevices = _db.GetDevices();
            CurrentDevicesTable.ItemsSource = CurrentDevices;
            CurrentDeviceType.ItemsSource = Type;
            foreach (var device in CurrentDevices) {
                device.IsChanged = false;
            }
            DevicesConections_Loaded(sender, e);
        }

        private void DeleteDevice_Click(object sender, RoutedEventArgs e) {
            _db.DeleteDevice(DeviceToDelet.SelectedItem.ToString());
            DevicesSettings_Reloaded(sender, e);
        }

        List<string> FirstDevices = new List<string>();
        List<string> SecondDevices = new List<string>();
        private void DevicesConections_Loaded(object sender, RoutedEventArgs e) {
            //CurrentDevicesConections = new ObservableCollection<Connecctions>();
            CurrentDevicesConections = _db.GetConnections();
            DevicesConnections.ItemsSource = CurrentDevicesConections;
            FirstDevices = _db.GetDevicesNames();
            FirstDevice.ItemsSource = FirstDevices;
        }

        private void FirstDevice_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            /*var context = (sender as ComboBox)?.DataContext as Connecctions;
            if (context == null)
                return;
            SecondDevices = new List<string>(FirstDevices);
            var firstDev = (DevicesConnections.SelectedItem as Connecctions).FirstDevices;
            SecondDevices.Remove(firstDev);
            List<string> usedSecondDevices = _db.GetUsedSecDevices((sender as ComboBox).SelectedItem.ToString());
            context.SecDev.Clear();

            foreach (var con in CurrentDevicesConections) {
                if (con.FirstDevices == firstDev) {
                    if (con.SecDev.Count != 0) {
                        SecondDevices.Remove(con.SecDev[CurrentDevicesConections.IndexOf(con)].SecondDevices);
                    }
                }
            }

            foreach (var device in SecondDevices) {
                context.SecDev.Add(new Models.SecDev { SecondDevices = device });
            }*/
        }

        private void SecDevices_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var context = (sender as ComboBox)?.DataContext as Connecctions;
        }

        private void SaveDevicesConections_Click(object sender, RoutedEventArgs e) {
            ReflectConnections();
            _db.UpdateConnections(CurrentDevicesConections);
            AddRemoteConnections();
            DevicesConections_Loaded(sender, e);
        }

        private void ReflectConnections() {
            ObservableCollection<Connecctions> refCons = new ObservableCollection<Connecctions>();
            foreach (var con in CurrentDevicesConections) {
                refCons.Add(new Connecctions() { FirstDevices = con.Sec.SecondDevices, Sec = new SecDev { SecondDevices = con.FirstDevices }, Speed = con.Speed, IsVisible = false });
            }
            foreach (var con in refCons) {
                CurrentDevicesConections.Add(con);
            }
        }

        private void AddRemoteConnections() {
            LocalDijkstra dijkstra = new LocalDijkstra(CurrentDevicesConections);
            dijkstra.Initial();
            foreach (int i in dijkstra.Devices.Keys) {
                var newCons = dijkstra.MainAlg(i);
                _db.UpdateConnections(newCons);
            }
        }

        private void DeleteDeveiceConection_Click(object sender, RoutedEventArgs e) {
            var fd = (DevicesConnections.SelectedItem as Connecctions).FirstDevices;
            var sd = (DevicesConnections.SelectedItem as Connecctions).Sec.SecondDevices;
            _db.DeleteDevicesConnection(fd, sd);
            CurrentDevicesConections.Remove(DevicesConnections.SelectedItem as Connecctions);
        }

        private void SecDevices_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var context = (sender as ComboBox)?.DataContext as Connecctions;
            if (context == null)
                return;
            SecondDevices = new List<string>(FirstDevices);
            var firstDev = (DevicesConnections.SelectedItem as Connecctions).FirstDevices;
            SecondDevices.Remove(firstDev);
            List<string> usedSecondDevices = _db.GetUsedSecDevices((DevicesConnections.SelectedItem as Connecctions).FirstDevices);
            //context.SecDev = new ObservableCollection<SecDev>();
            foreach (var con in CurrentDevicesConections) {
                if (con.FirstDevices == firstDev) {
                    if (con.Sec != null && context.Sec == null) {
                        SecondDevices.Remove(con.Sec.SecondDevices);
                    }
                    if (con.Sec != null && context.Sec != null) {
                        if (context.Sec.SecondDevices != con.Sec.SecondDevices)
                            SecondDevices.Remove(con.Sec.SecondDevices);
                    }
                }
            }

            foreach (var device in SecondDevices) {
                if (context.SecDev.Count > SecondDevices.Count)
                    context.SecDev.Clear();
                if (context.SecDev.FirstOrDefault(x => x.SecondDevices == device) == null)
                    context.SecDev.Add(new Models.SecDev { SecondDevices = device });
            }
        }
    }

    public class LocalDijkstra {
        public Dictionary<int, string> Devices = new Dictionary<int, string>();
        public Dictionary<int, string> Fdevices = new Dictionary<int, string>();
        public Dictionary<int, string> Sdevices = new Dictionary<int, string>();
        public Dictionary<int, int> Connections = new Dictionary<int, int>();
        public Dictionary<int, double> ConSpeed = new Dictionary<int, double>();
        public List<List<int>> smeg = new List<List<int>>();
        public List<List<double>> cost = new List<List<double>>();
        public List<double> distance;
        public List<int> amount;
        public List<bool> IsVisited;
        private double size = 1000;

        ObservableCollection<Connecctions> Connecctions;

        public LocalDijkstra(ObservableCollection<Connecctions> con) {

            Connecctions = con;
            int j = 0;
            for (int i = 0; i < con.Count; i++) {
                Fdevices.Add(i, con[i].FirstDevices);
                Sdevices.Add(i, con[i].Sec.SecondDevices);
                ConSpeed.Add(i, Convert.ToInt32(con[i].Speed));

                if (!Devices.ContainsValue(con[i].FirstDevices)) {
                    Devices.Add(j, con[i].FirstDevices);
                    j++;
                }
                if (!Devices.ContainsValue(con[i].Sec.SecondDevices)) {
                    Devices.Add(j, con[i].Sec.SecondDevices);
                    j++;
                }
            }
            smeg = new List<List<int>>(Devices.Count);
            cost = new List<List<double>>(Devices.Count);
        }

        public void Initial() {
            foreach (int i in Devices.Keys) {
                smeg.Add(new List<int>());
                cost.Add(new List<double>());
                foreach (int j in Devices.Keys) {
                    smeg[i].Add(0);
                    cost[i].Add(0);
                }
            }
            SetValues();
        }

        private void SetValues() {
            /*foreach(int i in Devices.Keys) {
                foreach (int j in Devices.Keys) {
                    smeg[i][j] = 1;
                    cost[i][j] = ConSpeed[i];
                }
            }*/
            foreach (var con in Connecctions) {
                var i = Devices.FirstOrDefault(x => x.Value == con.FirstDevices).Key;
                var j = Devices.FirstOrDefault(x => x.Value == con.Sec.SecondDevices).Key;
                smeg[i][j] = 1;
                cost[i][j] = size / Convert.ToInt32(con.Speed);
            }
        }

        public ObservableCollection<Connecctions> MainAlg(int FirstDevId) {
            distance = new List<double>();
            amount = new List<int>();
            IsVisited = new List<bool>();

            foreach (int i in Devices.Keys) {
                distance.Add(double.MaxValue);
                amount.Add(0);
                IsVisited.Add(false);
            }

            distance[FirstDevId] = 0;

            for (int j = 0; j < Devices.Count - 1; j++) {
                int u = GetMinimuDistance();
                IsVisited[u] = true;

                foreach (int i in Devices.Keys) {
                    if (!IsVisited[i] && cost[u][i] != 0 && distance[u] != double.MaxValue && distance[u] + cost[u][i] < distance[i]) {
                        if (distance[u] == 0) {
                            distance[i] = distance[u] + cost[u][i];
                            amount[i]++;
                        } else {
                            distance[i] = distance[u] + cost[u][i];
                            amount[i] += amount[u] + 1;
                        }
                    }
                }
            }

            ObservableCollection<Connecctions> newCons = new ObservableCollection<Connecctions>();
            foreach (int i in Devices.Keys) {
                if (distance[i] != double.MaxValue && distance[i] != 0) {
                    newCons.Add(new Connecctions { FirstDevices = Devices[FirstDevId], Sec = new SecDev { SecondDevices = Devices[i] }, Speed = (amount[i] * size / distance[i]).ToString(), IsVisible = false });
                }
            }
            return newCons;
        }

        private int GetMinimuDistance() {
            double min = Double.MaxValue;
            int minIndex = -1;

            foreach (int i in Devices.Keys) {
                if (!IsVisited[i] && distance[i] <= min) {
                    min = distance[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }
    }
}
