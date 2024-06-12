using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

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
        public SpeedsChart() { }
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
                    values.Add(new ObservablePoint((double)x.IndexOf(x[i]), (double)y[k][i]));
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
                serie.YToolTipLabelFormatter = (chartPoint) => $"{chartPoint.Coordinate.PrimaryValue}";
                serie.XToolTipLabelFormatter = (chartPoint) => $"{XAxes[0].Name}: {XAxes[0].Labels.ElementAt((System.Index)chartPoint.Coordinate.SecondaryValue)}";
                Series.Add(serie);
            }
        }
        public SpeedsChart(List<decimal> y, List<decimal> xPoints, List<string> serieName) {
            Series = new ObservableCollection<ISeries>();
            List<string> labels = new List<string>();
            foreach (var item in xPoints) {
                labels.Add(item.ToString());
            }
            List<decimal> x = new List<decimal>(xPoints);
            values = new ObservableCollection<ObservablePoint>();
            LineSeries<ObservablePoint> serie = new LineSeries<ObservablePoint>();

            for (int k = 0; k < y.Count; k++) {
                values.Add(new ObservablePoint((double)x.IndexOf(x[k]), (double)y[k]));
                x[k] = 0;
            }

            XAxes = new Axis[] {
                new Axis {
                    Name = "Объем данных, Мбит",
                    Labels = labels
                }
            };

            YAxes = new Axis[] {
                new Axis {
                    Name = @"Скорость передачи данных, Мбит\с"
                }
            };
            serie.Values = values;
            serie.Fill = null;
            serie.Name = serieName[0];
            serie.YToolTipLabelFormatter = (chartPoint) => $"{chartPoint.Coordinate.PrimaryValue}";
            serie.XToolTipLabelFormatter = (chartPoint) => $"{XAxes[0].Name}: {XAxes[0].Labels.ElementAt((System.Index)chartPoint.Coordinate.SecondaryValue)}";
            Series.Add(serie);

        }

        public void GetTimeLine(ISeries[] Series, Axis[] xAxes, Axis[] yAxes, List<int> time, List<int> size, List<string> serieNames) {
            int k = 0;
            foreach (ColumnSeries<int> serie in Series) {
                serie.Values = new ObservableCollection<int>();
                List<string> labels = new List<string>();
                foreach (var item in size) {
                    labels.Add(item.ToString());
                }
                xAxes[0].Labels = labels;
                
                foreach (int i in time) {
                    (serie.Values as ObservableCollection<int>).Add(i);
                }

                //serie.IgnoresBarPosition = true;
                serie.Name = serieNames[k];
                k++;
                serie.YToolTipLabelFormatter = (chartPoint) => $"{chartPoint.Coordinate.PrimaryValue}";
                serie.XToolTipLabelFormatter = (chartPoint) => $"{xAxes[0].Name}: {xAxes[0].Labels.ElementAt((System.Index)chartPoint.Coordinate.SecondaryValue)}";
            }

        }
    }
}
