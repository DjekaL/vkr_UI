using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Timers;
using WpfApp1.Models;

namespace WpfApp1 {
    public class DevicePolling {
        Timer _timer;
        int _time;
        List<string> _hosts;
        MainWindowDataModel _model;
        Dictionary<string, DateTime> _AFKDevices = new Dictionary<string, DateTime>();

        public DevicePolling(MainWindowDataModel model, int time, List<string> hosts) {
            _model = model;
            _time = time;
            _hosts = hosts;
        }

        private void SetTimer() {
            _timer = new Timer(_time);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e) {
            _model.LastPolling = string.Format("Предыдущий опрос: \n{0}", DateTime.Now);
            var ab = DateTime.Now;
            _model.NextPolling = ab.AddMilliseconds(_time).ToString();
            _model.NextPolling = string.Format("Следующий опрос: \n{0}", DateTime.Now.AddMilliseconds(_time));
            PingOptions pingOptions = new PingOptions(128, true);
            Ping ping = new Ping();
            byte[] buffer = new byte[32];
            foreach (var host in _hosts) {
                try {
                    PingReply pingReply = ping.Send(host, 1000, buffer, pingOptions);
                    if (pingReply != null) {
                        switch (pingReply.Status) {
                            case IPStatus.Success:
                                if (_AFKDevices != null) {
                                    CheckAndRemoveAFKDevice(host);
                                }
                                break;
                            default:
                                AddAFKDevice(host);
                                break;
                        }
                    }
                }
                catch {
                    AddAFKDevice(host);
                }
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
                _AFKDevices[device.Key] = device.Value.AddMilliseconds(_time);
                //device.Value = DateTime.Now - device.Value;
            }
        }

        private void AddAFKDevice(string host) {
            if (_AFKDevices != null) {
                if (!_AFKDevices.ContainsKey(host)) {
                    _AFKDevices.Add(host, new DateTime(0));
                }
            } 
            else {
                _AFKDevices.Add(host, new DateTime(0));
            }
        }

        private void UpdateDeviceStatus() {
            _model.AFKDevicesNames = string.Empty;
            _model.AFKDevicesTimes = string.Empty;
            foreach (var device in _AFKDevices) {
                // получить с базы имя устройства по хосту
                _model.AFKDevicesNames += device.Key + "\n";
                _model.AFKDevicesTimes += $"{device.Value:T}" + "\n";
                _model.OfflineDevices = _AFKDevices.Count;
                _model.OnlineDevices = _hosts.Count - _AFKDevices.Count;
            }
        }
    }
}
