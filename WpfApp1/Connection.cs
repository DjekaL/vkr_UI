using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Connection {
        public int firstDevice { get; set; }
        public int secondDevice { get; set; }
        public int weight { get; set; }
        public string name { get; set; }

        public Connection(int first, int second, int speed, string name) { 
            firstDevice = first;
            secondDevice = second;  
            weight = speed;
            this.name = name;
        }
    }
}
