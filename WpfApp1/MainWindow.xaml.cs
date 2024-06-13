using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Confuguration;
using WpfApp1.Models;

namespace WpfApp1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        string _userCat = "";
        int topologSize = 0;
        int startVertex = 0;
        //public bool _isGetPerfomance = false;
        MainWindowDataModel _mainDataModel = new();

        Dictionary<int, string> dic = new Dictionary<int, string>();
        List<int> secondPoints = new List<int>();
        List<decimal> secondPointsTimes = new List<decimal>();
        decimal[,] cost;
        List<Connection> connections;

        ApplicationContext _context = new ApplicationContext();
        SQLiteHeper _db;

        public MainWindow() {
            InitializeComponent();
            Authorization authorization = new Authorization();
            authorization.ShowDialog();
            _userCat = authorization.cat;
            if (_userCat == "admin") {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }
            if (_userCat == "") {
                this.Close();
            }
            this.DataContext = _mainDataModel;

            _db = new SQLiteHeper(_context);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            /* List<Connection> connections = new List<Connection>();
             connections.Add(new Connection(1, 3, 200, "Сервер1"));
             connections.Add(new Connection(2, 4, 200, "Сервер2"));
             connections.Add(new Connection(3, 1, 200, ""));
             connections.Add(new Connection(3, 5, 200, ""));
             connections.Add(new Connection(4, 2, 200, ""));
             connections.Add(new Connection(4, 6, 200, ""));
             connections.Add(new Connection(5, 3, 200, ""));
             connections.Add(new Connection(5, 6, 200, ""));
             connections.Add(new Connection(6, 4, 200, ""));
             connections.Add(new Connection(6, 5, 200, ""));
             connections.Add(new Connection(6, 7, 200, ""));
             connections.Add(new Connection(7, 6, 150, ""));
             connections.Add(new Connection(7, 8, 150, ""));
             connections.Add(new Connection(7, 9, 150, ""));

             connections.Add(new Connection(8, 7, 150, ""));
             connections.Add(new Connection(8, 11, 100, ""));
             connections.Add(new Connection(8, 12, 100, ""));
             connections.Add(new Connection(8, 13, 100, ""));
             connections.Add(new Connection(8, 14, 100, ""));
             connections.Add(new Connection(8, 15, 100, ""));
             connections.Add(new Connection(8, 16, 100, ""));
             connections.Add(new Connection(8, 17, 100, ""));
             connections.Add(new Connection(8, 18, 100, ""));
             connections.Add(new Connection(8, 19, 100, ""));
             connections.Add(new Connection(8, 20, 100, ""));
             connections.Add(new Connection(8, 21, 100, ""));
             connections.Add(new Connection(8, 22, 100, ""));

             connections.Add(new Connection(9, 7, 150, ""));
             connections.Add(new Connection(9, 10, 150, ""));
             connections.Add(new Connection(9, 23, 100, ""));
             connections.Add(new Connection(9, 24, 100, ""));
             connections.Add(new Connection(9, 25, 100, ""));
             connections.Add(new Connection(9, 26, 100, ""));
             connections.Add(new Connection(9, 27, 100, ""));
             connections.Add(new Connection(9, 28, 100, ""));
             connections.Add(new Connection(9, 29, 100, ""));
             connections.Add(new Connection(9, 30, 100, ""));
             connections.Add(new Connection(9, 31, 100, ""));
             connections.Add(new Connection(9, 32, 100, ""));

             connections.Add(new Connection(10, 9, 150, ""));
             connections.Add(new Connection(10, 33, 100, ""));
             connections.Add(new Connection(10, 34, 100, ""));
             connections.Add(new Connection(10, 35, 100, ""));
             connections.Add(new Connection(10, 36, 100, ""));
             connections.Add(new Connection(10, 37, 100, ""));
             connections.Add(new Connection(10, 38, 100, ""));
             connections.Add(new Connection(10, 39, 100, ""));
             connections.Add(new Connection(10, 40, 100, ""));
             connections.Add(new Connection(10, 41, 100, ""));
             connections.Add(new Connection(10, 42, 100, ""));
             connections.Add(new Connection(10, 43, 100, ""));
             connections.Add(new Connection(10, 44, 100, ""));

             connections.Add(new Connection(11, 8, 100, "Бухгалтерия1"));
             connections.Add(new Connection(12, 8, 100, "Бухгалтерия2"));
             connections.Add(new Connection(13, 8, 100, "Бухгалтерия3"));
             connections.Add(new Connection(14, 8, 100, "Бухгалтерия4"));
             connections.Add(new Connection(15, 8, 100, "Служба Безопасности1"));
             connections.Add(new Connection(16, 8, 100, "Служба Безопасности2"));
             connections.Add(new Connection(17, 8, 100, "Служба Безопасности3"));
             connections.Add(new Connection(18, 8, 100, "Служба Безопасности4"));
             connections.Add(new Connection(19, 8, 100, "Администрация1"));
             connections.Add(new Connection(20, 8, 100, "Администрация2"));
             connections.Add(new Connection(21, 8, 100, "Администрация3"));
             connections.Add(new Connection(22, 8, 100, "Администрация4"));
             connections.Add(new Connection(23, 9, 100, "Производство1"));
             connections.Add(new Connection(24, 9, 100, "Производство2"));
             connections.Add(new Connection(25, 9, 100, "Производство3"));
             connections.Add(new Connection(26, 9, 100, "Производство4"));
             connections.Add(new Connection(27, 9, 100, "Производство5"));
             connections.Add(new Connection(28, 9, 100, "Логистика1"));
             connections.Add(new Connection(29, 9, 100, "Логистика2"));
             connections.Add(new Connection(30, 9, 100, "Логистика3"));
             connections.Add(new Connection(31, 9, 100, "Логистика4"));
             connections.Add(new Connection(32, 9, 100, "Логистика5"));
             connections.Add(new Connection(33, 10, 100, "Контроль качества1"));
             connections.Add(new Connection(34, 10, 100, "Контроль качества2"));
             connections.Add(new Connection(35, 10, 100, "Контроль качества3"));
             connections.Add(new Connection(36, 10, 100, "Контроль качества4"));
             connections.Add(new Connection(37, 10, 100, "Мониторинг1"));
             connections.Add(new Connection(38, 10, 100, "Мониторинг2"));
             connections.Add(new Connection(39, 10, 100, "Мониторинг3"));
             connections.Add(new Connection(40, 10, 100, "Мониторинг4"));
             connections.Add(new Connection(41, 10, 100, "Отдел продаж1"));
             connections.Add(new Connection(42, 10, 100, "Отдел продаж2"));
             connections.Add(new Connection(43, 10, 100, "Отдел продаж3"));
             connections.Add(new Connection(44, 10, 100, "Отдел продаж4"));

             string json = JsonConvert.SerializeObject(connections, Formatting.Indented);

             using (FileStream fs = new FileStream(@"./list1.json", FileMode.OpenOrCreate)) {
                 var bytes = Encoding.UTF8.GetBytes(json);
                 fs.Write(bytes, 0, bytes.Length);
             }*/

            string json = File.ReadAllText(@"./list1.json");
            connections = JsonConvert.DeserializeObject<List<Connection>>(json);

            foreach (var con in connections) {
                if (con.name != String.Empty) {
                    dic.Add(con.firstDevice, con.name);
                }
            }

            foreach (var item in dic.Values) {
                senderDevices.Items.Add(item);
            }

            //Опрос
            DevicePolling polling = new DevicePolling(_mainDataModel, 5000, new List<string> { "192.168.3.2", "123", "342" });
            polling.StartDevicePolling();

            //Графики маршрутов
            StatisticalPeriod.ItemsSource = new List<string>() { "День", "Неделя", "Месяц" };
        }
        private void senderDevices_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            recipientDevices.Items.Clear();
            secondPoints.Clear();
            secondPointsTimes.Clear();

            topologSize = connections.Count / 2 + 1;
            var smeg = new int[topologSize, topologSize];
            cost = new decimal[topologSize, topologSize];


            Dijkstra.Set(topologSize, smeg, cost);
            Dijkstra.Get(connections, smeg, cost, 100);

            startVertex = dic.Where(x => x.Value == senderDevices.SelectedItem.ToString()).FirstOrDefault().Key;

            Dijkstra.Deijkstra(cost, startVertex - 1, topologSize, secondPoints, secondPointsTimes);

            foreach (var point in secondPoints) {
                if (dic.ContainsKey(point)) {
                    recipientDevices.Items.Add(dic[point]);
                    /*secondPoints.RemoveAt(point);
                    secondPointsTimes.RemoveAt(point);*/
                }
            }
        }

        private void recipientDevices_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (recipientDevices.SelectedItem != null) {
                secondPoints.Clear();
                DataTable table = new DataTable();

                DataColumn column = new DataColumn();
                column.ColumnName = "Отправитель";
                table.Columns.Add(column);

                column = new DataColumn();
                column.ColumnName = "Получатель";
                table.Columns.Add(column);

                column = new DataColumn();
                column.ColumnName = "Размер данных, Мбит";
                column.DataType = Type.GetType("System.Decimal");
                table.Columns.Add(column);

                column = new DataColumn();
                column.ColumnName = "Средняя скорость передачи данных";
                column.DataType = Type.GetType("System.Decimal");
                table.Columns.Add(column);

                column = new DataColumn();
                column.ColumnName = "Время передачи, с";
                column.DataType = Type.GetType("System.Decimal");
                table.Columns.Add(column);

                column = new DataColumn();
                column.ColumnName = "теоретическое время передачи, с";
                column.DataType = Type.GetType("System.Decimal");
                table.Columns.Add(column);


                List<List<decimal>> x = new List<List<decimal>>();
                List<decimal> realTime = new List<decimal>();
                List<decimal> perfectTime = new List<decimal>();
                List<decimal> speeds = new List<decimal>();
                List<decimal> size = new List<decimal>();
                DataRow row;
                for (int i = 0; i < 10; i++) {
                    row = table.NewRow();
                    row[0] = senderDevices.SelectedItem.ToString();
                    row[1] = recipientDevices.SelectedItem.ToString();
                    Random random = new Random();
                    decimal dataSize = random.Next(50, 1010);
                    row[2] = dataSize;
                    size.Add(dataSize);

                    var smeg = new int[topologSize, topologSize];
                    cost = new decimal[topologSize, topologSize];


                    Dijkstra.Set(topologSize, smeg, cost);
                    Dijkstra.Get(connections, smeg, cost, dataSize);

                    startVertex = dic.Where(x => x.Value == senderDevices.SelectedItem.ToString()).FirstOrDefault().Key;
                    secondPointsTimes.Clear();
                    Dijkstra.Deijkstra(cost, startVertex - 1, topologSize, secondPoints, secondPointsTimes);
                    int ab = dic.Where(x => x.Value == recipientDevices.SelectedItem.ToString()).FirstOrDefault().Key;
                    int index = secondPoints.IndexOf(dic.Where(x => x.Value == recipientDevices.SelectedItem.ToString()).FirstOrDefault().Key);
                    decimal time = Math.Round(secondPointsTimes[index], 1);
                    double coef = (double)dataSize / 100;
                    decimal MonitTime = Math.Round((decimal)(random.NextDouble() * (((double)time + 10) - ((double)time + 2)) + (double)time + 2), 1);
                    decimal speed = Math.Round(dataSize / MonitTime);
                    row[3] = speed;
                    speeds.Add(speed);
                    realTime.Add(MonitTime);
                    row[4] = MonitTime;
                    row[5] = time;
                    perfectTime.Add((decimal)time);
                    table.Rows.Add(row);
                }
                x.Add(realTime);
                x.Add(perfectTime);
                routStatistics.ItemsSource = table.DefaultView;
                routStatistics.Columns[3].Header = "Средняя скорость передачи данных, Мбит/с";

                // Times charts
                List<string> names = new List<string> { "Время передачи, с", "Лучшее время передачи, с" };
                SpeedsChart chart = new SpeedsChart(x, size, names);
                //timesChart.DataContext = chart;

                // Speed chart
                names = new List<string> { "Скорость передачи" };
                chart = new SpeedsChart(speeds, size, names);
                //speedsChart.DataContext = chart;
            }
        }

        PerfomanceReciever perf;
        private void start_Click(object sender, RoutedEventArgs e) {
            //isRun = true;
            perf = new PerfomanceReciever(_mainDataModel);
            Task.Run(() => {
                _mainDataModel.isGetPerfomance = true;
                perf.RecieverStart();
                PerfomanceChart chart = new PerfomanceChart();

            });
        }

        private void end_Click(object sender, RoutedEventArgs e) {
            //isRun = false;
            _mainDataModel.isGetPerfomance = false;
            perf.IsContinue = false;
        }

        private void StatisticalPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            List<int> time = new List<int>();
            List<int> size = new List<int>();
            _db.GetStatistic(StatisticalPeriod.SelectedItem.ToString(), senderDevices.SelectedItem.ToString(), recipientDevices.SelectedItem.ToString(), time, size);
            _mainDataModel.GetStatisticCharts(StatisticalPeriod.SelectedItem.ToString(), time, size);
            
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e) {
            Task.Run(() => {
                _mainDataModel.DeviceStats = _db.GetDeviceStats();
            });
            //deviceStatics.ItemsSource = _mainDataModel.DeviceStats;
        }
    }
}
