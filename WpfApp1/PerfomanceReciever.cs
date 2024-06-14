using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1 {
    public class PerfomanceReciever {
        TcpClient _tcpClient;
        NetworkStream _stream;
        MainWindowDataModel _mainDataModel;
        //string Log { get; set; } = string.Empty;

        public PerfomanceReciever(MainWindowDataModel model) {
            _mainDataModel = model;
        }

        public async void RecieverStart() {
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync("192.168.3.10", 8888);
            _stream = _tcpClient.GetStream();
            IsContinue = true;
            var data = new List<byte>();
            while (_mainDataModel.isGetPerfomance) {
                var message = string.Empty;
                try {
                    int dataSize = 0;
                    while ((dataSize = _stream.ReadByte()) != '\n') {
                        data.Add((byte)dataSize);
                    }
                    message = Encoding.UTF8.GetString(data.ToArray());
                    data.Clear();
                    var perfomance = JsonConvert.DeserializeObject<Perfomance>(message);
                    //_mainDataModel.Log += perfomance;
                    _mainDataModel.Perfomances = perfomance;
                }
                catch {
                    _tcpClient.Close();
                }
            }
        }

        private async Task SendCommandAsync(string command) {
            try {
                var data = Encoding.UTF8.GetBytes(command);
                await _stream.WriteAsync(data);
            }
            catch { }
        }

        bool _isContinue;
        public bool IsContinue {
            get {
                return this._isContinue;
            }
            set {
                this._isContinue = value;
                if (value == false) {
                    SendCommandAsync("end");
                    _tcpClient.Close();
                } else {
                    SendCommandAsync("start");
                }
            }
        }
    }
}
