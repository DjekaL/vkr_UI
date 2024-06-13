using HarfBuzzSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp1.Confuguration;

namespace WpfApp1.Models {
    public class MainWindowDataModel : INotifyPropertyChanged {

        public MainWindowDataModel()
        {
            
        }

        private string _log = string.Empty;
        public bool isGetPerfomance = false;
        public string Log {
            get { return _log; }
            set {
                _log = value;
                OnPropertyChanged("Log");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Perfomance Perfomances {
            get { return _perfomances; }
            set {
                _perfomances = value;
                AddChartsValue();
            }
        }
        PerfomanceChart chart = new PerfomanceChart();
        private void AddChartsValue() {
            List<int> proc = new List<int> { _perfomances.ProcessorTime };
            chart.GetNewChcartVaalue(ProccesorSeriece, proc);
            List<int> mem = new List<int> { _perfomances.MemoryInUse };
            chart.GetNewChcartVaalue(MemorySeriece, mem);
            List<int> disk = new List<int> { _perfomances.DiskTime };
            chart.GetNewChcartVaalue(DiskSeriece, disk);
            List<int> net = new List<int> { _perfomances.PacketsPerSec, _perfomances.BsentPerSec, _perfomances.BRecievePerSec };
            chart.GetNewChcartVaalue(NetworkSeriece, net);
        }

        private Perfomance _perfomances = new Perfomance();
        public ISeries[] ProccesorSeriece { get; set; } = new ISeries[] { new LineSeries<int> { Values = new ObservableCollection<int>(), LineSmoothness = 0, GeometrySize = 0, Name = "Загрузка прочессора, % " } };
        public ISeries[] MemorySeriece { get; set; } = new ISeries[] { new LineSeries<int> { Values = new ObservableCollection<int>(), LineSmoothness = 0, GeometrySize = 0, Name = "Использование памяти, %" } };
        public ISeries[] DiskSeriece { get; set; } = new ISeries[] { new LineSeries<int> { Values = new ObservableCollection<int>(), LineSmoothness = 0, GeometrySize = 0, Name = "Активность диска, %" } };
        public ISeries[] NetworkSeriece { get; set; } = new ISeries[] { new LineSeries<int> {Values = new ObservableCollection<int>(), LineSmoothness = 0, GeometrySize = 0, Name = "Пакетов/с"},
                                                                        new LineSeries<int> {Values = new ObservableCollection<int>(), LineSmoothness = 0, GeometrySize = 0, Name = "Отправлено байт/с" },
                                                                        new LineSeries<int> {Values = new ObservableCollection<int>(), LineSmoothness = 0, GeometrySize = 0, Name = "Получено байт,с" } };

        private string _lastPolling = string.Empty;
        public string LastPolling {
            get { return _lastPolling; }
            set {
                _lastPolling = value;
                OnPropertyChanged("LastPolling");
            }
        }
        private string _nextPolling = string.Empty;
        public string NextPolling {
            get { return _nextPolling; }
            set {
                _nextPolling = value;
                OnPropertyChanged("NextPolling");
            }
        }

        private string _AFKDevicesNames = string.Empty;
        public string AFKDevicesNames {
            get { return _AFKDevicesNames; }
            set {
                _AFKDevicesNames = value;
                OnPropertyChanged("AFKDevicesNames");
            }
        }

        private string _AFKDevicesTimes = string.Empty;
        public string AFKDevicesTimes {
            get { return _AFKDevicesTimes; }
            set {
                _AFKDevicesTimes = value;
                OnPropertyChanged("AFKDevicesTimes");
            }
        }

        //Time
        public ISeries[] TimelineChart { get; set; } = new ISeries[] { new ColumnSeries<int> { Values = new ObservableCollection<int> ()},
                                                    new ColumnSeries<int> { Values = new ObservableCollection<int> ()} };

        public Axis[] TimelineXAxes { get; set; } = new Axis[] { new Axis { Name = "Средний объем данных, Мбит", Labels = new List<string>() } };
        public Axis[] TimelineYAxes { get; set; } = new Axis[] { new Axis { Name = "Средняя время передачи данных, с" } };
        public List<string> TimeLineSerieNames = new List<string>() { "Время передачи, с:", "Лучшеее время передачи, с:" };

        //Speed
        public ISeries[] SpeedlineChart { get; set; } = new ISeries[] { new ColumnSeries<int> { Values = new ObservableCollection<int>() } };
        public Axis[] SpeedlineXAxes { get; set; } = new Axis[] { new Axis { Name = "Средний объем данных, Мбит", Labels = new List<string>() } };
        public Axis[] SpeedlineYAxes { get; set; } = new Axis[] { new Axis { Name = "Средняя скорость передачи данных, Мбит\\с" } };
        public List<string> SpeedLineSerieNames = new List<string>() { "Скорость передачи, Мб/c" };

        SpeedsChart speedsChart = new SpeedsChart();

        public void GetStatisticCharts(string period, List<int> time, List<int> size) {
            /*switch (period) {
                case "Неделя":
                case "Месяц":
                    *//*TimelineChart = new ISeries[] { new ColumnSeries<int> { Values = new ObservableCollection<int> ()},
                                                    new ColumnSeries<int> { Values = new ObservableCollection<int> ()} };
                    *//*TimelineXAxes = new Axis[] { new Axis { Name = "Средний объем данных, Мбит", Labels = new List<string>() } };
                    TimelineYAxes = new Axis[] { new Axis { Name = "Средняя время передачи данных, с" } };

                    SpeedlineChart = new ISeries[] { new ColumnSeries<int> { Values = new ObservableCollection<int>() } };
                    SpeedlineXAxes = new Axis[] { new Axis { Name = "Средний объем данных, Мбит", Labels = new List<string>() } };
                    SpeedlineYAxes = new Axis[] { new Axis { Name = "Средняя скорость передачи данных, Мбит\\с" } };
                    break;
                case "День":
                    TimelineChart = new ISeries[] { new LineSeries<int> { Values = new ObservableCollection<int> ()},
                                                    new LineSeries<int> { Values = new ObservableCollection<int> ()} };
                    TimelineXAxes = new Axis[] { new Axis { Name = "Объем данных, Мбит", Labels = new List<string>() } };
                    TimelineYAxes = new Axis[] { new Axis { Name = "Время передачи данных, с" } };

                    SpeedlineChart = new ISeries[] { new LineSeries<int> { Values = new ObservableCollection<int>() } };
                    SpeedlineXAxes = new Axis[] { new Axis { Name = "Объем данных, Мбит", Labels = new List<string>() } };
                    SpeedlineYAxes = new Axis[] { new Axis { Name = "Скорость передачи данных, Мбит\\с" } };
                    break;
            }*/
            
            speedsChart.GetTimeLine(TimelineChart, TimelineXAxes, TimelineYAxes, time, size, TimeLineSerieNames);
            List<int> speed = new List<int>();
            foreach (int i in time) {
                if (i > 0) {
                    speed.Add(size[time.IndexOf(i)] / i);
                } else {
                    speed.Add(0);
                }
            }
            speedsChart.GetTimeLine(SpeedlineChart, SpeedlineXAxes, SpeedlineYAxes, speed, size, SpeedLineSerieNames);
        }
        public ObservableCollection<DeviceStatus> DeviceStats {
            get {
                return _deviceStats;
            }
            set { 
                _deviceStats = value;
                OnPropertyChanged("DeviceStats");
            } 
        } 
        private ObservableCollection<DeviceStatus> _deviceStats = new ObservableCollection<DeviceStatus>();
    }
}
