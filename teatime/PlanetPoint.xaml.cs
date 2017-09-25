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
        public PlanetPoint()
        {
            InitializeComponent();

            this.MouseEnter += PlanetPoint_MouseEnter;
            this.MouseLeave += PlanetPoint_MouseLeave;
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
