using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Dijkstra {

        public static void Set(int size, int[,] smeg, int[,] cost) {
            //заполнение матриц
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    smeg[i, j] = 0; //матрицу смежности - нулями
                    cost[i, j] = Int32.MaxValue; //стоимости - максимальным числом
                }
            }
        }
        public static void Get(List<Connection> cons, int[,] smeg, int[,] cost, Dictionary<int, string> dic) {
            int v; //вершина графа откуда дуга
            int w; //вершина графа куда входит дуга
            int c; //стоимость дуги

            foreach(var con in cons) {
                v = con.firstDevice;
                w = con.secondDevice;
                c = con.weight;
                smeg[v - 1, w - 1] = 1;
                cost[v - 1, w - 1] = c;
                if (con.name != String.Empty) {
                    dic.Add(con.firstDevice, con.name);
                }
            }
        }
        public static void Deijkstra(int[,] cost, int st, int size, List<int> seconds) {
            int[] distance = new int[size]; //массив стоимости
            bool[] visited = new bool[size]; //массив для посещенной вершины

            for (int i = 0; i < size; i++) {
                distance[i] = cost[st, i]; //заполняем макс. числом
                visited[i] = false; //пока вершина не посещена
            }

            Console.WriteLine();
            int u = 0;
            distance[st] = 0;

            for (int j = 0; j < size - 1; j++) {
                visited[u] = true; //вершина посещена
                u = MinimumDistance(distance, visited, size);

                //Нахождение кратчайшего пути
                for (int i = 0; i < size; i++) {
                    if (!visited[i] && cost[u, i] != Int32.MaxValue && distance[u] + cost[u, i] < distance[i]) {
                        distance[i] = distance[u] + cost[u, i];
                    }
                }
            }

            for (int i = 0; i < size; i++) {
                if (distance[i] != Int32.MaxValue && i != st ) {
                    seconds.Add(i + 1);
                }
            }
            
        }
        private static int MinimumDistance(int[] distance, bool[] visited, int size) {
            int min = Int32.MaxValue;
            int minIndex = 0;

            for (int i = 0; i < size; i++) {
                if (!visited[i] && distance[i] <= min) //пока вершина не посещена и стоимость меньшье либо равна "максимальному" числу
                {
                    min = distance[i]; //присваиваем эту стоимость
                    minIndex = i; //сохраняем индекс вершины
                }
            }
            return minIndex;
        }
    }
}
