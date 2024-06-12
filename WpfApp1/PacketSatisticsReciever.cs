using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WpfApp1.Confuguration;
using WpfApp1.Confuguration.Models;

namespace WpfApp1 {
    public class PacketSatisticsReciever {

        ApplicationContext _dbContext;

        TcpListener _listener;
        TcpClient _tcpClient;
        NetworkStream _stream;
        Statistics _statistics;
        public Statistics Statistics {
            get { return _statistics; }
            set { 
                _statistics = value; 
            }
        }

        public PacketSatisticsReciever() { }

        public async void SatisticsRecieverStart() {
            _listener = new TcpListener(IPAddress.Any, 8080);
            _listener.Start();
            while (true) {
                _tcpClient = await _listener.AcceptTcpClientAsync();
                _stream = _tcpClient.GetStream();
                var data = new List<byte>();
                try {
                    while (true) {
                        var message = string.Empty;
                        int dataSize = 0;
                        while ((dataSize = _stream.ReadByte()) != '\n') {
                            data.Add((byte)dataSize);
                        }
                        message = Encoding.UTF8.GetString(data.ToArray());
                        data.Clear();
                        var stat = JsonConvert.DeserializeObject<Statistics>(message);
                        stat.ipSender = (_tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
                    }
                }
                catch {
                    _tcpClient.Close();
                }
            }

           /* // Пришло два устройства
            var firstId = 1;
            var secId = 2;
            var connection = _dbContext.Connections
                .AsNoTracking()
                .FirstOrDefault(c => c.FirstDeviceId == firstId && c.SecondDeviceId == secId);

            if (connection != null) {
                DataTransferLog dLog = new DataTransferLog {
                    RecieveTime = new System.DateTime(),
                    SendingTime = new System.DateTime(),
                    DataSize = 0,
                    Connection = connection,
                    ConnectionId = connection.Id
                };

                _dbContext.DataTransferLogs.Add(dLog);
                await _dbContext.SaveChangesAsync();
            }
            else {
                throw new System.Exception();
            }

            DataTransferLog dataLog = new DataTransferLog();
            var firstDevice = _dbContext.DataTransferLogs
                .AsNoTracking()
                .Where(d => d.ConnectionId == 1)
                .Select(d => d.Connection)
                .Select(c => c.FirstDevice)
                .FirstOrDefault();*/
        }
    }
}
