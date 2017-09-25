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

namespace teatime
{
    /// <summary>
    /// Interaction logic for Planet.xaml
    /// </summary>
    public partial class Planet : UserControl
    {
        public Planet(int type)
        {
            InitializeComponent();

            switch (type)
            {
                case 0:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-small.png", UriKind.Relative));

                    break;
                case 1:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-med.png", UriKind.Relative));
                    break;

                case 2:
                    image.Source = new BitmapImage(new Uri(@"/img/planet-large.png", UriKind.Relative));
                    break;

                default:
                    Debug.WriteLine("This shouldn't have happened.");
                    break;
            }
        }
    }
}
