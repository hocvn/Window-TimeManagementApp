using Microsoft.UI.Xaml.Controls;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System.Linq;

namespace TimeManagementApp.Statistics
{
    public sealed partial class StatisticsPage : Page
    {
        private readonly StatisticsViewModel _viewModel = new StatisticsViewModel();

        public StatisticsPage()
        {
            this.InitializeComponent();
            this.DataContext = _viewModel;
            SetupPieChart();
            SetupLineChart();
            SetupTagBarChart();
        }

        private void SetupPieChart()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0,
                OutsideLabelFormat = "{1}: {0}",
                InsideLabelFormat = null,
                TextColor = OxyColors.Black,
                FontSize = 12
            };

            foreach (var taskStat in _viewModel.TaskStatistics)
            {
                pieSeries.Slices.Add(new PieSlice(taskStat.Name, taskStat.Value)
                {
                    Fill = OxyColor.FromRgb(taskStat.Color.R, taskStat.Color.G, taskStat.Color.B),
                    IsExploded = false
                });
            }

            var model = new PlotModel { Title = "Task Statistics", TitleFontSize = 14 };
            model.Series.Add(pieSeries);
            model.PlotMargins = new OxyThickness(40, 40, 40, 40);

            PieChartView.Model = model;
        }

        private void SetupLineChart()
        {
            var lineChartModel = new PlotModel { Title = "Focus Time by Timestamp and Tag", TitleFontSize = 14 };

            var legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.LeftTop,
                LegendOrientation = LegendOrientation.Vertical,
                LegendBorder = OxyColors.Black,
                LegendBorderThickness = 1
            };
            lineChartModel.Legends.Add(legend);

            var focusSessions = _viewModel.Sessions.Where(s => s.Type == "Focus");

            foreach (var tagGroup in focusSessions.GroupBy(s => s.Tag))
            {
                var lineSeries = new LineSeries { Title = tagGroup.Key };

                var timeGroups = tagGroup.GroupBy(s => new { s.Timestamp.Date, s.Timestamp.Hour, s.Timestamp.Minute })
                                          .Select(g => new { Time = g.Key, TotalDuration = g.Sum(s => s.Duration) })
                                          .OrderBy(g => g.Time.Date)
                                          .ThenBy(g => g.Time.Hour)
                                          .ThenBy(g => g.Time.Minute);

                foreach (var timeGroup in timeGroups)
                {
                    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timeGroup.Time.Date.AddHours(timeGroup.Time.Hour).
                        AddMinutes(timeGroup.Time.Minute)), timeGroup.TotalDuration / 60.0)); // Convert to minutes
                }

                lineChartModel.Series.Add(lineSeries);
            }

            lineChartModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MM/dd/yyyy",
            });

            lineChartModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
            });

            LineChartView.Model = lineChartModel;
        }

        private void SetupTagBarChart()
        {
            var tagBarChartModel = new PlotModel { Title = "Total Focus and Break Time by Tag", TitleFontSize = 14 };

            var legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop,
                LegendOrientation = LegendOrientation.Vertical,
                LegendBorder = OxyColors.Black,
                LegendBorderThickness = 1
            };
            tagBarChartModel.Legends.Add(legend);

            var focusSeries = new BarSeries { Title = "Focus", StrokeColor = OxyColors.Purple, FillColor = OxyColors.Purple, StrokeThickness = 1 };
            var breakSeries = new BarSeries { Title = "Break", StrokeColor = OxyColors.Orange, FillColor = OxyColors.Orange, StrokeThickness = 1 };

            var tags = _viewModel.Sessions.Select(s => s.Tag).Distinct().ToList();

            foreach (var tag in tags)
            {
                var focusTotal = _viewModel.Sessions.Where(s => s.Tag == tag && s.Type == "Focus").Sum(s => s.Duration) / 60.0; // Convert to minutes
                var breakTotal = _viewModel.Sessions.Where(s => s.Tag == tag && s.Type == "Break").Sum(s => s.Duration) / 60.0; // Convert to minutes

                focusSeries.Items.Add(new BarItem { Value = focusTotal });
                breakSeries.Items.Add(new BarItem { Value = breakTotal });
            }

            tagBarChartModel.Series.Add(focusSeries);
            tagBarChartModel.Series.Add(breakSeries);

            tagBarChartModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "TagsAxis",
                ItemsSource = tags
            });

            tagBarChartModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Duration (minutes)"
            });

            TagBarChartView.Model = tagBarChartModel;
        }
    }
}
