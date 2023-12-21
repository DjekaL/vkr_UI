using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _userCat = "";
        int topologSize = 0;
        int startVertex = 0;

        Dictionary<int, string> dic = new Dictionary<int, string>();
        List<int> secondPoints = new List<int>();
        int[,] cost;
        List<Connection> connections;

        public MainWindow()
        {
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

            topologSize = connections.Count;
            var smeg = new int[topologSize, topologSize];
            cost = new int[topologSize, topologSize];


            Dijkstra.Set(topologSize, smeg, cost);
            Dijkstra.Get(connections, smeg, cost, dic);

            foreach (var item in dic.Values) {
                senderDevices.Items.Add(item);
            }

        }
        private void senderDevices_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            recipientDevices.Items.Clear();
            secondPoints.Clear();

            startVertex = dic.Where(x => x.Value == senderDevices.SelectedItem.ToString()).FirstOrDefault().Key;

            Dijkstra.Deijkstra(cost, startVertex - 1, topologSize, secondPoints);

            foreach(var point in secondPoints) {
                if (dic.ContainsKey(point)) {
                    recipientDevices.Items.Add(dic[point]);
                }
            }
        }
    }
}
