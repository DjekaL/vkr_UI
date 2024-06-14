using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Timers;
using WpfApp1.Confuguration;
using WpfApp1.Confuguration.Models;
using WpfApp1.Models;

namespace WpfApp1 {
    public class DevicePolling {
        Timer _timer;
        List<string> _hosts;
        MainWindowDataModel _model;
        Dictionary<string, DateTime> _AFKDevices = new Dictionary<string, DateTime>();
        Dictionary<string, string> _devices = new Dictionary<string, string>();
        Dictionary<string, int> _devicesId = new Dictionary<string, int>();
        SQLiteHeper _db;

        public DevicePolling(MainWindowDataModel model, List<string> hosts, SQLiteHeper db) {
            _model = model;
            _hosts = hosts;
            _db = db;
            foreach(string host in hosts) {
                _devices.Add(host, _db.GetDeviceNameByHost(host));
            }
            foreach(string name in _devices.Values) {
                _devicesId.Add(name, _db.GetDeviceIdByName(name));
            }
        }

        private void SetTimer() {
            _timer = new Timer(_model.Period);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private async void OnTimedEvent(Object source, ElapsedEventArgs e) {
            _model.LastPolling = string.Format("Предыдущий опрос: \n{0}", DateTime.Now);
            var ab = DateTime.Now;
            _model.NextPolling = ab.AddMilliseconds(_model.Period).ToString();
            _model.NextPolling = string.Format("Следующий опрос: \n{0}", DateTime.Now.AddMilliseconds(_model.Period));
            PingOptions pingOptions = new PingOptions(128, true);
            Ping ping = new Ping();
            byte[] buffer = new byte[32];
            foreach (var host in _hosts) {
                var state = new StateLog();
                state.Time = DateTime.Now;
                state.DeviceId = _devicesId[_devices[host]];
                try {
                    PingReply pingReply = ping.Send(host, 1000, buffer, pingOptions);
                    if (pingReply != null) {
                        switch (pingReply.Status) {
                            case IPStatus.Success:
                                if (_AFKDevices != null) {
                                    CheckAndRemoveAFKDevice(host);
                                    state.StateId = 0;
                                }
                                break;
                            default:
                                AddAFKDevice(host);
                                state.StateId = 1;
                                break;
                        }
                    }
                }
                catch {
                    AddAFKDevice(host);
                    state.StateId = 1;
                }
                //await _db.AddDeviceState(state);
            }
            if (_AFKDevices != null) {
                UpdateDevicesAFKTime();
                UpdateDeviceStatus();
            }
        }

        public async void StartDevicePolling() {
            SetTimer();
        }

        private void CheckAndRemoveAFKDevice(string host) {
            if (_AFKDevices.ContainsKey(host)) {
                _AFKDevices.Remove(host);
            }
        }

        private void UpdateDevicesAFKTime() {
            foreach (var device in _AFKDevices) {
                //_AFKDevices[device.Key] += DateTime.Now - device.Value - device.Value;
                _AFKDevices[device.Key] = device.Value.AddMilliseconds(_model.Period);
                //device.Value = DateTime.Now - device.Value;
            }
        }

        private void AddAFKDevice(string host) {
            if (_AFKDevices != null) {
                if (!_AFKDevices.ContainsKey(host)) {
                    _AFKDevices.Add(host, new DateTime(0));
                }
            } else {
                _AFKDevices.Add(host, new DateTime(0));
            }
        }

        private void UpdateDeviceStatus() {
            _model.AFKDevicesNames = string.Empty;
            _model.AFKDevicesTimes = string.Empty;
            foreach (var device in _AFKDevices) {
                // получить с базы имя устройства по хосту
                _model.AFKDevicesNames += _devices[device.Key] + "\n";
                _model.AFKDevicesTimes += $"{device.Value:T}" + "\n";
                _model.OfflineDevices = _AFKDevices.Count.ToString();
                _model.OnlineDevices = (_hosts.Count - _AFKDevices.Count).ToString();
            }
        }
    }
}
