using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Dao;
using Windows.Foundation;

namespace TimeManagementApp.Global
{
    public class BackgroundViewModel : INotifyPropertyChanged
    {
        public LinearGradientBrush PageBackgroundBrush { get; set; }
        public LinearGradientBrush NavigationViewBackgroundBrush { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private const double PageGradientStopOffset1 = 0.0;
        private const double PageGradientStopOffset2 = 8.0;
        private const double NavigationViewGradientStopOffset1 = 0.0;
        private const double NavigationViewGradientStopOffset2 = 2.5;


        public BackgroundViewModel()
        {
            IDao dao = new LocalSettingsDao();

            PageBackgroundBrush = dao.LoadSavedBackground(PageGradientStopOffset1, PageGradientStopOffset2);
            NavigationViewBackgroundBrush = dao.LoadSavedBackground(NavigationViewGradientStopOffset1, NavigationViewGradientStopOffset2);

            if (PageBackgroundBrush == null)
            {
                PageBackgroundBrush = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.AliceBlue, Offset = PageGradientStopOffset1 },
                        new GradientStop { Color = Colors.CornflowerBlue, Offset = PageGradientStopOffset2 }
                    }
                };

                NavigationViewBackgroundBrush = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.AliceBlue, Offset = NavigationViewGradientStopOffset1 },
                        new GradientStop { Color = Colors.CornflowerBlue, Offset = NavigationViewGradientStopOffset2 }
                    }
                };
            }
        }
    }
}
