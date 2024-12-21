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
            SetupTimeAreaChart();
        }

        private void SetupPieChart()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0,
                OutsideLabelFormat = "{1}: {0}", // Label format: "percentage: Task"
                InsideLabelFormat = null, // Use outside labels only
                TextColor = OxyColors.Black, // Ensure text color is readable
                FontSize = 12 // Adjust font size for better spacing
            };

            foreach (var taskStat in _viewModel.TaskStatistics)
            {
                pieSeries.Slices.Add(new PieSlice(taskStat.Name, taskStat.Value)
                {
                    Fill = OxyColor.FromRgb(taskStat.Color.R, taskStat.Color.G, taskStat.Color.B),
                    IsExploded = false
                });
            }

            var model = new PlotModel { Title = "Task Statistics" };
            model.Series.Add(pieSeries);

            // Adjust PlotMargins to ensure labels fit within the chart area
            model.PlotMargins = new OxyThickness(-40, 20, 30, 20); // Adjust margins as needed

            PieChartView.Model = model;
        }

        private void SetupLineChart()
        {
            var model = new PlotModel { Title = "Tags Breakdown by Timestamp" };
            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy"
            };
            model.Axes.Add(dateAxis);

            foreach (var tagData in _viewModel.TagTimeSeriesData)
            {
                var lineSeries = new LineSeries
                {
                    Title = tagData.Tag, // Ensure each series has a title
                    ItemsSource = tagData.DataPoints,
                    DataFieldX = nameof(DataPoint.X),
                    DataFieldY = nameof(DataPoint.Y),
                    TrackerFormatString = "{0}\n{1}: {2:0.00} min" // Display values in minutes with 2 decimal places
                };

                model.Series.Add(lineSeries);
            }

            // Add a legend
            var legend = new Legend
            {
                LegendPosition = LegendPosition.LeftTop,
                LegendPlacement = LegendPlacement.Outside,
                LegendBackground = OxyColors.White,
                LegendBorder = OxyColors.Black
            };
            model.Legends.Add(legend);

            LineChartView.Model = model;
        }

        private void SetupTagBarChart()
        {
            var model = new PlotModel { Title = "Total Focus Time on Each Tag" };
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "Tags"
            };
            categoryAxis.Labels.AddRange(_viewModel.TotalTimePerTag.Select(t => t.Tag).ToList());
            model.Axes.Add(categoryAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Total Time (minutes)", // Update to minutes
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                StringFormat = "0.00" // Display values with 2 decimal places
            };
            model.Axes.Add(valueAxis);

            var barSeries = new BarSeries
            {
                Title = "Total Time",
                LabelPlacement = LabelPlacement.Inside,
                FillColor = OxyColor.FromRgb(135, 206, 250), // Light blue fill color
                LabelFormatString = "{0:0.00} min" // Display values in minutes with 2 decimal places
            };

            foreach (var tagTotal in _viewModel.TotalTimePerTag)
            {
                barSeries.Items.Add(new BarItem { Value = tagTotal.TotalTime });
            }

            model.Series.Add(barSeries);
            TagBarChartView.Model = model;
        }

        private void SetupTimeAreaChart()
        {
            var model = new PlotModel { Title = "Total Focus Time Over Periods" };
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "Periods"
            };
            categoryAxis.Labels.AddRange(new[] { "Last Day", "Last Week", "Last Month" });
            model.Axes.Add(categoryAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Total Time (minutes)", // Update to minutes
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                StringFormat = "0.00" // Display values with 2 decimal places
            };
            model.Axes.Add(valueAxis);

            var areaSeries = new AreaSeries
            {
                Title = "Total Time",
                Fill = OxyColor.FromRgb(135, 206, 250)  // Light blue fill for the area
            };

            var timePeriods = new[]
            {
                new TimePeriod { Period = "Last Day", TotalTime = _viewModel.TotalTimeLastDay },
                new TimePeriod { Period = "Last Week", TotalTime = _viewModel.TotalTimeLastWeek },
                new TimePeriod { Period = "Last Month", TotalTime = _viewModel.TotalTimeLastMonth }
            };

            foreach (var period in timePeriods)
            {
                areaSeries.Points.Add(new DataPoint(categoryAxis.Labels.IndexOf(period.Period), period.TotalTime));
            }

            model.Series.Add(areaSeries);
            TimeAreaChartView.Model = model;
        }
    }

    public class TimePeriod
    {
        public string Period { get; set; }
        public double TotalTime { get; set; }
    }
}
