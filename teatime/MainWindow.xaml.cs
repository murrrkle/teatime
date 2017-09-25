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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer orbitTimer;
        private DispatcherTimer idleTimer;
        private Vector planets;
        private Int32 holdTime;

        private const int SMALL = 0;
        private const int MED = 1;
        private const int LARGE = 2;

        private const int IDLE = 0;
        private const int POURING = 1;
        private const int DRINKING = 2;
        private const int HAPPY = 3;
        private int boxy_status = IDLE;

        private bool started = false;

        private bool teaPouring = false;

        public MainWindow()
        {
            InitializeComponent();
            HideElements(); 

            orbitTimer = new DispatcherTimer(); // syncs planet orbits
            idleTimer = new DispatcherTimer(); // plays an idle animation every x milliseconds
            planets = new Vector();
          
            holdTime = 0;

            mainWindow.MouseMove += MainWindow_MouseMove;
            boxy.MouseDown += Boxy_MouseDown;
            boxy.MouseUp += Boxy_MouseUp;

            idleTimer.Interval = TimeSpan.FromMilliseconds(1000);

        }

        private void Boxy_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            TransformGroup t = new TransformGroup();
            t.Children.Add(new RotateTransform(0));
            t.Children.Add(new TranslateTransform(mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4, mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4));
            cursor.RenderTransform = t;

            //holdTime = holdTime - (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            //TODO: drinking animation
            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-drink.png", UriKind.Relative));
            boxy_status = DRINKING;

            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-happy.png", UriKind.Relative));
            boxy_status = HAPPY;

            if (!started)
            {
                MakeAllVisible();
                
            }

            // reset all planets
        }

        

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            
            Point mousePos = e.GetPosition(this);
            TranslateTransform transform = new TranslateTransform
            {
                X = mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4,
                Y = mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4
            };
            this.cursor.RenderTransform = transform;
                
        }

        private void Boxy_MouseDown(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(this);

            // holdTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            //clickTimer.Start();

            PourTea(mousePos);

            boxy.Source = new BitmapImage(new Uri(@"/img/boxy-pour.png", UriKind.Relative));
            boxy_status = POURING;
            
        }

        private void PourTea(Point mousePos)
        {
            teaPouring = true;
            TransformGroup t = new TransformGroup();
            t.Children.Add(new RotateTransform(-45));
            t.Children.Add(new TranslateTransform(mousePos.X - Canvas.GetLeft(cursor) - cursor.Width / 4, mousePos.Y - Canvas.GetTop(cursor) - cursor.Height / 4));
            cursor.RenderTransform = t;
            // play sound of pouring
            //pouring animation that loops
        }

        private void UpdatePlanets()
        {

        }
        
        private void MakeAllVisible()
        {
            space.Visibility = Visibility.Visible;
            orbits.Visibility = Visibility.Visible;

            btn_s1.Visibility = Visibility.Visible;
            btn_s2.Visibility = Visibility.Visible;
            btn_s3.Visibility = Visibility.Visible;
            btn_s4.Visibility = Visibility.Visible;

            btn_m1.Visibility = Visibility.Visible;
            btn_m2.Visibility = Visibility.Visible;
            btn_m3.Visibility = Visibility.Visible;
            btn_m4.Visibility = Visibility.Visible;

            btn_l1.Visibility = Visibility.Visible;
            btn_l2.Visibility = Visibility.Visible;
            btn_l3.Visibility = Visibility.Visible;
            btn_l4.Visibility = Visibility.Visible;
            started = true;
        }
        private void HideElements()
        {
            space.Visibility = Visibility.Hidden;
            orbits.Visibility = Visibility.Hidden;

            btn_s1.Visibility = Visibility.Hidden;
            btn_s2.Visibility = Visibility.Hidden;
            btn_s3.Visibility = Visibility.Hidden;
            btn_s4.Visibility = Visibility.Hidden;

            btn_m1.Visibility = Visibility.Hidden;
            btn_m2.Visibility = Visibility.Hidden;
            btn_m3.Visibility = Visibility.Hidden;
            btn_m4.Visibility = Visibility.Hidden;

            btn_l1.Visibility = Visibility.Hidden;
            btn_l2.Visibility = Visibility.Hidden;
            btn_l3.Visibility = Visibility.Hidden;
            btn_l4.Visibility = Visibility.Hidden;
        }

    }
}
