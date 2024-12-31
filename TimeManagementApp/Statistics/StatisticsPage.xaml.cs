using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Microsoft.UI.Xaml.Controls;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Defaults;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;

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
            SetupTaskStatusPieChart();
            SetupLineChart();
            SetupTagBarChart();
            SetupFocusTimeBarChart();
        }

        private void SetupPieChart()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelFormat = "{1}",
                InsideLabelPosition = 0.6,
                AreInsideLabelsAngled = true,
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

            var model = new PlotModel { Title = "Task Due Statistics", TitleFontSize = 14 };
            model.Series.Add(pieSeries);
            model.PlotMargins = new OxyThickness(35, 35, 35, 35);
            model.Background = OxyColors.Transparent;

            PieChartView.Model = model;
        }

        private void SetupTaskStatusPieChart()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelFormat = "{1}",
                InsideLabelPosition = 0.6,
                AreInsideLabelsAngled = true,
                TextColor = OxyColors.Black,
                FontSize = 12
            };

            foreach (var taskStat in _viewModel.TaskStatusStatistics)
            {
                pieSeries.Slices.Add(new PieSlice(taskStat.Name, taskStat.Value)
                {
                    Fill = OxyColor.FromRgb(taskStat.Color.R, taskStat.Color.G, taskStat.Color.B),
                    IsExploded = false
                });
            }

            var model = new PlotModel { Title = "Task Status Statistics", TitleFontSize = 14 };
            model.Series.Add(pieSeries);
            model.PlotMargins = new OxyThickness(35, 35, 35, 35);
            model.Background = OxyColors.Transparent;

            TaskStatusPieChartView.Model = model;
        }

        private void SetupLineChart()
        {
            var lineChartModel = new PlotModel { Title = "Focus Time by Timestamp and Tag", TitleFontSize = 14 };

            var legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop,
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
                    lineSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(timeGroup.Time.Date.AddHours(timeGroup.Time.Hour).
                        AddMinutes(timeGroup.Time.Minute)), timeGroup.TotalDuration / 60.0)); // Convert to minutes
                }

                lineChartModel.Series.Add(lineSeries);
            }

            lineChartModel.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MM/dd/yyyy",
            });

            lineChartModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
            });

            lineChartModel.Background = OxyColors.Transparent;
            LineChartView.Model = lineChartModel;
        }

        private void SetupTagBarChart()
        {
            var tagBarChartModel = new PlotModel { Title = "Total Focus and Break Time by Tag", TitleFontSize = 14 };

            var legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.LeftBottom,
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

            tagBarChartModel.Background = OxyColors.Transparent;
            TagBarChartView.Model = tagBarChartModel;
        }

        private void SetupFocusTimeBarChart()
        {
            var barChartModel = new PlotModel { Title = "Average Focus Time by Timestamp", TitleFontSize = 14 };

            var tags = _viewModel.Sessions.Select(s => s.Tag).Distinct().ToList();

            var focusSeriesDay = new BarSeries { Title = "Focus - Last Day", FillColor = OxyColor.FromRgb(128, 0, 128), StackGroup = "1" }; // Purple
            var focusSeriesWeek = new BarSeries { Title = "Focus - Last Week", FillColor = OxyColor.FromRgb(255, 0, 255), StackGroup = "2" }; // Magenta
            var focusSeriesMonth = new BarSeries { Title = "Focus - Last Month", FillColor = OxyColor.FromRgb(0, 255, 255), StackGroup = "3" }; // Cyan

            var now = DateTime.Now;

            foreach (var tag in tags)
            {
                var focusLastDay = _viewModel.Sessions.Where(s => s.Tag == tag && s.Type == "Focus" && (now - s.Timestamp).TotalDays <= 1).
                    DefaultIfEmpty().Average(s => s == null ? 0 : s.Duration) / 60.0; // Average duration in minutes
                var focusLastWeek = _viewModel.Sessions.Where(s => s.Tag == tag && s.Type == "Focus" && (now - s.Timestamp).TotalDays <= 7).
                    DefaultIfEmpty().Average(s => s == null ? 0 : s.Duration) / 60.0;
                var focusLastMonth = _viewModel.Sessions.Where(s => s.Tag == tag && s.Type == "Focus" && (now - s.Timestamp).TotalDays <= 30).
                    DefaultIfEmpty().Average(s => s == null ? 0 : s.Duration) / 60.0;

                focusSeriesDay.Items.Add(new BarItem { Value = focusLastDay, CategoryIndex = tags.IndexOf(tag) });
                focusSeriesWeek.Items.Add(new BarItem { Value = focusLastWeek, CategoryIndex = tags.IndexOf(tag) });
                focusSeriesMonth.Items.Add(new BarItem { Value = focusLastMonth, CategoryIndex = tags.IndexOf(tag) });
            }

            barChartModel.Series.Add(focusSeriesDay);
            barChartModel.Series.Add(focusSeriesWeek);
            barChartModel.Series.Add(focusSeriesMonth);

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left, Key = "TagsAxis", ItemsSource = tags };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                Title = "Average Duration (minutes)"
            };

            // Add legend
            var legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorder = OxyColors.Black,
                LegendBorderThickness = 1
            };
            barChartModel.Legends.Add(legend);

            barChartModel.Axes.Add(categoryAxis);
            barChartModel.Axes.Add(valueAxis);

            barChartModel.Background = OxyColors.Transparent;
            FocusTimeBarChartView.Model = barChartModel;
        }

    }
}
