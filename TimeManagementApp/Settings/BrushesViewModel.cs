using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Collections.ObjectModel;

namespace TimeManagementApp.Settings
{
    public class BrushesViewModel
    {
        public ObservableCollection<LinearGradientBrush> BackgroundBrushes { get; set; }

        public BrushesViewModel()
        {
            BackgroundBrushes = new ObservableCollection<LinearGradientBrush>
            {
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.AliceBlue, Offset = 0.0 },
                        new GradientStop { Color = Colors.CornflowerBlue, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.MistyRose, Offset = 0.0 },
                        new GradientStop { Color = Colors.Salmon, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.LavenderBlush, Offset = 0.0 },
                        new GradientStop { Color = Colors.HotPink, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.Honeydew, Offset = 0.0 },
                        new GradientStop { Color = Colors.LightGreen, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.LightYellow, Offset = 0.0 },
                        new GradientStop { Color = Colors.Gold, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.Lavender, Offset = 0.0 },
                        new GradientStop { Color = Colors.SlateBlue, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.SeaShell, Offset = 0.0 },
                        new GradientStop { Color = Colors.Chocolate, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.PapayaWhip, Offset = 0.0 },
                        new GradientStop { Color = Colors.Orange, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.Aquamarine, Offset = 0.0 },
                        new GradientStop { Color = Colors.Teal, Offset = 1.0 }
                    }
                },
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.LightGoldenrodYellow, Offset = 0.0 },
                        new GradientStop { Color = Colors.Goldenrod, Offset = 1.0 }
                    }
                },
            };
        }
    }

}
