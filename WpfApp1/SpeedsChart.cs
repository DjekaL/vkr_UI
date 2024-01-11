using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApp1 {
    internal class SpeedsChart {
        private ObservableCollection<ObservablePoint> values;
        public ObservableCollection<ISeries> Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public DrawMarginFrame Frame { get; set; } = new() {
            Stroke = new SolidColorPaint {
                Color = new(0, 0, 0)
            }
        };
        public SpeedsChart(List<List<decimal>> y, List<decimal> xPoints, List<string> serieName) {
            Series = new ObservableCollection<ISeries>();
            List<string> labels = new List<string>();
            foreach (var item in xPoints) {
                labels.Add(item.ToString());
            }
            for (int k = 0; k < y.Count; k++) {
                List<decimal> x = new List<decimal>(xPoints);
                values = new ObservableCollection<ObservablePoint>();
                LineSeries<ObservablePoint> serie = new LineSeries<ObservablePoint>();
                for (int i = 0; i < y[k].Count; i++) {
                    values.Add(new ObservablePoint((double)x.IndexOf(x[i]), (double)y[k][i] ));
                    x[i] = 0;
                }
                XAxes = new Axis[] {
                new Axis {
                    Name = "Объем данных, Мбит",
                    Labels = labels
                }
            };

                YAxes = new Axis[] {
                new Axis {
                    Name = "Время передачи данных, с"
                    
                }
            };
                serie.Values = values;
                serie.Fill = null;
                //serie.GeometrySize = 3;
                serie.Name = serieName[k];
                serie.YToolTipLabelFormatter = (chartPoint) => $"{chartPoint.Coordinate.PrimaryValue}:";
                serie.XToolTipLabelFormatter = (chartPoint) => $"{XAxes[0].Name}: {XAxes[0].Labels.ElementAt((System.Index)chartPoint.Coordinate.SecondaryValue)}";
                Series.Add(serie);
            }
        }
    }
}
