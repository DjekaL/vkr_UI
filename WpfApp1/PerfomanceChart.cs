using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;

namespace WpfApp1 {
    public class PerfomanceChart {
        public PerfomanceChart() { }

        public void GetNewChcartVaalue(ISeries[] Series, List<int> values) {
            foreach (LineSeries<int> serie in Series) {
                (serie.Values as ObservableCollection<int>).Add(values[Array.IndexOf(Series, serie)]);
                //serie.Values = new int[] { (serie.Values.ToArray()), values[Array.IndexOf(Series, serie)] };
            }
        }
    }
}
