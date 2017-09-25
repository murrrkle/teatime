using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace teatime
{
    /// <summary>
    /// Interaction logic for Planet.xaml
    /// </summary>
    public partial class Planet : UserControl
    {
        private DispatcherTimer timer;


        public int Size { get; }
        public int Orbit { get; }
        public int Direction { get; }
        public const int SMALL = 0;
        public const int MED = 1;
        public const int LARGE = 2;

        private int degreesDelta;

        public int degrees;
        public int radius;
        public Planet(int PlanetType, int orbitType, int direction)
        {
            InitializeComponent();
            degrees = 0;
            degreesDelta = 1;
            radius = 1;
            this.IsHitTestVisible = false;
            Size = PlanetType;
            Orbit = orbitType;
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            switch (PlanetType)
            {
                case SMALL:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-small.png", UriKind.Relative));
                    timer.Interval = TimeSpan.FromMilliseconds(10);
                    break;
                case MED:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-med.png", UriKind.Relative));
                    timer.Interval = TimeSpan.FromMilliseconds(20);
                    break;

                case LARGE:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-large.png", UriKind.Relative));
                    timer.Interval = TimeSpan.FromMilliseconds(40);
                    break;

                default:
                    Debug.WriteLine("This shouldn't have happened.");
                    break;
            }
            switch (orbitType)
            {
                case 0:
                    radius = 130;
                    degreesDelta = 3;
                    break;
                case 1:
                    radius = 190;
                    degreesDelta = 2;
                    break;
                case 2:
                    radius = 275;
                    degreesDelta = 1;
                    break;

            }

            switch (direction)
            {
                case 0:
                    degrees = 270;
                    break;
                case 1:
                    degrees = 0;
                    break;
                case 2:
                    degrees = 90;
                    break;
                case 3:
                    degrees = 180;
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).UpdatePlanetPosition(this);
            degrees += degreesDelta;
            if (degrees == 360)
                degrees = 0;
        }

        public void BeginOrbit()
        {
            timer.Start();
        }
    }
}
