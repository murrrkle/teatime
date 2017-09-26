using System;
using System.Collections.Generic;
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

namespace teatime
{
    /// <summary>
    /// Interaction logic for PlanetPoint.xaml
    /// </summary>
    public partial class PlanetPoint : UserControl
    {
        public int Type { get; set; }
        public int Direction { get; set; }

        public const int SHORT = 0;
        public const int MED = 1;
        public const int LONG = 2;

        public const int UP = 0;
        public const int RIGHT = 1;
        public const int DOWN = 2;
        public const int LEFT = 3;

        public PlanetPoint()
        {
            InitializeComponent();
            Type = SHORT;
            Direction = UP;
            this.MouseEnter += PlanetPoint_MouseEnter;
            this.MouseLeave += PlanetPoint_MouseLeave;
            this.MouseDown += PlanetPoint_MouseDown;
            this.MouseUp += PlanetPoint_MouseUp;
        }

        private void PlanetPoint_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.AddPlanet(Type);
        }

        private void PlanetPoint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            MainWindow.ChoosePlanet(mousePos, Type, Direction);
        }

        private void PlanetPoint_MouseLeave(object sender, MouseEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(@"/img/button-planet.png", UriKind.Relative));
        }

        private void PlanetPoint_MouseEnter(object sender, MouseEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(@"/img/button-planet-hover.png", UriKind.Relative));
        }
    }
}
