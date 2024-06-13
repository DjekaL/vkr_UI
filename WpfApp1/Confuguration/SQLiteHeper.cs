using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp1.Confuguration.Models;
using WpfApp1.Models;

namespace WpfApp1.Confuguration {
    public class SQLiteHeper {

        ApplicationContext _context;

        public SQLiteHeper(ApplicationContext context) {
            _context = context;
        }

        public void DBLoad() {
            _context.Database.EnsureCreated();
            _context.Connections.Load();
            _context.DataTransferLogs.Load();
            _context.Devices.Load();
            _context.DeviceTypes.Load();
            _context.StateLogs.Load();
            _context.States.Load();
        }

        public void AddNewDeviceType(string type) {
            var newType = new DeviceType { Name = type };
            _context.DeviceTypes.Add(newType);
            _context.SaveChanges();
        }

        public List<string> GetDevicesTypes() {
            return _context.DeviceTypes.Select(x => x.Name).ToList();
        }

        public void UpdateDeviceType(string type, string currentType) {
            var record = _context.DeviceTypes.SingleOrDefault(x => x.Name == currentType);
            if (record != null) {
                record.Name = type;
                _context.SaveChanges();
            }
        }

        public void DeleteDeviceType(string type) {
            var record = _context.DeviceTypes.SingleOrDefault(x => x.Name == type);
            if (record != null) {
                _context.DeviceTypes.Remove(record);
                _context.SaveChanges();
            }
        }

        public void AddNewDevice(Device device) {
            _context.Devices.Add(device);
            _context.SaveChanges();
        }

        public int GetDeviceTypeByName(string type) {
            return _context.DeviceTypes.FirstOrDefault(x => x.Name == type).Id;
        }

        public List<string> GetDevicesNames() {
            return _context.Devices.Select(x => x.Name).ToList();
        }

        public ObservableCollection<Devices> GetDevices() {
            var devices = _context.Devices;
            var currentDevices = new ObservableCollection<Devices>();
            foreach (var device in devices) {
                currentDevices.Add(new Devices { Id = device.Id, Name = device.Name, Type = GetDeviceTypeById(device.DeviceTypeId), IP = device.Ip });
            }
            return currentDevices;
        }

        public string GetDeviceTypeById(int id) {
            return _context.DeviceTypes.FirstOrDefault(x => x.Id == id).Name;
        }

        public void UpdateDevice(Devices device) {
            var record = _context.Devices.SingleOrDefault(x => x.Id == device.Id);
            if (record != null) {
                record.Name = device.Name;
                record.Ip = device.IP;
                record.DeviceTypeId = GetDeviceTypeByName(device.Type);
                _context.SaveChanges();
            }
        }

        public void DeleteDevice(string name) {
            var record = _context.Devices.SingleOrDefault(x => x.Name == name);
            if (record != null) {
                _context.Devices.Remove(record);
                _context.SaveChanges();
            }
        }

        public List<string> GetUsedSecDevices(string firstDev) {
            return _context.Connections.Select(x => x.FirstDevice).Where(x => x.Name == firstDev).Select(x => x.Name).ToList();
        }

        public void DeleteDevicesConnection(string firstDev, string secDev) {
            var fd = _context.Devices.FirstOrDefault(x => x.Name == firstDev);
            var sd = _context.Devices.FirstOrDefault(x => x.Name == secDev);
            if (fd != null && sd != null) {
                var record = _context.Connections.SingleOrDefault(x => x.FirstDeviceId == fd.Id && x.SecondDeviceId == sd.Id);
                if (record != null) {
                    _context.Connections.Remove(record);
                    _context.SaveChanges();
                }
            }
        }

        public void GetStatistic(string period, string devSender, string devReciever, List<int> time, List<int> size) {
            var ds = _context.Devices.FirstOrDefault(x => x.Name == devSender);
            var dr = _context.Devices.FirstOrDefault(x => x.Name == devReciever);
            var con = _context.Connections.FirstOrDefault(x => x.FirstDeviceId == ds.Id && x.SecondDeviceId == dr.Id);
            if (con != null) {
                var lastDay = _context.DataTransferLogs.AsNoTracking().Where(x => x.ConnectionId == con.Id).OrderBy(x => x.SendingTime).LastOrDefault().SendingTime;
                var ab = lastDay.ToString("dd.MM.yyyy HH:mm:ss");
                lastDay = DateTime.Parse(ab);
                DateTime firstDay = DateTime.Now;
                switch (period) {
                    case "День":
                        firstDay = lastDay;
                        break;
                    case "Неделя":
                        firstDay = lastDay.AddDays(-6);
                        break;
                    case "Месяц":
                        firstDay = lastDay.AddDays(-lastDay.Day + 1);
                        break;
                }
                lastDay = new DateTime(lastDay.Year, lastDay.Month, lastDay.Day, 23, 59, 59);
                firstDay = new DateTime(firstDay.Year, firstDay.Month, firstDay.Day, 0, 0, 1);

                var startOfDay = firstDay;
                List<List<DataTransferLog>> Logs = new List<List<DataTransferLog>>();
                int daysAmount = (lastDay - firstDay).Duration().Days + 1;
                for (int i = 0; i < daysAmount; i++) {
                    var endOfDay = startOfDay.AddHours(23).AddMinutes(59).AddSeconds(58);

                    var startOfDayStr = startOfDay.ToString("yyyy-MM-dd HH:mm:ss");
                    var endOfDayStr = endOfDay.ToString("yyyy-MM-dd HH:mm:ss");

                    var res = _context.DataTransferLogs.FromSqlInterpolated($"SELECT * from DataTransferLogs where strftime('%Y-%m-%d %H:%M:%S', SendingTime) <= {endOfDayStr} and strftime('%Y-%m-%d %H:%M:%S', SendingTime) >= {startOfDayStr} and ConnectionId == {con.Id} and RecieveTime != '' and DataSize > 0").ToList();
                    Logs.Add(res);
                    startOfDay = startOfDay.AddDays(1);
                }
                switch (period) {
                    case "День":
                        foreach (var log in Logs) {
                            foreach (var item in log) {
                                TimeSpan interval = new TimeSpan();
                                int averageSize = 0;
                                if (log.Count > 0) {
                                    size.Add(Convert.ToInt32(item.DataSize));
                                    interval = (TimeSpan)(item.RecieveTime - item.SendingTime);
                                    time.Add((int)interval.TotalSeconds);
                                } else {
                                    size.Add(averageSize);
                                    time.Add((int)interval.TotalSeconds);
                                }
                            }
                        }
                        break;
                    case "Неделя":
                    case "Месяц":
                        foreach (var log in Logs) {
                            int averageSize = 0;
                            TimeSpan interval = new TimeSpan();
                            foreach (var item in log) {
                                averageSize += Convert.ToInt32(item.DataSize);
                                interval += (TimeSpan)(item.RecieveTime - item.SendingTime);
                            }
                            if (log.Count > 0) {
                                size.Add(averageSize / log.Count);
                                time.Add(Convert.ToInt32(interval.TotalSeconds) / log.Count);
                            } else {
                                size.Add(averageSize);
                                time.Add((int)interval.TotalSeconds);
                            }
                        }
                        break;
                }
            }
        }

        public ObservableCollection<Connecctions> GetConnections() {
            var connections = _context.Connections.Where(x => x.IsVisible == true).OrderBy(x => x.FirstDeviceId);
            var currentConnectionss = new ObservableCollection<Connecctions>();
            foreach (var con in connections) {
                currentConnectionss.Add(new Connecctions { FirstDevices = con.FirstDevice.Name, SecDev = new ObservableCollection<SecDev> { new SecDev { SecondDevices = con.SecondDevice.Name } }, Sec = new SecDev { SecondDevices = con.SecondDevice.Name }, Speed = con.DataTransferingSpeed.ToString() });
            }
            return currentConnectionss;
        }

        public void UpdateConnections(ObservableCollection<Connecctions> connections) {
            /*_context.Database.ExecuteSqlRaw("TRUNCATE TABLE Connections");
            foreach (var con in connections) {
                var FDId = _context.Devices.AsNoTracking().FirstOrDefault(d => d.Name == con.FirstDevices).Id;
                var SDId = _context.Devices.AsNoTracking().FirstOrDefault(d => d.Name == con.Sec.SecondDevices).Id;
                _context.Connections.Add(new Models.Connection { FirstDeviceId = FDId, SecondDeviceId = SDId, DataTransferingSpeed = double.Parse(con.Speed)});
                _context.SaveChanges();
            }*/
            var currentConnections = _context.Connections;
            List<Models.Connection> newConnections = new List<Models.Connection>();
            foreach (var con in connections) {
                bool isExists = false;
                foreach (var curCon in currentConnections) {
                    if (curCon.FirstDevice.Name == con.FirstDevices && curCon.SecondDevice.Name == con.Sec.SecondDevices && curCon.DataTransferingSpeed.ToString() != con.SecDevice) {
                        curCon.DataTransferingSpeed = double.Parse(con.Speed);
                        isExists = true;
                        break;
                    }
                    if ((curCon.FirstDevice.Name != con.FirstDevices && curCon.SecondDevice.Name != con.Sec.SecondDevices) ||
                        (curCon.FirstDevice.Name == con.FirstDevices && curCon.SecondDevice.Name != con.Sec.SecondDevices) ||
                        (curCon.FirstDevice.Name != con.FirstDevices && curCon.SecondDevice.Name == con.Sec.SecondDevices)) {
                        isExists = false;
                    } else {
                        isExists = true;
                    }
                }
                if (!isExists) {
                    var FDId = _context.Devices.AsNoTracking().FirstOrDefault(d => d.Name == con.FirstDevices).Id;
                    var SDId = _context.Devices.AsNoTracking().FirstOrDefault(d => d.Name == con.Sec.SecondDevices).Id;
                    currentConnections.Add(new Models.Connection { FirstDeviceId = FDId, SecondDeviceId = SDId, DataTransferingSpeed = double.Parse(con.Speed), IsVisible = con.IsVisible });
                }
            }
            _context.SaveChanges();
        }

        public ObservableCollection<DeviceStatus> GetDeviceStats() {
            var stats = new ObservableCollection<DeviceStatus>();
            var devices = _context.Devices.AsNoTracking();
            foreach (var device in devices) {
                if (device != null) {
                    var status = _context.StateLogs.AsNoTracking().OrderBy(x => x.Id).LastOrDefault(x => x.DeviceId == device.Id);
                    var curDay = DateTime.Now;
                    var lastDay = new DateTime(curDay.Year, curDay.Month, curDay.Day, 23, 59, 59);
                    var firstDay = new DateTime(curDay.Year, curDay.Month, curDay.Day, 0, 0, 1);
                    var startOfDayStr = firstDay.ToString("yyyy-MM-dd HH:mm:ss");
                    var endOfDayStr = lastDay.ToString("yyyy-MM-dd HH:mm:ss");
                    var connections = _context.Connections.AsNoTracking().Select(x => x.FirstDeviceId == device.Id);
                    int amount = 0;
                    foreach (var connection in connections) {
                        if (connection != null) {
                            var res = _context.DataTransferLogs.FromSqlInterpolated($"SELECT * from DataTransferLogs where strftime('%Y-%m-%d %H:%M:%S', SendingTime) <= {endOfDayStr} and strftime('%Y-%m-%d %H:%M:%S', SendingTime) >= {startOfDayStr} and ConnectionId == {connection} and RecieveTime != '' and DataSize > 0").ToList();
                            amount += res.Count;
                        }
                    }
                    //if (status != null) {
                        stats.Add(new DeviceStatus { Name = device.Name, Ip = device.Ip, Status = status != null ? status.State.Type : "", LastCheck = status != null ? status.Time.ToString() : "", PacketAmount = amount.ToString() });
                    //}
                }
            }
            return stats;
        }
    }
}
