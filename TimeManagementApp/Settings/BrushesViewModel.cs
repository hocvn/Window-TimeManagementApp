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
using Windows.UI;

namespace TimeManagementApp.Settings
{
    public class BrushesViewModel
    {
        public ObservableCollection<LinearGradientBrush> BackgroundBrushes { get; set; }

        public BrushesViewModel()
        {
            var colorPairs = new List<(Color StartColor, Color EndColor)>
            {
                (Colors.AliceBlue, Colors.CornflowerBlue),
                (Colors.MistyRose, Colors.Salmon),
                (Colors.LavenderBlush, Colors.HotPink),
                (Colors.Honeydew, Colors.LightGreen),
                (Colors.LightYellow, Colors.Gold),
                (Colors.Lavender, Colors.SlateBlue),
                (Colors.SeaShell, Colors.Chocolate),
                (Colors.PapayaWhip, Colors.Orange),
                (Colors.Aquamarine, Colors.Teal),
                (Colors.LightGoldenrodYellow, Colors.Goldenrod),
                // other themes go here
            };

            BackgroundBrushes = new ObservableCollection<LinearGradientBrush>();

            foreach (var (startColor, endColor) in colorPairs)
            {
                BackgroundBrushes.Add(new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = startColor, Offset = 0.0 },
                        new GradientStop { Color = endColor, Offset = 1.0 }
                    }
                });
            }
        }
    }
}
